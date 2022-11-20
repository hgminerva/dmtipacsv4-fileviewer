using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmtipacs_viewer_openner
{
    class Config
    {
        public string JsonPath { get; set; }
        public string ViewerProgramPath { get; set; }
        public string DropboxDrive { get; set; }
        public int DropboxDirectoryLevel { get; set; }
    }
}
