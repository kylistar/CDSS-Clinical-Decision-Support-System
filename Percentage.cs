using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Percentage
    {
        private string operation;
        private int amount;
        private double percent;


        public Percentage()
        {

        }
        public Percentage(string operation, double percent, int amount)
        {
            Percent = percent;
            Operation = operation;
            Amount = amount;
        }

        public double Percent { get => percent; set => percent = value; }
        public string Operation { get => operation; set => operation = value; }
        public int Amount { get => amount; set => amount = value; }
    }
}
