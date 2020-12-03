using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingApp.LoggingFiles
{
    public class LoggingOptions
    {
        public virtual string FilePath { get; set; } // Gets the path of a specific file, will set the file if not already there.
        public virtual string FolderPath { get; set; } // Gets and sets the folder path of logs.

    }
}
