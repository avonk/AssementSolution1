//using AssessmentSolution.DataModel;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace AssessmentSolution
//{
//    public class DataFile : IEnumerable
//    {

//        private List<Record> records = new List<Record>();

//        public DataFile()
//        {

//        }
//        /// <summary>
//        /// constructor taking filepath 
//        /// </summary>
//        /// <param name="filePath"></param>
//        public DataFile(string filePath)
//        {
//            ReadTextFile(filePath);
//        }
//        /// <summary>
//        /// Adds new record to Data
//        /// </summary>
//        /// <param name="record"></param>
//        public void Add(Record record)
//        {
//            records.Add(record);
//        }
//        /// <summary>
//        /// Returns number of records Data
//        /// </summary>
//        public int Count
//        {
//            get { return records.Count(); }
//        }
//        /// <summary>
//        /// returns Record as per input index
//        /// </summary>
//        /// <param name="index"></param>
//        /// <returns>Record</returns>
//        public Record this[int index]
//        {
//            get { return records[index]; }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return new DataEnumerator(this);
//        }
//        #region Data Methods
//        /// <summary>
//        /// This method finds total number of records of a file type(xml,dll,nf,config)
//        /// </summary>
//        /// <param name="fileType"></param>
//        /// <returns> int count of records</returns>
//        public int GetCount(string fileType = null)
//        {
//            int index = 0;
//            if (string.IsNullOrEmpty(fileType))
//                return this.Count;
//            else
//            {
//                try
//                {
//                    Parallel.For(0, this.Count, x =>
//                    {
//                        if (this[x].FileType == fileType) Interlocked.Increment(ref index);
//                    });
//                }
//                catch (AggregateException ae)
//                {
//                    Utility.HandleAggregateExceptions(ae);
//                }
//            }

//            return index;
//        }
//        /// <summary>
//        /// This method finds total sum of size for records of a passed file type(xml,dll,nf,config)
//        /// </summary>
//        /// <param name="fileType"></param>
//        /// <returns>int sum of all record's size property</returns>
//        public int GetSize(string fileType)
//        {
//            int totalSize = 0;
//            object locker = new object();
//            try
//            {
//                Parallel.For(0, this.Count, x =>
//                {
//                    if (this[x].FileType == fileType)
//                    {
//                        Interlocked.Add(ref totalSize, this[x].Size);
//                    }
//                });
//            }
//            catch (AggregateException ae)
//            {
//                Utility.HandleAggregateExceptions(ae);
//            }

//            return totalSize;
//        }
//        /// <summary>
//        /// This method finds average of size number of records of a file type(xml,dll,nf,config)
//        /// </summary>
//        /// <param name="fileType"></param>
//        /// <returns>double average of record's size property</returns>
//        public double GetAverageSize(string fileType)
//        {
//            double average = 0;
//            try
//            {
//                int totalSize = GetSize(fileType);
//                int totalFiles = GetCount(fileType);
//                if (totalFiles != 0)
//                    average = (double)totalSize / totalFiles;

//                return Math.Floor(average * 100) / 100;
//            }
//            catch (AggregateException ae)
//            {
                
//            }
//            return Math.Floor(average * 100) / 100;
//        }


//        /// <summary>
//        /// Reads all the text from a location(filePath)
//        /// if the file is xml, nf, config,dll it adds the record to Data 
//        /// </summary>
//        /// <param name="filePath"></param>
//        public void ReadTextFile(string filePath)
//        {
//            string[] readText;
//            object locker = new object();
//            try
//            {
//                Parallel.ForEach(readText = File.ReadAllLines(filePath), (x) =>
//                {
//                    if (Utility.IsFileInContext(x))
//                    {
//                        string[] record = x.Split(new[] { "^|^" }, StringSplitOptions.None);
//                        string type = Utility.GetFileTYpe(record[0]);
//                        lock (locker)
//                            this.Add(new Record(record[0].Trim('^'), int.Parse(record[1]), record[2].Trim('^'), type));
//                    }
//                });
//            }
//            catch (AggregateException ae)
//            {
//                // Utility.HandleAggregateExceptions(ae);
//            }
//        }
//        #endregion

//    }
//    public class DataEnumerator : IEnumerator
//    {
//        internal DataFile Data;
//        internal int CurrentIndex;
//        internal Record CurrentRecord;
//        public DataEnumerator(DataFile data)
//        {
//            Data = data;
//            CurrentIndex = -1;
//        }
//        public object Current
//        {
//            get
//            {
//                return CurrentRecord;
//            }
//        }

//        public bool MoveNext()
//        {
//            if (++CurrentIndex >= Data.Count)
//                return false;
//            else
//            {
//                CurrentRecord = Data[CurrentIndex];
//                return true;
//            }
//        }

//        public void Reset()
//        {
//            CurrentIndex = -1;
//        }
//    }

//}
