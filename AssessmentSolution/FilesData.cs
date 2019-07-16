using AssessmentSolution;
using AssessmentSolution.Common;
using AssessmentSolution.DataModel;
using AssessmentSolution.Logger;
using AssessmentSolution.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


public class FilesData : IEnumerable
{
    internal List<Record> FilteredRecordList { get; set; }

    public FilesData()
    {

    }

    /// <summary>
    /// constructor taking filepath to open and process file date
    /// </summary>
    /// <param name="FilePath"></param>
    public FilesData(string filePath)
    {
        FilteredRecordList = new List<Record>();
        ReadFile(filePath);
        if (FilteredRecordList?.Count() > 0)
            PrintFilesDataInfo();
    }


    /// <summary>
    /// Adds new record to FilteredRecordList
    /// </summary>
    /// <param name="record"></param>
    public void Add(Record record)
    {
        FilteredRecordList.Add(record);
    }

    /// <summary>
    /// returns Record as per input index
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Record</returns>
    public Record this[int index]
    {
        get { return FilteredRecordList[index]; }
    }

    /// <summary>
    /// Reads all the text from location provided
    /// if the file is xml, nf, config,dll it adds the record to Data 
    /// </summary>
    public void ReadFile(string filePath)
    {
        try
        {
            Logger.LogInfo("LoadFile- Start");
            string inputFilePath = string.Format(@"{0}\DataFiles\Assessment Input.txt", Environment.CurrentDirectory);
            FileInfo inputFileInfo = new FileInfo(inputFilePath);
            if (inputFileInfo.Exists)
            {
                Parallel.ForEach(File.ReadAllLines(inputFilePath), line =>
                {
                    CreateRecordCollection(line);
                });
                // DisplayFileInfo();
            }
        }
        catch (FileNotFoundException fe)
        {
            Logger.LogError("File not found exception: ", fe);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error in LoadFile", ex);
        }
    }

    /// <summary>
    /// process each line and add record data to FIleteredRecordList
    /// </summary>
    /// <param name="line"></param>
    private void CreateRecordCollection(string line)
    {
        object locker = new object();
        if (Utility.IsFileRequired(line))
        {
            string[] result = Regex.Matches(line, @"\^(.*?)\^").Cast<Match>().Select(m => m.Value).ToArray();
            Record record = new Record();
            string type = Utility.GetFileType(result[0].Trim('^'));
            int? size = Utility.ConvertStringToInt(result[1].Trim('^'));
            if (size != null)
            {
                record.FileSize = size.Value;
                record.CreatedTime = Convert.ToDateTime(result[2].Trim('^'));
                record.FileType = type;
            }
            lock (locker)
                this.Add(record);
        }
    }

    /// <summary>
    /// method to call display filetype information
    /// </summary>
    /// <param name="filesData"></param>
    internal void PrintFilesDataInfo()
    {
        if (FilteredRecordList == null)
        {
            Console.WriteLine("File does not contain data");
            return;
        }
        /*Prompt user for selecting a file type
        1 is for xml, 2 is for dll, 3 is for nf and 4 is for config files*/

        //bool flag to maintain program lifecycle. When set to false the program terminates.
        bool isExitPrompted = false;
        Console.WriteLine("Please select" + Environment.NewLine +
            "1- for xml File" + Environment.NewLine +
            "2- for dll files" + Environment.NewLine +
            "3- for nf files" + Environment.NewLine +
            "4- for config files" + Environment.NewLine +
            "5- to exit" + Environment.NewLine);
        //continued iteration to get multiple user inputs
        var input = Console.ReadLine();
        while (!isExitPrompted)
        {
            //check if input is valid int type
            if (int.TryParse(input, out int i))
            {
                switch (i)
                {
                    case 1:
                        DisplayFileInfo(FileType.XML);
                        break;
                    case 2:
                        DisplayFileInfo(FileType.DLL);
                        break;
                    case 3:
                        DisplayFileInfo(FileType.NF);
                        break;
                    case 4:
                        DisplayFileInfo(FileType.CONFIG);
                        break;
                    case 5:
                        isExitPrompted = true;
                        break;
                    default:
                        Console.WriteLine("invalid input please select values between 1 and 5");
                        break;
                }
                if (!isExitPrompted)
                    input = Console.ReadLine();
            }
            else
            {
                //Prompt user to input a valid int number between 1 and 5
                Console.WriteLine("invalid input please select values between 1 and 5");
                input = Console.ReadLine();
            }

        }
    }

