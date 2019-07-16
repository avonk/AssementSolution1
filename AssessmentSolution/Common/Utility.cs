using AssessmentSolution.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSolution.Utility
{
    public static class Utility
    {
        ///// <summary>
        ///// Shows Total number of count of selected type, aggrigate sum of all file of selected type
        ///// and average size of a selected type file
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="fileData"></param>
        //internal static void ShowFileInfo(string type, FilesData fileData)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append($"Total Number of {type} files are - {fileData.GetCount(type)}" + Environment.NewLine);
        //    sb.Append($"Aggregate size of {type} file is - {fileData.GetSize(type)}" + Environment.NewLine);
        //    sb.Append($"Average size of {type} files is - {fileData.GetAverageSize(type)}" + Environment.NewLine);
        //    WriteLine(sb);
        //}

        /// <summary>
        /// return file extention from string fileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static string GetFileType(string fileName)
        {
            return Path.GetExtension(fileName);
        }

        /// <summary>
        /// return int value converted from string using int.TryParse
        /// </summary>
        /// <param name="intString"></param>
        /// <returns></returns>
        internal static int? ConvertStringToInt(string intString)
        {
            int i = 0;
            return (int.TryParse(intString, out i) ? i : (int?)null);
        }

        /// <summary>
        /// check fileExtention is .dll/.xml/.nf/.config (ignore case)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static bool IsFileRequired(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) &&
                (fileName.IndexOf(FileType.CONFIG, StringComparison.OrdinalIgnoreCase) > 0 ||
                fileName.IndexOf(FileType.DLL, StringComparison.OrdinalIgnoreCase) > 0 ||
                fileName.IndexOf(FileType.NF, StringComparison.OrdinalIgnoreCase) > 0 ||
                fileName.IndexOf(FileType.XML, StringComparison.OrdinalIgnoreCase) > 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Handles Aggregate exceptions
        /// </summary>
        /// <param name="ae"></param>
        //internal static void HandleAggregateExceptions(AggregateException ae)
        //{
        //    ae.Handle((e) => {

        //        var ignoredExceptions = new List<Exception>();
        //        foreach (var ex in ae.Flatten().InnerExceptions)
        //        {
        //            if (ex is ArgumentException || ex is NullReferenceException || ex is DivideByZeroException)
        //            {
        //                Console.WriteLine("Encountered error while processing request.");
        //            }
        //            else
        //            {
        //                ignoredExceptions.Add(ex);
        //            }
        //            Debug.WriteLine(ex.Message);
        //        }
        //        if (ignoredExceptions.Count > 0) throw new AggregateException(ignoredExceptions);
        //        return true;
        //    });
        //}


    }
}

