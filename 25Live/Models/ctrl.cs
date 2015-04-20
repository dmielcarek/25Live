using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace _25Live.Models
{
    public class ctrl
    {
        //Read all the config variables from web.config file
        


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
                SqlConnection con = new SqlConnection(cs);
                SqlCommand com = new SqlCommand("Execute usp_25LiveDataInFile", con);
                com.CommandTimeout = 300;// 5 min
                com.Connection = con;
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(com);
                 
                // Delay(3000);
               
                da.Fill(dt);
                //Task.Delay(50000);
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
        // The method takes start index, length and value of a field and concatnate it with line variable
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

        public IDictionary<string, string> createDataInFile()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>(); // Will be used for return values
            String message = "";
            
            try
            {

                var allConfigurations = ReadAllSettings();
                var colDepartmentID = int.Parse(allConfigurations["ColDepartmentId"]);
                var lenDepartmentID = int.Parse(allConfigurations["LengthDepartmentId"]);
                var colCatalog = int.Parse(allConfigurations["ColCatalog"]);
                var lenCatalog = int.Parse(allConfigurations["LengthCatalog"]);
                var colSection = int.Parse(allConfigurations["ColSection"]);
                var lenSection = int.Parse(allConfigurations["LengthSection"]);
                var colDays = int.Parse(allConfigurations["ColDays"]);
                var lenDays = int.Parse(allConfigurations["LengthDays"]);
                var colStartHours = int.Parse(allConfigurations["ColStartHours"]);
                var colStartMinutes = int.Parse(allConfigurations["ColStartMinutes"]);
                var colFinishtHours = int.Parse(allConfigurations["ColFinishtHours"]);
                var colFinishtMinutes = int.Parse(allConfigurations["ColFinishtMinutes"]);
                var colAP = int.Parse(allConfigurations["ColAP"]);
                var colEnrollment = int.Parse(allConfigurations["ColEnrollment"]);
                var lenEnrollment = int.Parse(allConfigurations["LengthEnrollment"]);
                var colRoomName = int.Parse(allConfigurations["ColRoomName"]);
                var lenRoomName = int.Parse(allConfigurations["LengthRoomName"]);
                var colBeginYear = int.Parse(allConfigurations["ColBeginYear"]);
                var colBeginMonth = int.Parse(allConfigurations["ColBeginMonth"]);
                var colBeginDay = int.Parse(allConfigurations["ColBeginDay"]);
                var colEndYear = int.Parse(allConfigurations["ColEndYear"]);
                var colEndMonth = int.Parse(allConfigurations["ColEndMonth"]);
                var colEndDay = int.Parse(allConfigurations["ColEndDay"]);
                var colMeeting = int.Parse(allConfigurations["ColMeeting"]);
                var lenMeeting = int.Parse(allConfigurations["LengthMeeting"]);
                var colASM = int.Parse(allConfigurations["ColAssignment"]);
                var lenASM = int.Parse(allConfigurations["LengthAssignment"]);
                var colEvent = int.Parse(allConfigurations["ColEvent"]); // This can be ignored and leave space)s
                var lenEvent = int.Parse(allConfigurations["LengthEvent"]);
                var colInstructor = int.Parse(allConfigurations["ColInstructor"]);
                var lenInstructor = int.Parse(allConfigurations["LengthInstructor"]);
                var colCourse = int.Parse(allConfigurations["ColCourse"]);
                var lenCourse = int.Parse(allConfigurations["LengthCourse"]);
                var colTerm = int.Parse(allConfigurations["ColTerm"]);
                var lengthTerm = int.Parse(allConfigurations["LengthTerm"]);
                var colCRN = int.Parse(allConfigurations["ColCRN"]);
                var lenCRN = int.Parse(allConfigurations["LengthCRN"]);

                //Call the stored procedure to get data
                //loop through the records
                String classRows = getData();
                

                if (!string.IsNullOrEmpty(classRows))
                {
                    var objects = JArray.Parse(classRows); // parse as array 
                    var codeRanFlag = 0;
                    foreach (JObject root in objects)
                    {
                        var roomName = root.GetValue("RoomName").ToString().Trim();
                        var days = root.GetValue("Days").ToString().Trim();
                        var classID = root.GetValue("ClassID").ToString().Trim();
                        var startHours = root.GetValue("StartHours").ToString().Trim();
                        var startMinutes = root.GetValue("StartMinutes").ToString().Trim();
                        var finishHours = root.GetValue("FinishHours").ToString().Trim();
                        var finishMinutes = root.GetValue("FinishMinutes").ToString().Trim();
                        var apDesignator = root.GetValue("APDesignator").ToString().Trim();
                        var enrollment = root.GetValue("Enrollment").ToString().Trim();
                        var departmentID = root.GetValue("DepartmentID").ToString().Trim();
                        var startWeek = root.GetValue("StartWeek").ToString().Trim();
                        var finishWeek = root.GetValue("FinishWeek").ToString().Trim();
                        var crn = root.GetValue("CRN").ToString().Trim();
                        var courseName = root.GetValue("CourseName").ToString().Trim();
                        var section = root.GetValue("Section").ToString().Trim();
                        var catalog = root.GetValue("Catalog").ToString().Trim(); // Course Number
                        var term = root.GetValue("term").ToString().Trim(); // YearQuarterID
                        var instructorEmail = root.GetValue("WorkEmail").ToString().Trim();
                        var assignment = root.GetValue("AssignmentField").ToString().Trim();
                        var meetingNumber = root.GetValue("MeetingNumber").ToString().Trim();
                        //generate line
                        string line = "";
                        var spacesToAdd = 0; // Initialising it for reuse.
                        //Adding the spaces to make the field with length as specifed in lenField
                        spacesToAdd = lenDepartmentID - departmentID.Length;
                        if (spacesToAdd > 0)
                            departmentID = departmentID.PadRight(departmentID.Length + spacesToAdd, ' ');

                        line += departmentID;
                        line = concatNewField(line, colCatalog, lenCatalog, catalog);
                        line = concatNewField(line, colSection, lenSection, section);
                        line = concatNewField(line, colDays, lenDays, days);
                        line = concatNewField(line, colStartHours, startHours.Length, startHours);
                        line = concatNewField(line, colStartMinutes, startMinutes.Length, startMinutes);
                        line = concatNewField(line, colFinishtHours, finishHours.Length, finishHours);
                        line = concatNewField(line, colFinishtMinutes, finishMinutes.Length, finishMinutes);
                        line = concatNewField(line, colAP, apDesignator.Length, apDesignator);
                        line = concatNewField(line, colEnrollment, lenEnrollment, enrollment);
                        line = concatNewField(line, colRoomName, lenRoomName, roomName);
                        line = concatNewField(line, colBeginYear, startWeek.Length, startWeek);
                        line = concatNewField(line, colEndYear, finishWeek.Length, finishWeek);
                        line = concatNewField(line, colMeeting, lenMeeting, meetingNumber);
                        line = concatNewField(line, colASM, lenASM, assignment);
                        var Event = ""; // This field is optional and we will just fill this with spaces.
                        line = concatNewField(line, colEvent, lenEvent, Event);
                        line = concatNewField(line, colInstructor, lenInstructor, instructorEmail);
                        line = concatNewField(line, colCourse, lenCourse, courseName);
                        line = concatNewField(line, colTerm, lengthTerm, term);
                        line = concatNewField(line, colCRN, lenCRN, crn);
                        //line += System.Environment.NewLine;



                        //Write to a file
                        term = term.ToString().Trim();
                        var filename = "datain"+term + ".dat";
                        string path = allConfigurations["FilePath"] + filename;

                        if (codeRanFlag == 0)
                        {
                            // Check if the file exists.
                            //If it exists that archive that file and create a new file.
                            var archivePath = allConfigurations["ArchivePath"];
                            if (System.IO.File.Exists(path))
                            {
                                String timeStamp = GetTimestamp(DateTime.Now);
                                var newFileName = term + "_" + timeStamp + ".dat";
                                var newPath = archivePath + newFileName;
                                // Move the existing file to archive folder
                                System.IO.File.Move(path, newPath);
                                message += "File " + newFileName + " has been moved to " + archivePath + ".";

                            }
                            //Create new file and write the line in it
                            if (!System.IO.File.Exists(path))
                            {
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                                message += " A new file named " + filename + " is created at location " + allConfigurations["FilePath"] + ".";
                                //TextWriter tw = new StreamWriter(path);
                                file.WriteLine(line);
                                file.Close();
                            }
                            codeRanFlag = 1;
                        }
                        else
                        {
                            // Here ideally you should just be appending data but I am still checking if the file is not created to be safe

                            if (!System.IO.File.Exists(path))
                            {
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                                file.WriteLine(line);
                                file.Close();
                            }
                            else if (System.IO.File.Exists(path))
                            {
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                                file.WriteLine(line);
                                file.Close();
                            }                          
                           
                        }
                    }
                }
                //Session["message"] = message;
                dict["message"] = message;
                dict["status"] = "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception", ex);
                //Session["exception"] = ex.Message.ToString();
                dict["message"] = ex.Message.ToString();
                dict["status"] = "failure";

            }
            return dict;
        }
    }
}