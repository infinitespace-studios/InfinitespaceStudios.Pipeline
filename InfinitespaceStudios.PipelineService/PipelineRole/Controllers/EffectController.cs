using MonoGame.RemoteEffect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RemoteEffectRole.Controllers
{
    public class EffectController : ApiController
    {
        // POST api/values
        public Result Post([FromBody]Data value)
        {
            try {
                string error = String.Empty;
                string compiledWith = String.Empty;
                byte[] buf = RunMGCB(value.Code, value.Platform, value.Version, out error, out compiledWith);
                return new Result() { Compiled = buf, Error = error, CompiledWith = compiledWith };
            } catch (Exception ex)
            {
                return new Result() { Error = ex.ToString() };
            }
        }

        static string GetVersion(string version)
        {
            var v =version.Split(new char[] { '.' });
            return v.Length ==4 ? $"{v[0]}.{v[1]}" : version;
        }

        static byte[] RunMGCB(string code, string platform, string version, out string error, out string compiledWith)
        {
            string[] platforms = new string[]
            {
                "DesktopGL",
                "Android",
                "iOS",
                "tvOS",
                "OUYA",
            };

            compiledWith = GetVersion(version);
            var mgfxExe = Path.Combine(HttpContext.Current.Server.MapPath(@"~\"),"Tools", compiledWith, "2MGFX.exe");
            if (!File.Exists (mgfxExe))
            {
                compiledWith = "3.5";
                mgfxExe = Path.Combine(HttpContext.Current.Server.MapPath(@"~\"), "Tools", "2MGFX.exe");
            }
            error = String.Empty;
            var tempPath = Path.GetFileName(Path.ChangeExtension(Path.GetTempFileName(), ".fx"));
            var xnb = Path.ChangeExtension(tempPath, ".mgfx");
            var tempOutput = Path.GetTempPath();
            File.WriteAllText(Path.Combine(tempOutput, tempPath), code);
            var startInfo = new ProcessStartInfo();
            var progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            startInfo.FileName = mgfxExe;
            startInfo.WorkingDirectory = tempOutput;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            var profile = platforms.Contains(platform) ? "OpenGL" : "DirectX_11";
            startInfo.Arguments = string.Format("./{0} ./{1} /Profile:{2}", tempPath, xnb, profile);
            var process = Process.Start(startInfo);
            try
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    error = process.StandardError.ReadToEnd();
                }
                if (File.Exists(Path.Combine(tempOutput, xnb)))
                {
                    return File.ReadAllBytes(Path.Combine(tempOutput, xnb));
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                File.Delete(Path.Combine(tempOutput, tempPath));
                File.Delete(Path.Combine(tempOutput, xnb));
            }
            return new byte[0];
        }
    }
}
namespace MonoGame.RemoteEffect
{
    public class Result
    {
        public byte[] Compiled { get; set;  }

        public string Error { get; set;  }

        public string CompiledWith { get; set; }
    }
    public class Data
    {
        public string Platform { get; set; }
        public string Code { get; set; }
        public string Version { get; set; }
    }
}