    private void DisplayFileInfo(string fileType)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Total Number of " + fileType + " files are -" + GetCount(fileType) + Environment.NewLine);
        sb.Append("Total size of " + fileType + "file is - " + GetSize(fileType) + Environment.NewLine);
        sb.Append("Average size of " + fileType + "files is -" + GetAverageSize(fileType) + Environment.NewLine);
        Console.WriteLine(sb);
    }

    /// <summary>
    /// This method finds total number of records of a file type(xml,dll,nf,config)
    /// </summary>
    /// <param name="fileType"></param>
    /// <returns> int count of records</returns>
    public int GetCount(string fileType = null)
    {
        int index = 0;
        if (string.IsNullOrEmpty(fileType))
            return this.FilteredRecordList.Count();
        else
        {
            try
            {
                Parallel.For(0, this.FilteredRecordList.Count(), x =>
                {
                    if (this[x].FileType == fileType) Interlocked.Increment(ref index);
                });
            }
            catch (AggregateException ae)
            {
                Logger.LogError("Error while calculating total size: ", ae);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error while calculating total size: ", ex);
            }
        }

        return index;
    }
    /// <summary>
    /// This method finds total sum of size for records of a passed file type(xml,dll,nf,config)
    /// </summary>
    /// <param name="fileType"></param>
    /// <returns>int sum of all record's size property</returns>
    public int GetSize(string fileType)
    {
        int totalSize = 0;
        object locker = new object();
        try
        {
            Parallel.For(0, FilteredRecordList.Count(),
                  index =>
                  {
                      if (this[index].FileType == fileType)
                          Interlocked.Add(ref totalSize, this[index].FileSize);
                  });
            return totalSize;
        }
        catch (AggregateException ae)
        {
            Logger.LogError("Error while calculating total size: ", ae);
            Console.WriteLine("An exception occured while calculating total size. ");
        }
        catch (Exception ex)
        {
            Logger.LogError("Error while calculating total size: ", ex);
        }
        return totalSize;
    }
    /// <summary>
    /// This method finds average of size number of records of a file type(xml,dll,nf,config)
    /// </summary>
    /// <param name="fileType"></param>
    /// <returns>double average of record's size property</returns>
    public double GetAverageSize(string fileType)
    {
        double average = 0;
        try
        {
            int totalSize = GetSize(fileType);
            int totalFiles = GetCount(fileType);
            if (totalFiles != 0)
                average = (double)totalSize / totalFiles;
            return average;
        }
        catch (AggregateException ae)
        {
            Logger.LogError("Error while calculating average: ", ae);
            Console.WriteLine("An exception occured while calculating average. ");
            Debug.WriteLine(ae.Message);
            Console.ReadLine();
        }
        return average;
    }

    public IEnumerator GetEnumerator()
    {
        return new DataEnumerator(this);
    }

}


public class DataEnumerator : IEnumerator
{
    internal FilesData FilesData;
    internal int CurrentIndex;
    internal Record CurrentRecord;

    public DataEnumerator()
    {
    }

    public DataEnumerator(FilesData filesData)
    {
        FilesData = filesData;
        CurrentIndex = -1;
    }
    public object Current
    {
        get
        {
            return CurrentRecord;
        }
    }

    public bool MoveNext()
    {
        if (++CurrentIndex >= FilesData.FilteredRecordList.Count)
            return false;
        else
        {
            CurrentRecord = FilesData[CurrentIndex];
            return true;
        }
    }

    public void Reset()
    {
        CurrentIndex = -1;
    }
}

