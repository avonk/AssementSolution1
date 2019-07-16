using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSolution.DataModel
{
    public class Record
    {
        public string FilePath { get; set; }
        public int  FileSize { get; set; }
        public DateTime CreatedTime { get; set; }
        public string FileType { get; set; }
    }
}
