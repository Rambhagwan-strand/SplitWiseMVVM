using SplitWiseMVVM.Model;
using SplitWiseMVVM.View.Command;
using SplitWiseMVVM.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SplitWiseMVVM.ViewModel
{
    class SplitWiseVM : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string moneyGiven;

        public string MoneyGiven
        {
            get { return moneyGiven; }
            set 
            {
                if (moneyGiven != value)
                {
                    moneyGiven = value;
                    OnPropertyChanged("MoneyGiven");
                }
                
            }
        }

        public void AddNewUserGroup()
        {
            GroupOfUsers.Add(new List<User>());
        }





        private string share;

        public string Share
        {
            get { return share; }
            set 
            {
                share = value;
                OnPropertyChanged("Share");
            }
        }

        public ObservableCollection<List<User>> GroupOfUsers { get; set; }
        public ObservableCollection<string> ResultOfSettlement { get; set; }

        private List<User> currentlySelectedUserList;

        public List<User> CurrentlySelectedUserList
        {
            get { return currentlySelectedUserList; }
            set
            {
                currentlySelectedUserList = value;
                OnPropertyChanged("CurrentlySelectedUserList");
                SettlementForCurrentGroup(CurrentlySelectedUserList, ResultOfSettlement);
            }
        }

        private int selectedListIndex;

        public int SelectedListIndex
        {
            get { return selectedListIndex; }
            set 
            {
                selectedListIndex = value; 

            }
            
        }


        public void addToUsers()
        {
            if (string.IsNullOrWhiteSpace(Share))
                Share = "1";
            try
            {
                User user = new User(Name, Convert.ToInt32(MoneyGiven), Convert.ToDouble(Share));
                GroupOfUsers[GroupOfUsers.Count - 1].Add(user);
                Name = "";
                MoneyGiven = "";
                Share = "";
            } catch (Exception e)
            {
                MessageBox.Show("Plese check your input type,\n Amount paid should be integer (eg 10) \n Share should be a float value or integer (eg. 15.7, 5) \n" + e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public AddTransactionCommand AddTransactionCommand { get; set; }
        public CalculateSettlementCommand CalculateSettlementCommand { get; set; }

        public AddGroupCommand AddGroupCommand { get; set; }
        public SplitWiseVM()
        {
            GroupOfUsers = new ObservableCollection<List<User>>();
            GroupOfUsers.Add(new List<User>());
            ResultOfSettlement = new ObservableCollection<string>();
            AddTransactionCommand = new AddTransactionCommand(this);
            CalculateSettlementCommand = new CalculateSettlementCommand(this);
            AddGroupCommand = new AddGroupCommand(this);
            
        }




        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SettlementForCurrentGroup(List<User> Users, ObservableCollection<string> ResultOfSettlement)
        {
            //No person is there so no settlement required.
            ResultOfSettlement.Clear();

            if (Users.Count == 0)
            {
                return;
            }
            

            double SumOfRatio = 0;
            double totalMoney = 0;
            double MoneyAdjustmentAfterSettlement = 0;
            double ExpectedFromCurrentPerson;
            string Dialogue;
            for (int i = 0; i < Users.Count; i++)
            {
                SumOfRatio += Users[i].share;
                totalMoney += Users[i].moneyGiven;
            }
            if (SumOfRatio == 0)
            {
                Console.WriteLine("Total sum of share can not be zero");
            }
            else
            {
                ResultOfSettlement.Add("Current Group Calculation");
                for (int i = 0; i < Users.Count; i++)
                {

                    ExpectedFromCurrentPerson = (double)(Users[i].share / SumOfRatio) * totalMoney;
                    MoneyAdjustmentAfterSettlement = Math.Round(Users[i].moneyGiven - ExpectedFromCurrentPerson, 2);
                    if (MoneyAdjustmentAfterSettlement >= 0)
                    {
                        Dialogue = "need to receive";
                    }
                    else
                    {
                        Dialogue = "need to pay";
                    }
                    string ans = Users[i].name + " " + Dialogue + " " + Convert.ToString(Math.Abs(MoneyAdjustmentAfterSettlement));

                    ResultOfSettlement.Add(ans);
                }
                ResultOfSettlement.Add("***************************************************");

            }

        }

        public void PrintSettledShareForAllGroup(ObservableCollection<List<User>> Users, ObservableCollection<string> ResultOfSettlement)
        {
            //No person is there so no settlement required.
            if (Users.Count == 0)
            {
                return;
            }
            ResultOfSettlement.Clear();
            string CurrentPerson;
            string Dialogue;
            double CurrentSettlement;
            ResultOfSettlement.Add("All Groups Calculation");
            Dictionary<String, double> FinalSettlement = new Dictionary<string, double>();
            for (int indexOfUserGroup = 0; indexOfUserGroup < Users.Count; indexOfUserGroup++)
            {
                double SumOfRatio = 0;
                double totalMoney = 0;
                double MoneyAdjustmentAfterSettlement = 0;
                double ExpectedFromCurrentPerson;
                
                for (int i = 0; i < Users[indexOfUserGroup].Count; i++)
                {
                    SumOfRatio += Users[indexOfUserGroup][i].share;
                    totalMoney += Users[indexOfUserGroup][i].moneyGiven;
                }
                if (SumOfRatio == 0)
                {
                    Console.WriteLine("Total sum of share can not be zero");
                }
                else
                {
                    
                    for (int i = 0; i < Users[indexOfUserGroup].Count; i++)
                    {

                        ExpectedFromCurrentPerson = (double)(Users[indexOfUserGroup][i].share / SumOfRatio) * totalMoney;
                        MoneyAdjustmentAfterSettlement = Users[indexOfUserGroup][i].moneyGiven - ExpectedFromCurrentPerson;
                        if (FinalSettlement.ContainsKey(Users[indexOfUserGroup][i].name))
                        {
                            FinalSettlement[Users[indexOfUserGroup][i].name] += MoneyAdjustmentAfterSettlement;
                        } else
                        {
                            FinalSettlement[Users[indexOfUserGroup][i].name] = MoneyAdjustmentAfterSettlement;
                        }

                    }
                    

                }
            }
            foreach (KeyValuePair<string, double> entry in FinalSettlement)
            {
                CurrentPerson = entry.Key;
                CurrentSettlement = Math.Round(entry.Value, 2);
                if (CurrentSettlement >= 0)
                {
                    Dialogue = "need to receive";
                }
                else
                {
                    Dialogue = "need to pay";
                }
                string ans = CurrentPerson + " " + Dialogue + " " + Convert.ToString(Math.Abs(CurrentSettlement));
                ResultOfSettlement.Add(ans);

            }

            ResultOfSettlement.Add("***************************************************");


        }
    }
}
