//
// One Crime
//

namespace crimes.Models
{
    
    public class Crime
    {
        public string IUCR { get; set; }
        public string PrimaryDesc { get; set; }
        public string SecondaryDesc { get; set; }
        public int NumOccur { get; set; }
        public double CrimePer { get; set; }
        public double ArrestedPer { get; set; }
        public int Month { get; set; }
        public int NumCrime { get; set; }
        
        public Crime()
        { }
        
        public Crime(string iucr, string primary, string secondary, int numOccur, double crimePer, double arrestedPer, int month, int numCrime)
        {
            IUCR = iucr;
            PrimaryDesc = primary;
            SecondaryDesc = secondary;
            NumOccur = numOccur;
            CrimePer = crimePer;
            ArrestedPer = arrestedPer;
            Month = month;
            NumCrime = numCrime;
        }
    }
}