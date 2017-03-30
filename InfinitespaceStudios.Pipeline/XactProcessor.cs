using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace MonoGame.RemoteEffect
{
    [ContentProcessor(DisplayName = "XactProcessor - Infinitespace Studios")]
    public class XactProcessor : ContentProcessor<XapProjectFile, object>
    {
        [DefaultValue("wine")]
        public string WineExe { get; set; }

        [DefaultValue("")]
        public string WineInstall { get; set; }

        public XactProcessor()
        {
            WineExe = "wine";
            WineInstall = String.Empty;
        }

        public override object Process(XapProjectFile input, ContentProcessorContext context)
        {
            
            var location = Path.GetFullPath(GetType().Assembly.Location);
            if (string.IsNullOrEmpty(WineInstall))
                WineInstall = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".wine");
            // execute the XActBld3.exe. 
            var isWindows = Environment.OSVersion.Platform != PlatformID.Unix;
            var programFiles = isWindows ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) :
                Path.Combine(WineInstall, "drive_c", "Program Files (x86)");
            var xnaPath = Path.Combine(programFiles, "Microsoft XNA", "XNA Game Studio", "v4.0");
            var XactBld3Tool = Path.Combine("Tools", "XactBld3.exe");
            var toolPath = Path.Combine(xnaPath, XactBld3Tool);
           
            if (!isWindows)
            {
                // extract wine and install XNA
                if (!File.Exists (toolPath))
                {
                    throw new InvalidProgramException($"Could not locate {XactBld3Tool}. You need to install Wine and XNA Tooling. See https://github.com/infinitespace-studios/InfinitespaceStudios.Pipeline/wiki for more details.");
                }
            }
            
            if (isWindows && !File.Exists (toolPath))
            {
                toolPath = Path.Combine(Environment.GetEnvironmentVariable("XNAGSv4"), XactBld3Tool);
                if (!File.Exists (toolPath))
                {
                    toolPath = Path.Combine(location, XactBld3Tool);
                    if (!File.Exists(toolPath))
                        throw new InvalidProgramException($"Could not locate {XactBld3Tool}");
                }
            }

            var outputPath = Path.GetDirectoryName(context.OutputFilename);
            Directory.CreateDirectory(outputPath);
            var inputPath = input.FileName;
            if (!isWindows)
            {
                outputPath = "Z:" + outputPath.Replace("/", "\\");
                if (outputPath.EndsWith("\\"))
                    outputPath = outputPath.Substring(0, outputPath.Length - 1);
                inputPath = "Z:" + inputPath.Replace("/", "\\");
            }

            var stdout_completed = new ManualResetEvent(false);
            var stderr_completed = new ManualResetEvent(false);
            var psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = isWindows ? toolPath : WineExe,
                Arguments = (!isWindows ? $"\"{toolPath}\" " : String.Empty) + $"/WINDOWS \"{inputPath}\" \"{outputPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
            };

            context.Logger.LogImportantMessage($"Running: {psi.FileName} {psi.Arguments}");
           
            using (var proc = new System.Diagnostics.Process())
            {             
                proc.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                    {
                        stdout_completed.Set();
                        return;
                    }
                    //context.Logger.LogImportantMessage($"{e.Data}");
                };
                proc.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null)
                    {
                        stderr_completed.Set();
                        return;
                    }
                    //context.Logger.LogImportantMessage($"{e.Data}");
                };
                proc.StartInfo = psi;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                if (psi.RedirectStandardError)
                    stderr_completed.WaitOne(TimeSpan.FromSeconds(30));
                if (psi.RedirectStandardOutput)
                    stdout_completed.WaitOne(TimeSpan.FromSeconds(30));
                if (proc.ExitCode != 0)
                {
                    throw new InvalidContentException();
                }
                foreach (var file in Directory.GetFiles(Path.GetDirectoryName(context.OutputFilename), "*.xgs", SearchOption.TopDirectoryOnly))
                {
                    context.AddOutputFile(file);
                }
                foreach (var file in Directory.GetFiles(Path.GetDirectoryName(context.OutputFilename), "*.xsb", SearchOption.TopDirectoryOnly))
                {
                    context.AddOutputFile(file);
                }
                foreach (var file in Directory.GetFiles(Path.GetDirectoryName(context.OutputFilename), "*.xwb", SearchOption.TopDirectoryOnly))
                {
                    context.AddOutputFile(file);
                }
            }
            return null;
        }
    }
}
