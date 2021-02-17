using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;

namespace crimes.Pages
{
    public class AreaCrimesPerMonthModel: PageModel
    {
        public List<Models.Crime> CrimeList { get; set; }     
        public Exception EX { get; set; }
        public string Input { get; set; }
        
        public void OnGet(string input)
        {
            List<Models.Crime> crimes = new List<Models.Crime>();
					
            // make input available to web page:
            Input = input;
            
            //clear exception:
            EX = null;
            
            try
            {
                if(input == null)
                {
                    //nothing to do.
                }
                else
                {
                    int id;
                    string sql;
                    
                    if(System.Int32.TryParse(input, out id))
                    {
                        //lookup area by area id
                        sql = string.Format(@"
                        SELECT MONTH(CrimeDate) AS Month, COUNT(CrimeDate) AS NumCrime,
                        ROUND(CAST(COUNT(CrimeDate) AS FLOAT)/(SELECT COUNT(CrimeDate) FROM Crimes INNER JOIN Areas 
                        ON Crimes.Area = Areas.Area WHERE Crimes.Area = {0} )*100, 2) AS 'Crime %' 
                        FROM Crimes INNER JOIN Areas ON Crimes.Area = Areas.Area
                        WHERE Crimes.Area = {0}
                        GROUP BY MONTH(CrimeDate)
                        ORDER BY Month ASC;
                        ", id);
                    }
                    else
                    {
                        //lookup area by partial name match
                        input = input.Replace("'", "''");
                        
                        sql = string.Format(@"
                        SELECT MONTH(CrimeDate) AS Month, COUNT(CrimeDate) AS NumCrime,
                        ROUND(CAST(COUNT(CrimeDate) AS FLOAT)/(SELECT COUNT(CrimeDate) FROM Crimes INNER JOIN Areas 
                        ON Crimes.Area = Areas.Area WHERE AreaName LIKE '{0}' )*100, 2) AS 'Crime %' 
                        FROM Crimes INNER JOIN Areas ON Crimes.Area = Areas.Area
                        WHERE AreaName LIKE '{0}'
                        GROUP BY MONTH(CrimeDate)
                        ORDER BY Month ASC
                        ", input);
                    }
                    
					DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);
                    
                    if(ds.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception("Area not found");
                    }
					foreach (DataRow row in ds.Tables["TABLE"].Rows)
                    {
                        Models.Crime c = new Models.Crime();
                        
                        c.Month = Convert.ToInt32(row["Month"]);
                        c.NumCrime = Convert.ToInt32(row["NumCrime"]);
                        c.CrimePer = Convert.ToDouble(row["Crime %"]);
                        
                        crimes.Add(c);
                    }
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
    }
    
}