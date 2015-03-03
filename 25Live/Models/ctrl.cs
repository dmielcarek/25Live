using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace _25Live.Models
{
    public class ctrl
    {

        public dynamic ReadAllSettings()
        {
            
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    /*
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                        * */
                    return appSettings;
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return null;
        }

        public string ReadSetting(string key)
        {
            string result = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
                //return result;
                //Console.WriteLine(result);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return result;
        }

        public String getData()
        {
            try
            {
                DataTable dt = new DataTable();
                String cs = System.Configuration.ConfigurationManager.ConnectionStrings["ClassDal"].ConnectionString;
                //SqlDataReader reader;
                SqlConnection con = new SqlConnection(cs);
                SqlCommand com = new SqlCommand("Execute usp_25LiveDataInFile", con);
                //cmd.CommandText = "usp_25LiveDataInFile";
                //cmd.CommandType = CommandType.StoredProcedure;
                com.Connection = con;
                //return (Boolean)com.ExecuteScalar();
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(com);
                 Task.Delay(10000);
                // Delay(3000);
                da.Fill(dt);
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                return serializer.Serialize(rows);
                /*
                reader = com.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = reader[0];
                    }
                }*/

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception", ex);
            }
            return null;
            

        }

        public string concatNewField(string line, int colNewField, int lenNewField, string valueNewField )
        {
            var spacesToAdd = 0; // Initialising it for reuse.
            var diffSpacesBetFields = 0; //subtract colNextField and length of the line and add the resultant number of spaces and concat with the next field value
            //subtract colNextField and length of the line and add the resultant number of spaces and concat with the next field value
            diffSpacesBetFields = colNewField - line.Length - 1;
            if (diffSpacesBetFields > 0)
                line = line.PadRight(line.Length + diffSpacesBetFields, ' ');
            spacesToAdd = lenNewField - valueNewField.Length;
            if (spacesToAdd > 0)
            {
                valueNewField = valueNewField.PadRight(valueNewField.Length + spacesToAdd, ' ');
            }
            line += valueNewField;

            return line;

        }
        public String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }
    }
}