//
//One Area
//

namespace crimes.Models
{
    
    public class ChicagoArea
    {
        public int AreaNum { get; set; }
        public string AreaName { get; set; }
        public int NumCrimes { get; set; }
        public double CrimePer { get; set; }
        
        public ChicagoArea()
        { }
        
        public ChicagoArea(int area, string areaName, int numCrimes, double crimePer)
        {
            AreaNum = area;
            AreaName = areaName;
            NumCrimes = numCrimes;
            CrimePer = crimePer;
        }
    }
}
