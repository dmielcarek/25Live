﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace _25Live_New.lccTools
{
    public class lccToolsClass
    {
        public lccSettingsClass lccSCSettings = new lccSettingsClass();
        public string lccFTranslateId(string lccParamSId)
        {
            bool lccBFound = false;
            int lccIOrigLength = lccParamSId.Length;
            int lccILoop = 0;
            string lccSReturn = lccParamSId;
            try
            {
                lccFLogInfo("0", "18", 1, "[lccFTranslateId] Started - Id [" + lccParamSId+"]");
                for (lccILoop = 0; lccILoop < lccSCSettings.lccALTranslationIdPairs.Count && lccBFound == false; lccILoop++)
                {
                    lccFLogInfo("0", "19", 1, "[lccFTranslateId] Compare Id [" + lccParamSId + "] to Key [" + lccSCSettings.lccALTranslationIdPairs[lccILoop].lccSKey + "]");
                    if (lccSCSettings.lccALTranslationIdPairs[lccILoop].lccSKey.Equals(lccParamSId) == true)
                    {
                        lccFLogInfo("0", "20", 1, "[lccFTranslateId] Match");
                        lccBFound = true;
                        lccSReturn = lccSCSettings.lccALTranslationIdPairs[lccILoop].lccSValue;
                        if (lccSReturn.Length < lccIOrigLength)
                        {
                            lccFLogInfo("0", "20", 1, "[lccFTranslateId] Original Length [" + lccIOrigLength.ToString() + "] longer, Right Padding value [" + lccSReturn+"]");
                            lccSReturn = lccSReturn.PadRight(lccIOrigLength, ' ');
                            lccFLogInfo("0", "20", 1, "[lccFTranslateId] Right Padded value [" + lccSReturn + "]");
                        }
                    }
                }
                lccFLogInfo("0", "21", 1, "[lccFTranslateId] Started - Finished");
            }
            catch (Exception lccException)
            {
                lccFLogInfo("0", "0", 1, "[lccFTranslateId] ERROR: "+lccException.Message);
            }
            return lccSReturn;
        }
        public bool lccFLoadTranslationPairs()
        {
            bool lccBReturn = false;
            string lccSSource = "";
            string[] lccSSourceSplit = null;
            lccKeyValuePairClass lccKVPPair = new lccKeyValuePairClass();
            FileStream lccFSSource = null;
            StreamReader lccSRSource = null;
            try
            {
                lccFLogInfo("0", "23", 1, "[lccFLoadTranslationPairs] Started");
                if (File.Exists(lccSCSettings.lccSIdTranslationsPath) == false)
                {
                    lccFLogInfo("0", "23", 1, "[lccFLoadTranslationPairs] File does not exist [" + lccSCSettings.lccSIdTranslationsPath+"]");
                }
                else
                {
                    lccFLogInfo("0", "23", 1, "[lccFLoadTranslationPairs] Loading records");
                    lccFSSource = new FileStream(lccSCSettings.lccSIdTranslationsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    lccSRSource = new StreamReader(lccFSSource);
                    while ((lccSSource = lccSRSource.ReadLine()) != null)
                    {
                        if (lccSSource.Length > 0)
                        {
                            lccSSourceSplit = lccSSource.Split('\t');
                            if (lccSSourceSplit.Length > 1)
                            {
                                lccKVPPair.lccFClearValues();
                                lccKVPPair.lccSKey = lccSSourceSplit[0];
                                lccKVPPair.lccSValue = lccSSourceSplit[1];
                                lccSCSettings.lccALTranslationIdPairs.Add(lccFReturnNewKVPCPair(lccKVPPair));
                            }
                        }
                    }
                    lccSRSource.Close();
                    lccFSSource.Close();
                    lccFLogInfo("0", "23", 1, "[lccFLoadTranslationPairs] Loaded Records: "+lccSCSettings.lccALTranslationIdPairs.Count.ToString());
                }
                lccFLogInfo("0", "24", 1, "[lccFLoadTranslationPairs] Started");
                lccBReturn = true;
            }
            catch (Exception lccException)
            {
                lccFLogInfo("0", "0", 1, "[lccFLoadTranslationPairs] ERROR: " + lccException.Message);
            }
            return lccBReturn;
        }
        public string lccFReturnLogOutput()
        {
            string lccSReturn = "";
            if (lccSCSettings.lccSBLogOutput != null)
            {
                if (lccSCSettings.lccSBLogOutput.Length > 0)
                {
                    if (lccSCSettings.lccBDebugMode == true)
                    {
                        lccSReturn = lccSCSettings.lccSBLogOutput.ToString();
                    }
                }
            }
            return lccSReturn;
        }
        public bool lccFLogInfo(string lccParamSFunctionId, string lccParamSLogLevel, int lccParamIFlag, String lccParamSLogRecord)
        {
            // flag
            // 0 - regular, show on console and write to log records
            // 1 - only write to log records
            // 2 - console only
            // 4 - write log records and clear
            bool returnVal = false;
            int lccILoop = 0;
            DateTime lccLogDateTime = DateTime.Now;
            StringBuilder lccSBTargetStr = new StringBuilder();
            String lccLogAppendYearMonthStr = "";
            FileStream lccLogFile = null;
            StreamWriter lccLogWriter = null;

            if (lccParamSLogRecord.Length > 0 || lccParamIFlag==4)
            {
                if (lccSCSettings.lccCheckLogLevel(lccParamSLogLevel) == true)
                {
                    if (lccSCSettings.lccSLogPath.Length > 0)
                    {
                        try
                        {
                            switch (lccParamIFlag)
                            {
                                case 0:
                                case 1:
                                    lccSBTargetStr.Append(lccLogDateTime.Year.ToString());
                                    break;
                                case 4:
                                    if (lccSCSettings.lccBLogAppendYearMonth == false)
                                    {
                                        lccLogFile = new FileStream(lccSCSettings.lccSLogPath + ".log", FileMode.Append, FileAccess.Write);
                                    }
                                    else
                                    {
                                        lccLogAppendYearMonthStr = DateTime.Now.Year.ToString();
                                        if (DateTime.Now.Month < 10)
                                        {
                                            lccLogAppendYearMonthStr += "0";
                                        }
                                        lccLogAppendYearMonthStr += DateTime.Now.Month.ToString();
                                        if (DateTime.Now.Day < 10)
                                        {
                                            lccLogAppendYearMonthStr += "0";
                                        }
                                        lccLogAppendYearMonthStr += DateTime.Now.Day.ToString();
                                        lccLogFile = new FileStream(lccSCSettings.lccSLogPath + "-" + lccLogAppendYearMonthStr + ".log", FileMode.Append, FileAccess.Write);
                                    }
                                    lccLogWriter = new StreamWriter(lccLogFile);
                                    break;
                            }

                            if (lccLogDateTime.Month < 10)
                            {
                                lccSBTargetStr.Append("0");
                            }
                            lccSBTargetStr.Append(lccLogDateTime.Month.ToString());
                            if (lccLogDateTime.Day < 10)
                            {
                                lccSBTargetStr.Append("0");
                            }
                            lccSBTargetStr.Append(lccLogDateTime.Day.ToString());
                            lccSBTargetStr.Append("\t");

                            if (lccLogDateTime.Hour < 10)
                            {
                                lccSBTargetStr.Append("0");
                            }
                            lccSBTargetStr.Append(lccLogDateTime.Hour.ToString());
                            lccSBTargetStr.Append(":");
                            if (lccLogDateTime.Minute < 10)
                            {
                                lccSBTargetStr.Append("0");
                            }
                            lccSBTargetStr.Append(lccLogDateTime.Minute.ToString());
                            lccSBTargetStr.Append(":");
                            if (lccLogDateTime.Second < 10)
                            {
                                lccSBTargetStr.Append("0");
                            }
                            lccSBTargetStr.Append(lccLogDateTime.Second.ToString());
                            lccSBTargetStr.Append(":");
                            if (lccLogDateTime.Millisecond < 10)
                            {
                                lccSBTargetStr.Append("0");
                            }
                            lccSBTargetStr.Append(lccLogDateTime.Millisecond.ToString());
                            lccSBTargetStr.Append("\t");
                            lccSBTargetStr.Append(lccSCSettings.lccSViewersIP);
                            lccSBTargetStr.Append("\t");
                            lccSBTargetStr.Append("[" + lccParamSFunctionId + "] ");
                            lccSBTargetStr.Append("\t");
                            lccSBTargetStr.Append("[" + lccParamSLogLevel + "] ");
                            lccSBTargetStr.Append(lccParamSLogRecord);

                            switch (lccParamIFlag)
                            {
                                case 0:
                                case 1:
                                    lccSCSettings.lccALLogRecords.Add(lccSBTargetStr.ToString());
                                    break;
                                case 4:
                                    for (lccILoop = 0; lccILoop < lccSCSettings.lccALLogRecords.Count; lccILoop++)
                                    {
                                        lccLogWriter.WriteLine(lccSCSettings.lccALLogRecords[lccILoop]);
                                    }
                                    lccLogWriter.Close();
                                    lccLogFile.Close();
                                    lccSCSettings.lccALLogRecords.Clear();
                                    break;
                            }
                            lccSCSettings.lccSBLogOutput.Append("\r\n<br>" + lccSBTargetStr);
                        }
                        catch (Exception lccException)
                        {
                            lccSCSettings.lccALLogRecords.Add("[lccFLogInfo] ERROR: " + lccException.Message);
                        }
                    }
                }
            }
            return returnVal;
        }
        public lccKeyValuePairClass lccFReturnNewKVPCPair(lccKeyValuePairClass lccParamKVPCPair)
        {
            lccKeyValuePairClass lccKVPCReturn = new lccKeyValuePairClass();
            lccKVPCReturn.lccSKey = lccParamKVPCPair.lccSKey;
            lccKVPCReturn.lccSValue = lccParamKVPCPair.lccSValue;
            return lccKVPCReturn;
        }
    }
    public class lccKeyValuePairClass
    {
        public string lccSKey { get; set; }
        public string lccSValue { get; set; }
        public lccKeyValuePairClass()
        {
            lccFClearValues();
        }
        public void lccFClearValues()
        {
            lccSKey = "";
            lccSValue = "";
        }
    }
    public class lccSettingsClass
    {
        public bool lccBLogAppendYearMonth { get; set; }
        public bool lccBDebugMode { get; set; }
        public string lccSLogPath { get; set; }
        public string lccSIdTranslationsPath { get; set; }
        public string lccSViewersIP { get; set; }
        public string lccSLogLevels { get; set; }
        public string lccSDebugIP { get; set; }
        public StringBuilder lccSBLogOutput = new StringBuilder();
        public List<string> lccALLogRecords = new List<string>();
        public List<lccKeyValuePairClass> lccALTranslationIdPairs = new List<lccKeyValuePairClass>();
        public List<string> lccALLogLevels = new List<string>();
        public System.Collections.Specialized.NameValueCollection allConfigurations = null;

        public lccSettingsClass()
        {
            lccFClearValues();
        }
        public bool lccCheckLogLevel(string lccSParam)
        {
            // Log Levels
            bool lccBReturnVal = false;
            int lccILoop = 0;
            try
            {
                if (lccSParam.Length == 0
                    || lccSParam.Equals("0") == true
                    )
                {
                    lccBReturnVal = true;
                }
                else
                {
                    for (lccILoop = 0; lccILoop < lccALLogLevels.Count && lccBReturnVal == false; lccILoop++)
                    {
                        if (lccALLogLevels[lccILoop].Equals(lccSParam) == true)
                        {
                            lccBReturnVal = true;
                        }
                    }
                }
            }
            catch (Exception lccException)
            {
                //lccFLogInfo("0", "", 1, "[lccCheckLogLevel] ERROR: " + lccException.Message);
            }
            return lccBReturnVal;
        }

        public bool lccFSetLogLevels()
        {
            // Log Levels
            bool lccBReturnVal = false;
            int lccILoop = 0;
            string[] lccSLogLevelsSplit = lccSLogLevels.Split(',');
            try
            {
                lccALLogLevels.Clear();
                for (lccILoop = 0; lccILoop < lccSLogLevelsSplit.Length; lccILoop++)
                {
                    lccALLogLevels.Add(lccSLogLevelsSplit[lccILoop]);
                }
                lccBReturnVal = true;
            }
            catch (Exception lccException)
            {
            }
            return lccBReturnVal;
        }

        public void lccFClearValues()
        {
            lccBDebugMode = false;
            lccBLogAppendYearMonth = true;
            lccSLogPath = "";
            lccSIdTranslationsPath = "";
            lccSViewersIP = "";
            lccSLogLevels = "";
            lccSDebugIP = "";
            lccSBLogOutput.Length = 0;
            lccSViewersIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            allConfigurations = null;
            lccALLogRecords.Clear();
            lccALLogLevels.Clear();
            lccALTranslationIdPairs.Clear();
        }
    }
}