﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sledge.Editor.Compiling
{
    public class BatchCompileStep
    {
        public string Operation { get; set; }
        public bool SystemCommand { get; set; }
        public string Flags { get; set; }
        public string BeforeExecute { get; set; }
        public string AfterExecute { get; set; }
    }
}
