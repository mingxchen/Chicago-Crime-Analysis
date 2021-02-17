using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;

namespace crimes.Pages
{
    public class AreaCrimesTop10Model : PageModel
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
                        SELECT TOP 10 Crimes.IUCR, PrimaryDesc, SecondaryDesc, Count(CID) AS NumOccur,
                        ROUND(CAST(COUNT(CID) AS FLOAT)/(SELECT COUNT(CID) FROM Crimes INNER JOIN Areas ON 
                        Crimes.Area = Areas.Area WHERE Crimes.Area = {0})*100, 2) AS 'Crime %',
                        ROUND(CAST(COUNT(CASE WHEN Arrested = 1 THEN 1 END)AS FLOAT)/COUNT(Crimes.IUCR)*100, 2) AS 'Arrested %'
                        FROM Crimes INNER JOIN codes ON Crimes.IUCR = Codes.IUCR INNER JOIN Areas ON Crimes.Area = Areas.Area
                        WHERE Crimes.Area = {0}
                        GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
                        ORDER BY numOccur DESC;
                        ", id);
                    }
                    else
                    {
                        //lookup area by partial name match
                        input = input.Replace("'", "''");
                        
                        sql = string.Format(@"
                        SELECT TOP 10 Crimes.IUCR, PrimaryDesc, SecondaryDesc, Count(CID) AS NumOccur,
                        ROUND(CAST(COUNT(CID) AS FLOAT)/(SELECT COUNT(CID) FROM Crimes INNER JOIN Areas ON 
                        Crimes.Area = Areas.Area WHERE AreaName LIKE '%Rogers%')*100, 2) AS 'Crime %',
                        ROUND(CAST(COUNT(CASE WHEN Arrested = 1 THEN 1 END)AS FLOAT)/COUNT(Crimes.IUCR)*100, 2) AS 'Arrested %'
                        FROM Crimes INNER JOIN codes ON Crimes.IUCR = Codes.IUCR INNER JOIN Areas ON Crimes.Area = Areas.Area
                        WHERE AreaName LIKE '%{0}%'
                        GROUP BY Crimes.IUCR, PrimaryDesc, SecondaryDesc
                        ORDER BY numOccur DESC;
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
                        
                        c.IUCR = Convert.ToString(row["IUCR"]);
                        c.PrimaryDesc = Convert.ToString(row["PrimaryDesc"]);
                        c.SecondaryDesc = Convert.ToString(row["SecondaryDesc"]);
                        c.NumOccur = Convert.ToInt32(row["NumOccur"]);
                        c.CrimePer = Convert.ToDouble(row["Crime %"]);
                        c.ArrestedPer = Convert.ToDouble(row["Arrested %"]);
                        
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