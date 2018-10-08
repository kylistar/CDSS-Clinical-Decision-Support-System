using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examproject
{
    class SimilarPatient
    {
        private Patient patient;
        private double distance;
        private int mostSimilarIndex;
        private int foot; // 0 for left 1 for right

        public SimilarPatient()
        {
            MostSimilarIndex = 0;
            distance = 0;
            foot = -1;
        }
        public SimilarPatient(Patient patient, int distance, int footRak)
        {
            Patient = patient;
            Distance = distance;
            Foot = footRak;
        }

        public double Distance { get => distance; set => distance = value; }
        public int Foot { get => foot; set => foot = value; }
        public int MostSimilarIndex { get => mostSimilarIndex; set => mostSimilarIndex = value; }
        internal Patient Patient { get => patient; set => patient = value; }
    }
}
