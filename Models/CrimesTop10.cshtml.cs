using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;

namespace crimes.Pages
{
    public class CrimesTop10Model : PageModel
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
    SELECT TOP 10 Crimes.IUCR, PrimaryDesc, SecondaryDesc, Count(CID) AS NumOccur,
    ROUND(CAST(COUNT(CID) AS FLOAT)/(SELECT COUNT(CID) FROM Crimes)*100, 2) AS 'Crime %',
    ROUND(CAST(COUNT(CASE WHEN Arrested = 1 THEN 1 END)AS FLOAT)/COUNT(Crimes.IUCR)*100, 2) AS 'Arrested %'
    FROM Crimes INNER JOIN codes ON Crimes.IUCR = Codes.IUCR
    GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
    ORDER BY numOccur DESC;
    ");
                    DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
                    
                    foreach (DataRow row in ds.Tables["TABLE"].Rows)
                    {
                        Models.Crime c = new Models.Crime();
                        
                        c.IUCR = Convert.ToString(row["IUCR"]);
                        c.PrimaryDesc = Convert.ToString(row["PrimaryDesc"]);
                        c.SecondaryDesc = Convert.ToString(row["SecondaryDesc"]);
                        c.NumOccur = Convert.ToInt32(row["NumOccur"]);
                        c.CrimePer = Convert.ToDouble(row["Crime %"]);
                        c.ArrestedPer = Convert.ToDouble(row["Arrested %"]);
                        
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