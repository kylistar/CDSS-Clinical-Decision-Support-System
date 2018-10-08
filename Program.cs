using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<SimilarPatient> similarPatients = new List<SimilarPatient>(); //  WHERE age_at_fysio BETWEEN 7 and 17
            Database DB = new Database("SELECT * FROM [Sheet1$] WHERE age_at_fysio BETWEEN 8 and 16 order by lpnr", "C:/Users/Emil/Dropbox/META/Fysio Vinnova");
            Database OPDB = new Database("SELECT * FROM [Sheet1$] order by lpnr", "C:/Users/Emil/Dropbox/META/OP Vinnova");
            Statistics stat = new Statistics();
            Search search = new Search(DB.Visits[0].Attributes.Count());
            search.Weights[1] = 1;
            search.Weights[4] = 2;
            search.Weights[15] = 1;
            search.Weights[181] = 2;

            for (int i = 0; i < DB.Visits.Count(); i++)
            {
                if ((double)DB.Visits[i].Attributes[0] == 1015)
                    Console.WriteLine();
            }

            similarPatients = search.findSimilarPatients(DB.Visits[545], DB);
            stat.getOperations(similarPatients, OPDB, DB.Visits[545]);
            int similarCount = 0, breake = 0;
            double top = 1, bot = 0.9;

            List<double> takenLpnr = new List<double>();

            List<SimilarPatient> hej = new List<SimilarPatient>();
            while (hej.Count() < 8)
            {
                foreach (KeyValuePair<String, List<SimilarPatient>> current in stat.opDict)
                {
                    if (current.Key != "noOp")
                    {
                        foreach (SimilarPatient patient in current.Value)
                        {
                            if (patient.Patient.Lpnr == 753)
                                Console.WriteLine();

                            if (patient.Distance > bot && patient.Distance < top && !takenLpnr.Contains(patient.Patient.Lpnr))
                            {
                                hej.Add(patient);
                                takenLpnr.Add(patient.Patient.Lpnr);
                                similarCount++;
                                breake = 1;
                                break;
                            }
                        }
                    }
                    if (breake == 1)
                    {
                        breake = 0;
                        break;
                    }
                }
                if (similarCount == 2)
                {
                    similarCount = 0;
                    top -= 0.1;
                    bot -= 0.1;
                }
            }


            foreach(Percentage x in stat.Operations)
            {
                if (x.Operation == "noOp")
                {
                    Console.WriteLine("Not operated patients amount: " + x.Amount + "\n-----\n");
                }
                else
                {
                    Console.WriteLine("Operation: " + x.Operation + " | Percentage: " + x.Percent + " | Amount: " + x.Amount + "\n-----\n");
                }
            }

            Console.WriteLine("\n For operation NHL69:\n " + stat.NrImproved + 
                " had at least 10 degrees wider range of motion then before operation, \nwhile " 
                + stat.NrDetoriated + " got at least a 10 degrees shorter range of motion,\n" 
                + (stat.NrOperated - stat.NrImproved - stat.NrDetoriated) + " patients no measureable difference before and after was measured");


            Console.WriteLine();
        }
    }
}
