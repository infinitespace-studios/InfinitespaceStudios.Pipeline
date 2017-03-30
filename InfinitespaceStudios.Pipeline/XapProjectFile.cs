using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MonoGame.RemoteEffect
{
    public class XapProjectFile
    {
        public string FileName { get; private set; }

        public XapProjectFile(string filename, ContentImporterContext context)
        {
            FileName = Path.GetFullPath(filename);
            context.Logger.LogImportantMessage (Path.GetDirectoryName(filename));
            
            var lines = File.ReadAllLines(FileName);
        }
    }
}
