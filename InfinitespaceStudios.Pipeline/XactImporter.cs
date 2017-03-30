using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.RemoteEffect
{
    [ContentImporter(".xap", DisplayName = "XactImporter - Infinitespace Studios",DefaultProcessor = "XactProcessor")]
    public class XactImporter : ContentImporter<XapProjectFile>
    {
        //read and parse the .xap file.
        public override XapProjectFile Import(string filename, ContentImporterContext context)
        {
            return new XapProjectFile(filename, context);
        }
    }
}
