//
// One CrimeCode
//

namespace crimes.Models
{
    
    public class CrimeCode
    {
        public string IUCR { get; set; }
        public string PrimaryDesc { get; set; }
        public string SecondaryDesc { get; set; }
        public int NumOccur { get; set; }
        
        public CrimeCode()
        { }
        
        public CrimeCode(string iucr, string primary, string secondary, int numOccur)
        {
            IUCR = iucr;
            PrimaryDesc = primary;
            SecondaryDesc = secondary;
            NumOccur = numOccur;
        }
    }
}