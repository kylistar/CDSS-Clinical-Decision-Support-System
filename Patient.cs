using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Patient
    {
        private List<Visit> visits = new List<Visit>();
        internal List<Visit> Visits { get => visits; set => visits = value; }
        public double OperationAge { get => operationAge; set => operationAge = value; }
        public double Lpnr { get => lpnr; set => lpnr = value; }

        private double lpnr;
        private double operationAge;

        public Patient()
        {
            Lpnr = 0;
            OperationAge = 0;
        }


    }
}
