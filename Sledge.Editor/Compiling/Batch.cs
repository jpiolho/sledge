using System.Collections.Generic;
using System.IO;
using Sledge.Settings.Models;

namespace Sledge.Editor.Compiling
{
    public class Batch
    {
        public List<BatchCompileStep> Steps { get; set; }
        public string BeforeExecute { get; set; }
        public string AfterExecute { get; set; }
        public string BeforeExecuteStep { get; set; }
        public string AfterExecuteStep { get; set; }
        public string TargetFile { get; set; }
        public string OriginalFile { get; set; }

        public Batch(Game game, Build build, string targetFile, string originalFile)
        {
            // TODO: this properly
            BeforeExecute = "";
            AfterExecute = "";
            BeforeExecuteStep = "";
            AfterExecuteStep = "";
            TargetFile = targetFile;
            var fileFlag = '"' + targetFile + '"';
            var bspFile = '"' + Path.ChangeExtension(targetFile, "bsp") + '"';
            var copyBsp = '"' + Path.ChangeExtension(originalFile, "bsp") + '"';
            Steps = new List<BatchCompileStep>
                        {
                            new BatchCompileStep { Operation = Path.Combine(build.Path, build.Csg), Flags = fileFlag },
                            new BatchCompileStep { Operation = Path.Combine(build.Path, build.Bsp), Flags = fileFlag },
                            new BatchCompileStep { Operation = Path.Combine(build.Path, build.Vis), Flags = fileFlag },
                            new BatchCompileStep { Operation = Path.Combine(build.Path, build.Rad), Flags = fileFlag },
                            new BatchCompileStep { Operation = "move", SystemCommand = true, Flags = bspFile + " " + copyBsp }
                            //new BatchCompileStep { Operation = "copy"}
                        };
        }
    }
}