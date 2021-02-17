using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;

namespace crimes.Pages
{
    public class AreasListModel : PageModel
    {
        public List<Models.ChicagoArea> AreaList { get; set; }
        public Exception EX { get; set; }
        
        public void OnGet()
        {
            List<Models.ChicagoArea> crimes = new List<Models.ChicagoArea>();
                
            // clear exception:
            EX = null;

            try
            {
                string sql = string.Format(@"
                SELECT Crimes.Area, AreaName, COUNT(CID) AS NumCrimes,
                ROUND(CAST(COUNT(CID) AS FLOAT)/(SELECT COUNT(CID) FROM Crimes)*100, 2) AS 'Crime %'
                FROM Crimes INNER JOIN Areas ON Crimes.Area = Areas.Area 
                WHERE Crimes.Area != 0
                GROUP BY Crimes.Area, AreaName
                ORDER BY AreaName ASC;
                ");
                
                DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
                    
                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    Models.ChicagoArea c = new Models.ChicagoArea();
                    
                    c.AreaNum = Convert.ToInt32(row["Area"]);
                    c.AreaName = Convert.ToString(row["AreaName"]);
                    c.NumCrimes = Convert.ToInt32(row["NumCrimes"]);
                    c.CrimePer = Convert.ToDouble(row["Crime %"]);
                    
                    crimes.Add(c);
                }
             }
            catch(Exception ex)
            {
                EX = ex;
            }
            finally
            {
                AreaList = crimes;
            }
        }
    } 
}             