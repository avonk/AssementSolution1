using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AssessmentSolution.Logger;

namespace AssessmentSolution
{
   public class Program
    {
       
        static void Main(string[] args)
        {
            string inputFilePath = FilePath();
            try
            {
                FileInfo inputFileInfo = new FileInfo(inputFilePath);
                if (inputFileInfo.Exists)
                {
                    FilesData filesData = new FilesData(FilePath());
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                    Console.ReadLine();
                    return;
                }
            }

            catch (FileNotFoundException fe)
            {
                Logger.Logger.LogError("File not found exception: ", fe);
                Console.WriteLine($"Cannot file find at given file filePath:" + inputFilePath);
                Debug.WriteLine(fe.FileName + " " + fe.InnerException);
                Console.ReadLine();
            }
            catch (PathTooLongException pe)
            {
                Logger.Logger.LogError("Path is too long. ", pe);
                Console.WriteLine("Path is too long.");
                Debug.WriteLine(pe.Message);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logger.Logger.LogError("File not found exception: ", ex);
                Console.WriteLine("An exception occured while reading file. ");
                Console.ReadLine();
            }        
        }

        private static string FilePath()
        {
            return string.Format(@"{0}\DataFiles\Assessment Input.txt", Environment.CurrentDirectory);
        }
    }
}
