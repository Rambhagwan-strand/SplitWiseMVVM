using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitWiseMVVM.Model
{
    public class User
    {
        public string name { get; set; }
        public int moneyGiven { get; set; }
        public double share { get; set; }

        public User(string customerName, int money, double shareInPercentage)
        {
            this.name = customerName;
            this.moneyGiven = money;
            this.share = shareInPercentage;
        }

        public override string ToString()
        {
            return $"{name} {moneyGiven} {share}";
        }
    }
}
