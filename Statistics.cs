using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Statistics
    {
        private int nrImproved, nrOperated, nrDetoriated;
        List<Percentage> operations = new List<Percentage>();
        public List<Percentage> Operations { get => operations; set => operations = value; }
        public int NrImproved { get => nrImproved; set => nrImproved = value; }
        public int NrOperated { get => nrOperated; set => nrOperated = value; }
        public int NrDetoriated { get => nrDetoriated; set => nrDetoriated = value; }

        public Dictionary<string, List<SimilarPatient>> opDict = new Dictionary<string, List<SimilarPatient>>()
        {
                {"noOp",new List<SimilarPatient>()},
                {"NHL69",new List<SimilarPatient>()},
                {"NHG99",new List<SimilarPatient>()},
                {"SDR",new List<SimilarPatient>()},
                {"NHL89",new List<SimilarPatient>()},
                {"NHK55",new List<SimilarPatient>()},
                {"NHL79",new List<SimilarPatient>()},
                {"NHG39",new List<SimilarPatient>()}
            };

        public Statistics()
        {
            NrOperated = 0;
            NrImproved = 0;
            NrDetoriated = 0;
        }

        public void getOperations(List<SimilarPatient> simPat, Database OPDB, Visit testPatient)
        {
            bool oprelevance = false;
            string opFound = "";
            Queue<Visit> opStack = new Queue<Visit>(OPDB.Visits);
            Visit tempVis;
            string operation = "";
            int[] opColNr = { 16, 20, 23, 26, 27 };
            tempVis = opStack.Dequeue();
            foreach (SimilarPatient patient in simPat)
            {
                while (patient.Patient.Lpnr > (double)tempVis.Attributes[0] && opStack.Count() != 0)
                {
                    tempVis = opStack.Dequeue();
                }
                if (patient.Patient.Lpnr == (double)tempVis.Attributes[0]) // check if same patient and if operation was after similarity
                {
                    while (patient.Patient.Lpnr == (double)tempVis.Attributes[0])
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            operation = SearchOperation((string)tempVis.Attributes[opColNr[i]]);
                            oprelevance = OpRelevance((double)tempVis.Attributes[4], patient.Patient);
                            if (operation != "" && oprelevance == true && PatientRelevance((double)tempVis.Attributes[4], patient, testPatient)) // kolla så att någon operation blev gjord
                            {
                                if (i < 3) // kolla bara höger/vänster för index 1-3
                                {
                                    //checka om det är samma som patienten dvs vänster lr höger på rak fot
                                    if ((double)tempVis.Attributes[opColNr[i] + 1] == 1 &&  //höger fot?
                                        patient.Foot == 1)
                                    {
                                        opFound = operation;
                                    }

                                    if ((double)tempVis.Attributes[opColNr[i] + 2] == 1 && //vänster fot?
                                        patient.Foot == 0)
                                    {
                                        opFound = operation;
                                    }

                                    if ((double)tempVis.Attributes[opColNr[i] + 2] == 0 && // tar med alla där det inte står vilken fot för mer träffar
                                        (double)tempVis.Attributes[opColNr[i] + 1] == 0)
                                    {
                                        opFound = operation;
                                    }
                                }
                                else
                                {
                                    opFound = operation;  // lägger till alla(kollar inte vänster lr höger) på sista för mer träffar
                                }
                            }
                        }
                        if (opFound != "")
                        {
                            patient.Patient.OperationAge = (double)tempVis.Attributes[4];
                            opDict[opFound].Add(patient);
                            opFound = "";
                        }
                        else if (oprelevance == true)
                        {
                            // om den inte hittade nån operation för rätt fot lägg till den i noOp
                            opDict["noOp"].Add(patient);
                        }
                        tempVis = opStack.Dequeue();
                    }
                
                }
                else
                {
                    opDict["noOp"].Add(patient);
                }
            }
            calcOpStat();
            getImprovement();
        }
        private bool PatientRelevance(double ageOfOp, SimilarPatient patient, Visit testPatient)
        {
            if (patient.Patient.Lpnr == 1545)
                Console.WriteLine();



            if (ageOfOp > (double)patient.Patient.Visits[patient.MostSimilarIndex].Attributes[4])
                return true;

            Search searchNewMostSimilar = new Search(testPatient.Attributes.Count());
            SimilarPatient temp;
            int i = 0;
            searchNewMostSimilar.Weights[1] = 1;
            searchNewMostSimilar.Weights[4] = 2;
            searchNewMostSimilar.Weights[15] = 1;
            searchNewMostSimilar.Weights[181] = 2;

            foreach (Visit visit in patient.Patient.Visits)
            {
                temp = searchNewMostSimilar.comparePatients(testPatient, visit);
                if (ageOfOp > (double)visit.Attributes[4] && temp.Distance > 0.5)
                {
                    patient.MostSimilarIndex = i;
                    return true;
                }
                i++;
            }

            return false;
        }
        private bool OpRelevance(double ageOfOp, Patient patient)
        {
            if(ageOfOp < (double)patient.Visits[0].Attributes[4] && 
                ageOfOp > (double)patient.Visits[(patient.Visits.Count() - 1)].Attributes[4])
            {
                return true;
            }
            return false;
        }
        private void getImprovement()
        {
            int beforeIndex = -1, afterIndex = -1;
            double currentImprovement;
            int i;
            double beforeBest = -99, afterBest = 0, current;
            List<double> distances = new List<double>();
            foreach(SimilarPatient patient in opDict["NHL69"])
            {
                beforeIndex = -1;
                afterIndex = -1;
                beforeBest = -99;
                afterBest = 0;

                NrOperated++;
                for(i = (patient.Patient.Visits.Count() - 1); i >= 0 ; i--)
                {
                    current = (double)patient.Patient.Visits[i].Attributes[4] - patient.Patient.OperationAge;
                    distances.Add(current);

                    if (beforeBest < current && current < 0)
                    {
                        beforeIndex = i;
                        beforeBest = current;
                    }
                    if(afterBest < current && current >= 0 && current < 2)
                    {
                        afterIndex = i;
                        afterBest = current;
                    }
                }
                if(afterIndex == -1)  // om ingen inom 2 år fanns kollar vi på den utanför 2 år
                {
                    afterIndex = patient.Patient.Visits.Count() - 1;
                }
                if (patient.Foot == 0) //vänster check
                {
                    currentImprovement = (double)patient.Patient.Visits[afterIndex].Attributes[181]
                        - (double)patient.Patient.Visits[beforeIndex].Attributes[181];
                }else if(patient.Foot == 1) // höger check
                {
                    currentImprovement = (double)patient.Patient.Visits[afterIndex].Attributes[182]
                        - (double)patient.Patient.Visits[beforeIndex].Attributes[182];
                }
                else // ta bara bästa foten för prototypens skull just nu
                {
                    currentImprovement = Math.Max((double)patient.Patient.Visits[afterIndex].Attributes[181]
                        - (double)patient.Patient.Visits[beforeIndex].Attributes[181], (double)patient.Patient.Visits[afterIndex].Attributes[182]
                        - (double)patient.Patient.Visits[beforeIndex].Attributes[182]);
                }
                if(currentImprovement > 9)
                {
                    NrImproved++;
                }
                else if (currentImprovement < -9)
                {
                    NrDetoriated++;
                }

            }
        }
        private void calcOpStat()
        {
            double nrPatients = 0;
            double percentage = 0;

            foreach(KeyValuePair<String, List<SimilarPatient>> current in opDict)
            {
                if(current.Key != "noOp")
                    nrPatients += current.Value.Count();
            }
            foreach (KeyValuePair<String, List<SimilarPatient>> current in opDict)
            {
                if (nrPatients == 0 || current.Value.Count() == 0)
                {
                }
                else
                {
                    percentage = 100 * (current.Value.Count() / nrPatients);
                    Operations.Add(new Percentage(current.Key, percentage, current.Value.Count()));
                }

            }
        }
        private string SearchOperation(string s)
        {
            string[] OpArray = new string[] { "NHL69", "NHG99", "SDR", "NHL89", "NHK55", "NHL79", "NHG39" };
            string s2 = "";
            for (int i = 0; i < OpArray.Length; i++)
            {
                if (s.Contains(OpArray[i]))
                {
                    s2 = OpArray[i];
                    return s2;
                }
            }
            return "";
        }
    }
}
