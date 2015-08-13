using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace _25Live.Models
{
    public class ctrl
    {
        //Read all the config variables from web.config file
        _25Live_New.lccTools.lccToolsClass lccTCTools = new _25Live_New.lccTools.lccToolsClass();
        public void ReadAllSettings()
        {
            
            try
            {
                lccTCTools.lccSCSettings.lccFBuildConfigurations(ConfigurationManager.AppSettings);
                lccTCTools.lccSCSettings.lccSLogLevels = lccTCTools.lccFGetConfiguration("lccLogLevels");
                lccTCTools.lccSCSettings.lccSLogPath = lccTCTools.lccFGetConfiguration("lccLogPath");
                lccTCTools.lccSCSettings.lccSIdTranslationsPath = lccTCTools.lccFGetConfiguration("lccIdTranslationsPath");
                lccTCTools.lccSCSettings.lccSDebugIP = lccTCTools.lccFGetConfiguration("lccDebugIP");
                lccTCTools.lccFLoadTranslationPairs();
                lccTCTools.lccSCSettings.lccFSetLogLevels();
                if (lccTCTools.lccSCSettings.lccSDebugIP.Length > 0)
                {
                    if (lccTCTools.lccSCSettings.lccSDebugIP.Equals(lccTCTools.lccSCSettings.lccSViewersIP) == true)
                    {
                        lccTCTools.lccSCSettings.lccBDebugMode = true;
                        lccTCTools.lccFLogInfo("0", "0", 1, "[ReadAllSettings] Debug Mode Activated");
                    }
                }
                lccTCTools.lccFLogInfo("0", "2", 1, "[ReadAllSettings] STARTED");

                if (lccTCTools.lccSCSettings.lccALConfigurations.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    lccTCTools.lccFLogInfo("0", "0", 1, "[ReadAllSettings] LogLevels [" + lccTCTools.lccSCSettings.lccSLogLevels + "]");
                    ///*
                    
                    
                    foreach (_25Live_New.lccTools.lccKeyValuePairClass lccKVPCLoop in lccTCTools.lccSCSettings.lccALConfigurations)
                    {
                        lccTCTools.lccFLogInfo("0", "11", 1, "[ReadAllSettings] key  [" + lccKVPCLoop.lccSKey + "] value [" + lccKVPCLoop.lccSValue + "]");
                    }
                    //  * */
                    //return lccTCTools.lccSCSettings.appSettings;
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            lccTCTools.lccFLogInfo("0", "12", 1, "[ReadAllSettings] FINISHED");
            //return null;
        }

        public string ReadSetting(string key)
        {
            string result = "";
            try
            {
                lccTCTools.lccFLogInfo("0", "13", 1, "[ReadSetting] STARTED Key [" + key+"]");
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key] ?? "Not Found";
                //return result;
                //Console.WriteLine(result);
            }
            catch (ConfigurationErrorsException lccException)
            {
                lccTCTools.lccFLogInfo("0", "0", 1, "[ReadSetting] Error reading app settings, ERROR [" + lccException.Message+"]");
            }
            lccTCTools.lccFLogInfo("0", "14", 1, "[ReadSetting] FINISHED");
            return result;
        }

        public String getData(String yearQuarter)
        {
            string lccSReturn = "";
            try
            {
                lccTCTools.lccFLogInfo("0", "3", 1, "[getData] STARTED yearQuarter [" + yearQuarter + "]");
                DataTable dt = new DataTable();
                String cs = System.Configuration.ConfigurationManager.ConnectionStrings["ClassDal"].ConnectionString;
                SqlConnection con = new SqlConnection(cs);
                SqlCommand com = new SqlCommand(lccTCTools.lccFGetConfiguration("25LiveUSP"), con);

                com.CommandType = CommandType.StoredProcedure;
                //Add your parameters
                String yearQ = yearQuarter; //"B561";
                com.Parameters.AddWithValue("@DataInYearQuarter", yearQ);

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
                lccTCTools.lccFLogInfo("0", "4", 1, "[getData] Copying dt.Rows");
                foreach (DataRow dr in dt.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                lccTCTools.lccFLogInfo("0", "5", 1, "[getData] Returning Serialize");
                return serializer.Serialize(rows);
                //return lccSReturn;
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
                //Console.WriteLine("Exception", ex);
                lccTCTools.lccFLogInfo("0", "0", 1, "[getData] ERROR [" + ex.Message+"]");
                return "[getData] ERROR: [" + ex.Message + "]";
            }
            lccTCTools.lccFLogInfo("0", "0", 1, "[getData] Error - FINISHED outside of try/catch");
            return "";
            //return null;
            

        }
        // The method takes start index, length and value of a field and concatnate it with line variable
        public string concatNewField(string line, int colNewField, int lenNewField, string valueNewField )
        {
            var spacesToAdd = 0; // Initialising it for reuse.
            var diffSpacesBetFields = 0; //subtract colNextField and length of the line and add the resultant number of spaces and concat with the next field value
            try
            {
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
            }
            catch (Exception lccException)
                {
                    lccTCTools.lccFLogInfo("0", "0", 1, "[concatNewField] ERROR [" + lccException .Message+ "]");
                }
            return line;

        }
        public String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        public IDictionary<string, string> createDataInFile(String yearQuarter)
        {
            int lccIObjectsCount = 0;
            IDictionary<string, string> dict = new Dictionary<string, string>(); // Will be used for return values
            String message = "";
            var filename = "";
            int colAP = 0;
            int colASM = 0;
            int colBeginYear = 0;
            int colBeginMonth = 0;
            int colBeginDay = 0;
            int colCatalog = 0;
            int colCourse = 0;
            int colCRN = 0;
            int colDays = 0;
            int colDepartmentID = 0;
            int colEndYear = 0;
            int colEndMonth = 0;
            int colEndDay = 0;
            int colEnrollment = 0;
            int colEvent = 0; // This can be ignored and leave space)s
            int colFinishHours = 0;
            int colFinishMinutes = 0;
            int colInstructor = 0;
            int colMeeting = 0;
            int colRoomName = 0;
            int colSection = 0;
            int colStartHours = 0;
            int colStartMinutes = 0;
            int colTerm = 0;

            int lenASM = 0;
            int lenCatalog = 0;
            int lenCourse = 0;
            int lenCRN = 0;
            int lenDays = 0;
            int lenDepartmentID = 0;
            int lenEnrollment = 0;
            int lenEvent = 0;
            int lenInstructor = 0;
            int lenMeeting = 0;
            int lenRoomName = 0;
            int lenSection = 0;
            int lengthTerm = 0;

            string path = "";
            //string line = "";
            string line = "";
            StringBuilder lccALLines = new StringBuilder();
            try
            {
                ReadAllSettings();
                lccTCTools.lccFLogInfo("0", "1", 1, "[createDataInFile] STARTED yearQuarter [" + yearQuarter + "]");

                colAP = lccTCTools.lccFGetConfigurationInt("ColAP");
                colASM = lccTCTools.lccFGetConfigurationInt("ColAssignment");
                colBeginYear = lccTCTools.lccFGetConfigurationInt("ColBeginYear");
                colBeginMonth = lccTCTools.lccFGetConfigurationInt("ColBeginMonth");
                colBeginDay = lccTCTools.lccFGetConfigurationInt("ColBeginDay");
                colCatalog = lccTCTools.lccFGetConfigurationInt("ColCatalog");
                colCourse = lccTCTools.lccFGetConfigurationInt("ColCourse");
                colCRN = lccTCTools.lccFGetConfigurationInt("ColCRN");
                colDays = lccTCTools.lccFGetConfigurationInt("ColDays");
                colDepartmentID = lccTCTools.lccFGetConfigurationInt("ColDepartmentId");
                colEndYear = lccTCTools.lccFGetConfigurationInt("ColEndYear");
                colEndMonth = lccTCTools.lccFGetConfigurationInt("ColEndMonth");
                colEndDay = lccTCTools.lccFGetConfigurationInt("ColEndDay");
                colEnrollment = lccTCTools.lccFGetConfigurationInt("ColEnrollment");
                colEvent = lccTCTools.lccFGetConfigurationInt("ColEvent"); // This can be ignored and leave space)s
                colFinishHours = lccTCTools.lccFGetConfigurationInt("ColFinishHours");
                colFinishMinutes = lccTCTools.lccFGetConfigurationInt("ColFinishMinutes");
                colInstructor = lccTCTools.lccFGetConfigurationInt("ColInstructor");
                colMeeting = lccTCTools.lccFGetConfigurationInt("ColMeeting");
                colRoomName = lccTCTools.lccFGetConfigurationInt("ColRoomName");
                colSection = lccTCTools.lccFGetConfigurationInt("ColSection");
                colStartHours = lccTCTools.lccFGetConfigurationInt("ColStartHours");
                colStartMinutes = lccTCTools.lccFGetConfigurationInt("ColStartMinutes");
                colTerm = lccTCTools.lccFGetConfigurationInt("ColTerm");

                lenASM = lccTCTools.lccFGetConfigurationInt("LengthAssignment");
                lenCatalog = lccTCTools.lccFGetConfigurationInt("LengthCatalog");
                lenCourse = lccTCTools.lccFGetConfigurationInt("LengthCourse");
                lenCRN = lccTCTools.lccFGetConfigurationInt("LengthCRN");
                lenDays = lccTCTools.lccFGetConfigurationInt("LengthDays");
                lenDepartmentID = lccTCTools.lccFGetConfigurationInt("LengthDepartmentId");
                lenEnrollment = lccTCTools.lccFGetConfigurationInt("LengthEnrollment");
                lenEvent = lccTCTools.lccFGetConfigurationInt("LengthEvent");
                lenInstructor = lccTCTools.lccFGetConfigurationInt("LengthInstructor");
                lenMeeting = lccTCTools.lccFGetConfigurationInt("LengthMeeting");
                lenRoomName = lccTCTools.lccFGetConfigurationInt("LengthRoomName");
                lenSection = lccTCTools.lccFGetConfigurationInt("LengthSection");
                lengthTerm = lccTCTools.lccFGetConfigurationInt("LengthTerm");

                //Call the stored procedure to get data
                //loop through the records
                if (lccTCTools.lccSCSettings.lccCheckLogLevel("17") == true)
                {
                    lccTCTools.lccFLogInfo("0", "17", 1, "[createDataInFile] Prevent USP from running (getData), test without SQL process");
                }
                else
                {
                    lccTCTools.lccFLogInfo("0", "16", 1, "[createDataInFile] Launching getData(" + yearQuarter + ")");
                    String classRows = getData(yearQuarter);



                    if (!string.IsNullOrEmpty(classRows))
                    {
                        lccTCTools.lccFLogInfo("0", "6", 1, "[createDataInFile] classRows Is Not NULL or Empty");
                        var objects = JArray.Parse(classRows); // parse as array 
                        var codeRanFlag = 0;
                        lccTCTools.lccFLogInfo("0", "7", 1, "[createDataInFile] classRows Count: " + objects.Count.ToString());

                        foreach (JObject root in objects)
                        {
                            lccIObjectsCount++;
                            var apDesignator = root.GetValue("APDesignator").ToString().Trim();
                            var assignment = root.GetValue("AssignmentField").ToString().Trim();
                            var catalog = root.GetValue("Catalog").ToString().Trim(); // Course Number
                            var classID = root.GetValue("ClassID").ToString().Trim();
                            var courseName = root.GetValue("CourseName").ToString().Trim();
                            var crn = root.GetValue("CRN").ToString().Trim();
                            var days = root.GetValue("Days").ToString().Trim();
                            var departmentID = root.GetValue("DepartmentID").ToString().Trim();
                            var enrollment = root.GetValue("Enrollment").ToString().Trim();
                            var finishHours = root.GetValue("FinishHours").ToString().Trim();
                            var finishMinutes = root.GetValue("FinishMinutes").ToString().Trim();
                            var finishWeek = root.GetValue("FinishWeek").ToString().Trim();
                            var meetingNumber = root.GetValue("MeetingNumber").ToString().Trim();
                            var roomName = root.GetValue("RoomName").ToString().Trim();
                            var section = root.GetValue("Section").ToString().Trim();
                            var startHours = root.GetValue("StartHours").ToString().Trim();
                            var startMinutes = root.GetValue("StartMinutes").ToString().Trim();
                            var startWeek = root.GetValue("StartWeek").ToString().Trim();
                            var term = root.GetValue("term").ToString().Trim(); // YearQuarterID
                            var instructorEmail = root.GetValue("WorkEmail").ToString().Trim();

                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [APDesignator] Value ["+root.GetValue("APDesignator").ToString().Length.ToString()+"][" + root.GetValue("APDesignator").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [AssignmentField] Value ["+root.GetValue("AssignmentField").ToString().Length.ToString()+"][" + root.GetValue("AssignmentField").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [Catalog] Value ["+root.GetValue("Catalog").ToString().Length.ToString()+"][" + root.GetValue("Catalog").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [ClassID] Value ["+root.GetValue("ClassID").ToString().Length.ToString()+"][" + root.GetValue("ClassID").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [CourseName] Value ["+root.GetValue("CourseName").ToString().Length.ToString()+"][" + root.GetValue("CourseName").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [CRN] Value ["+root.GetValue("CRN").ToString().Length.ToString()+"][" + root.GetValue("CRN").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [Days] Value ["+root.GetValue("Days").ToString().Length.ToString()+"][" + root.GetValue("Days").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [DepartmentID] Value ["+root.GetValue("DepartmentID").ToString().Length.ToString()+"][" + root.GetValue("DepartmentID").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [Enrollment] Value ["+root.GetValue("Enrollment").ToString().Length.ToString()+"][" + root.GetValue("Enrollment").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [FinishHours] Value ["+root.GetValue("FinishHours").ToString().Length.ToString()+"][" + root.GetValue("FinishHours").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [FinishMinutes] Value ["+root.GetValue("FinishMinutes").ToString().Length.ToString()+"][" + root.GetValue("FinishMinutes").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [FinishWeek] Value ["+root.GetValue("FinishWeek").ToString().Length.ToString()+"][" + root.GetValue("FinishWeek").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [MeetingNumber] Value ["+root.GetValue("MeetingNumber").ToString().Length.ToString()+"][" + root.GetValue("MeetingNumber").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [RoomName] Value ["+root.GetValue("RoomName").ToString().Length.ToString()+"][" + root.GetValue("RoomName").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [Section] Value ["+root.GetValue("Section").ToString().Length.ToString()+"][" + root.GetValue("Section").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [StartHours] Value ["+root.GetValue("StartHours").ToString().Length.ToString()+"][" + root.GetValue("StartHours").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [StartMinutes] Value ["+root.GetValue("StartMinutes").ToString().Length.ToString()+"][" + root.GetValue("StartMinutes").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [StartWeek] Value ["+root.GetValue("StartWeek").ToString().Length.ToString()+"][" + root.GetValue("StartWeek").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [term] Value ["+root.GetValue("term").ToString().Length.ToString()+"][" + root.GetValue("term").ToString() + "]");
                            lccTCTools.lccFLogInfo("0", "22", 1, "[createDataInFile] JSON Key [WorkEmail] Value [" + root.GetValue("WorkEmail").ToString().Length+ "][" + root.GetValue("WorkEmail").ToString() + "]");

                            //generate line
                            var spacesToAdd = 0; // Initialising it for reuse.
                            //Adding the spaces to make the field with length as specifed in lenField
                            spacesToAdd = lenDepartmentID - departmentID.Length;
                            if (spacesToAdd > 0)
                            {
                                departmentID = departmentID.PadRight(departmentID.Length + spacesToAdd, ' ');
                            }

                            line = departmentID;
                            line = concatNewField(line, colCatalog, lenCatalog, catalog);
                            line = lccTCTools.lccFTranslateId(line);
                            line = concatNewField(line, colSection, lenSection, section);
                            line = concatNewField(line, colDays, lenDays, days);
                            line = concatNewField(line, colStartHours, startHours.Length, startHours);
                            line = concatNewField(line, colStartMinutes, startMinutes.Length, startMinutes);
                            line = concatNewField(line, colFinishHours, finishHours.Length, finishHours);
                            line = concatNewField(line, colFinishMinutes, finishMinutes.Length, finishMinutes);
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
                            if (lccIObjectsCount < objects.Count)
                            {
                                line += System.Environment.NewLine;
                            }
                            lccALLines.Append(line);


                            //Write to a file
                            term = term.ToString().Trim();

                            if (codeRanFlag == 0)
                            {
                                filename = "datain" + term + ".dat";
                                path = lccTCTools.lccFGetConfiguration("FilePath") + filename;
                                // Check if the file exists.
                                //If it exists that archive that file.
                                var archivePath = lccTCTools.lccFGetConfiguration("ArchivePath");
                                if (System.IO.File.Exists(path))
                                {
                                    String timeStamp = GetTimestamp(DateTime.Now);
                                    var newFileName = term + "_" + timeStamp + ".dat";
                                    var newPath = archivePath + newFileName;
                                    // Move the existing file to archive folder
                                    System.IO.File.Move(path, newPath);
                                    lccTCTools.lccFLogInfo("0", "8", 1, "[createDataInFile] Archiving File [" + path + "] to [" + newPath + "]");
                                    message += "File " + newFileName + " has been moved to " + archivePath + ".";

                                }
                                codeRanFlag = 1;
                            }
                        }
                        if (line.Length > 0)
                        {
                            lccTCTools.lccFLogInfo("0", "9", 1, "[createDataInFile] Writing to file [" + path+"]");
                            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                            message += " A new file named " + filename + " is created at location " + lccTCTools.lccFGetConfiguration("FilePath") + ".";
                            file.WriteLine(lccALLines.ToString());
                            file.Close();
                        }

                    }
                }
                lccTCTools.lccFLogInfo("0", "10", 1, "[createDataInFile] FINISHED");
                message += "<hr>Process Finished.";
                dict["message"] = message + lccTCTools.lccFReturnLogOutput();
                dict["status"] = "success";
            }
            catch (Exception ex)
            {
                message += "<hr>Process Finished.";
                dict["message"] = "[createDataInFile] ERROR [" + ex.Message.ToString() + "]" + message + lccTCTools.lccFReturnLogOutput();
                dict["message"] = "[createDataInFile] ERROR - Inner Exception [" + ex.InnerException.ToString() + "]" + message + lccTCTools.lccFReturnLogOutput();
                dict["status"] = "failure";

            }
            lccTCTools.lccFLogInfo("0", "", 4, "");
            return dict;
        }
    }
}