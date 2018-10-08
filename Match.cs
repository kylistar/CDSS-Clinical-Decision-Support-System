using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Match
    {
        public Match()
        {

        }
        public double matchAge(double age1, double age2)
        {
            double distance;

            distance = Math.Sqrt(Math.Pow(age1 - age2, 2));

            if (distance < 4)
                return 1;



            return 0;
        }
        public double matchGMFCS(string p1, string p2)
        {
            if (p2 == "")
                return 1;

            int intPat1, intPat2;
            double distance;

            intPat1 = convertGMFCS(p1);
            intPat2 = convertGMFCS(p2);

            distance = Math.Sqrt(Math.Pow(intPat1 - intPat2, 2));

            if (distance == 0)
                return 1;

            if (distance == 1)
                return 0.75;

            return 0;
        }
        public int convertGMFCS(string s)
        {

            if (s.Contains("I"))
            {
                return 1;
            }
            else if (s.Contains("II"))
            {
                return 2;
            }
            else if (s.Contains("III"))
            {
                return 3;
            }
            else if (s.Contains("V"))
            {
                return 4;
            }
            else if (s.Contains("IV"))
            {
                return 5;
            }

            return 0;
        }
        public double matchAngle(double a1, double a2)
        {
            double distance = Math.Sqrt(Math.Pow(a1 - a2, 2));

            if (distance <= 5)
                return 1;

            if (distance <= 10)
                return 0.5;

            //if (distance <= 15)
            //    return 0.5;

            return 0;
        }
        public double matchString(string string1, string string2) // både för gender
        {
            if (string1 == string2)
            return 1;

            return 0;
        }
    }
}
