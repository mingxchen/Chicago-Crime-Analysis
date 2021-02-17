using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;

namespace crimes.Pages
{
    public class DeadliestMonthsModel : PageModel
    {
        public List<Models.Crime> CrimeList {get; set; }
                public Exception EX {get; set; }
                
        public void OnGet()
        {
                List<Models.Crime> crimes = new List<Models.Crime>();
                
                // clear exception:
                EX = null;
                
                try
                {
                    string sql = string.Format(@"
                    SELECT MONTH(CrimeDate) AS Month, COUNT(CrimeDate) AS NumOccur,
                    ROUND(CAST(COUNT(CrimeDate) AS FLOAT)/(SELECT COUNT(CrimeDate) FROM Crimes INNER JOIN Codes  
                    ON Crimes.IUCR = Codes.IUCR WHERE PrimaryDesc = 'HOMICIDE')*100, 2) AS 'Homicide %'
                    FROM Crimes INNER JOIN Codes  ON Crimes.IUCR = Codes.IUCR
                    WHERE PrimaryDesc = 'HOMICIDE'
                    GROUP BY MONTH(CrimeDate) 
                    ORDER BY 'Homicide %' DESC
                    ");
                    
                    DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
                    
                    foreach (DataRow row in ds.Tables["TABLE"].Rows)
                    {
                        Models.Crime c = new Models.Crime();
                        
                        c.Month = Convert.ToInt32(row["Month"]);
                        c.NumOccur = Convert.ToInt32(row["NumOccur"]);
                        c.CrimePer = Convert.ToDouble(row["Homicide %"]);
                        
                        crimes.Add(c);
                    }
                }
                catch(Exception ex)
                {
                    EX = ex;
				}
				finally
				{
                    CrimeList = crimes;
                }
        }
    }//class
}//namespace