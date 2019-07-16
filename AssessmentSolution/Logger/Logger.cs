using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSolution.Logger
{
    public static class Logger
    {
        static Logger()
        {
           
        }

        private static string LoggerPath()
        {
            return Path.Combine(Path.GetTempPath(), "AssessmentSolutionLogs");
        }

        private static string CreateErrorMessage(Exception ex)
        {
        StringBuilder messageBuilder = new StringBuilder();
            try
            {
                messageBuilder.Append("The Exception is:-");

                messageBuilder.Append("Exception :: " + ex.ToString());
                if (ex.InnerException != null)
                {
                    messageBuilder.Append("InnerException :: " + ex.InnerException.ToString());
                }
                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.Append("Exception:: Unknown Exception.");
                return messageBuilder.ToString();
            }

        }
        private static void LogFileWrite(string message)
        {
            string logFilePath = LoggerPath();
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                using (fileStream = new FileStream(logFilePath, FileMode.Append))
                {
                    using (streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(message);
                        Debug.WriteLine(message);
                    }
                }
            }
            finally
            {
                if (streamWriter != null) streamWriter.Close();
                if (fileStream != null) fileStream.Close();
            }
        }

        //private static void LogFileWrite(string message)
        //{
        //    string logFilePath = LoggerPath();
        //    FileStream fileStream = null;
        //    FileInfo logFileInfo = new FileInfo(logFilePath);

        //    StreamWriter streamWriter = null;
        //    try
        //    {
        //        if (!logFileInfo.Exists)
        //        {
        //            fileStream = logFileInfo.Create();
        //        }
        //        else
        //        {
        //            fileStream = new FileStream(logFilePath, FileMode.Append);
        //        }
        //        streamWriter = new StreamWriter(fileStream);
        //        streamWriter.WriteLine(message);
        //    }
        //    finally
        //    {
        //        if (streamWriter != null) streamWriter.Close();
        //        if (fileStream != null) fileStream.Close();
        //    }
        //}

        public static void LogError(string message, Exception ex)
        {
            string errorMessage = CreateErrorMessage(ex);
            LogFileWrite(errorMessage);
        }

        public static void LogInfo(string message)
        {
            LogFileWrite(message);
        }

        public static void LogWarning(string message)
        {
            LogFileWrite(message);
        }
    }
}
