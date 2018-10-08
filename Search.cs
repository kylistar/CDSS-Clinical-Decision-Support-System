using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class Search
    {
        int nrColumns;
        private List<double> weights;
        public List<double> Weights { get => weights; set => weights = value; }
        public Search(int nrColumns)
        {
            Weights = new List<double>();
            this.nrColumns = nrColumns;
            initWeights();
        }
        public List<SimilarPatient> findSimilarPatients(Visit patient, Database DB)
        {
            int mostSimilarIndex = 0;
            double currentLp = (double)DB.Visits[0].Attributes[0];
            List<SimilarPatient> simPat = new List<SimilarPatient>();
            SimilarPatient temp, currentBest = new SimilarPatient();
            Patient tempPat = new Patient();
            for (int i = 0; i < DB.Visits.Count(); i++)
            {
                if ((double)DB.Visits[i].Attributes[0] == 1077)
                    Console.WriteLine();

                temp = comparePatients(patient, DB.Visits[i]);
                if(currentLp == (double)DB.Visits[i].Attributes[0])
                {
                    tempPat.Visits.Add(DB.Visits[i]);
                }
                else
                {
                    if (currentBest.Distance > 0.5)  // när ny patient påträffas, lägg till den gamla och börja på en ny
                    {
                        currentBest.Patient = tempPat;
                        currentBest.Patient.Lpnr = currentLp;
                        simPat.Add(currentBest);
                    }
                    mostSimilarIndex = 0;
                    currentLp = (double)DB.Visits[i].Attributes[0];
                    currentBest = new SimilarPatient();
                    tempPat = new Patient();
                    tempPat.Visits.Add(DB.Visits[i]);
                }
                if(temp.Distance > currentBest.Distance)
                {
                    currentBest = temp;
                    currentBest.MostSimilarIndex = mostSimilarIndex;
                }
                mostSimilarIndex++;

            }
            return simPat;
        }
        public SimilarPatient comparePatients(Visit p1, Visit p2)
        {
            if ((double)p2.Attributes[0] == 1077)
                Console.WriteLine();

            SimilarPatient tempPatient = new SimilarPatient();
                //(double)DB.Patients[i].Attributes[0], comparePatients(patient, DB.Patients[i])
            Match matcher = new Match();
            double distance , top = 0, bottom = 0, left, right;
            for (int i = 0; i < nrColumns; i++)
            {
                switch (i)
                {
                    case 1:
                        top += Weights[i] * matcher.matchString((string)p1.Attributes[i], (string)p2.Attributes[i]);
                        break;
                    case 4:
                        top += Weights[i] * matcher.matchAge((double)p1.Attributes[i], (double)p2.Attributes[i]);
                        break;
                    case 15:
                        top += Weights[i] * matcher.matchGMFCS((string)p1.Attributes[i], (string)p2.Attributes[i]);
                        break;
                    case 181:// vänster rak
                        if (p1.Attributes[i] == DBNull.Value || p2.Attributes[i] == DBNull.Value || p2.Attributes[182] == DBNull.Value)
                            break;

                        left = matcher.matchAngle((double)p1.Attributes[i], (double)p2.Attributes[i]);
                        right = matcher.matchAngle((double)p1.Attributes[i], (double)p2.Attributes[i + 1]);

                        if (left == right && left == 0)
                        {
                            tempPatient.Foot = 0;
                            break;
                        }
                        if (left >= right) // left if it is more similar eller lika med höger annars höger
                        {
                            top += Weights[i] * matcher.matchAngle((double)p1.Attributes[i], (double)p2.Attributes[i]);
                            tempPatient.Foot = 0;
                            break;
                        }
                        
                        top += Weights[i] * matcher.matchAngle((double)p1.Attributes[i], (double)p2.Attributes[182]);
                        tempPatient.Foot = 1;

                        break;
                }
                bottom += Weights[i]; 
            }
            distance = top / bottom;
            tempPatient.Distance = distance;
            return tempPatient;
        }
        private void initWeights()
        {
            for(int i = 0; i < nrColumns; i++)
            {
                Weights.Add(0);
            }
        }


    }
}
