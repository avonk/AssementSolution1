using AssessmentSolution.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssessmentSolution
{
   public class ProcessFile
    {
        
        /// <summary>
        /// finds total number of records of a file type(xml,dll,nf,config)
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns> int count of records</returns>
        public int GetCount(FilesData filesData, string fileType = null)
        {
            int index = 0;
            if (string.IsNullOrEmpty(fileType))
                return filesData.Count;
            else
            {
                try
                {
                    Parallel.For(0, filesData.Count, x =>
                    {
                        if (filesData[x].FileType == fileType) Interlocked.Increment(ref index);
                    });
                }
                catch (AggregateException ae)
                {
                   // Utility.HandleAggregateExceptions(ae);
                }
            }

            return index;
        }

        /// <summary>
        /// This method finds total sum of size for records of a passed file type(xml,dll,nf,config)
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns>int sum of all record's size property</returns>
        public int GetSize(FilesData filesData, string fileType)
        {
            int totalSize = 0;
            object locker = new object();
            try
            {
                Parallel.For(0, filesData.Count, x =>
                {
                    if (filesData[x].FileType == fileType)
                    {
                        Interlocked.Add(ref totalSize, filesData[x].Size);
                    }
                });
            }
            catch (AggregateException ae)
            {
               // Utility.HandleAggregateExceptions(ae);
            }

            return totalSize;
        }

        /// <summary>
        /// This method finds average of size number of records of a file type(xml,dll,nf,config)
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns>double average of record's size property</returns>
        public double GetAverageSize(FilesData filesData, string fileType)
        {
            double average = 0;
            try
            {
                int totalSize = GetSize(fileType);
                int totalFiles = GetCount(fileType);
                if (totalFiles != 0)
                    average = (double)totalSize / totalFiles;

                return Math.Floor(average * 100) / 100;
            }
            catch (AggregateException ae)
            {
                //Utility.HandleAggregateExceptions(ae);
            }
            return Math.Floor(average * 100) / 100;
        }

    }
}
