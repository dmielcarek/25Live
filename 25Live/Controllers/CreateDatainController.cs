using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _25Live.Models;
using Newtonsoft.Json.Linq;
using System.IO;

namespace _25Live.Controllers
{
    public class CreateDatainController : Controller
    {
        // GET: CreateDatain
        public ActionResult Index()
        {
            try
            {
                ctrl obj = new ctrl();
                var allConfigurations = obj.ReadAllSettings();
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
                String classRows = obj.getData();
                String message = "";
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
                        var instructorName = root.GetValue("InstructorName").ToString().Trim();
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
                        line = obj.concatNewField(line, colCatalog, lenCatalog, catalog);
                        line = obj.concatNewField(line, colSection, lenSection, section);
                        line = obj.concatNewField(line, colDays, lenDays, days);
                        line = obj.concatNewField(line, colStartHours, startHours.Length, startHours);
                        line = obj.concatNewField(line, colStartMinutes, startMinutes.Length, startMinutes);
                        line = obj.concatNewField(line, colFinishtHours, finishHours.Length, finishHours);
                        line = obj.concatNewField(line, colFinishtMinutes, finishMinutes.Length, finishMinutes);
                        line = obj.concatNewField(line, colAP, apDesignator.Length, apDesignator);
                        line = obj.concatNewField(line, colEnrollment, lenEnrollment, enrollment);
                        line = obj.concatNewField(line, colRoomName, lenRoomName, roomName);
                        line = obj.concatNewField(line, colBeginYear, startWeek.Length, startWeek);
                        line = obj.concatNewField(line, colEndYear, finishWeek.Length, finishWeek);
                        line = obj.concatNewField(line, colMeeting, lenMeeting, meetingNumber);
                        line = obj.concatNewField(line, colASM, lenASM, assignment);
                        var Event = ""; // This field is optional and we will just fill this with spaces.
                        line = obj.concatNewField(line, colEvent, lenEvent, Event);
                        line = obj.concatNewField(line, colInstructor, lenInstructor, instructorName);
                        line = obj.concatNewField(line, colCourse, lenCourse, courseName);
                        line = obj.concatNewField(line, colTerm, lengthTerm, term);
                        line = obj.concatNewField(line, colCRN, lenCRN, crn);                      
                        line += "\n";//System.Environment.NewLine;



                        //Write to a file
                        term = term.ToString().Trim();
                        var filename = term + ".dat";
                        string path = allConfigurations["FilePath"] + filename;
                       
                        if (codeRanFlag == 0)
                        {
                            // Check if the file exists.
                            //If it exists that archive that file and create a new file.
                            var archivePath = allConfigurations["ArchivePath"];
                            if(System.IO.File.Exists(path))
                            {
                                String timeStamp = obj.GetTimestamp(DateTime.Now);
                                var newFileName = term + "_"+ timeStamp + ".dat";
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
                            // Here ideally you should just be appending data
                            
                            if (!System.IO.File.Exists(path))
                            {
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                                //TextWriter tw = new StreamWriter(path);
                                file.WriteLine(line);
                                file.Close();
                            }
                            else if (System.IO.File.Exists(path))
                            {
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                                //TextWriter tw = new StreamWriter(path, true);
                                file.WriteLine(line);
                                file.Close();
                            }
                        }
                        

                    }
                }
                Session["message"] = message;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception", ex);
                Session["exception"] = ex.Message.ToString();
                return View("ExceptionOccured");
            }
            

            return View("Datain");
        }
    }
}