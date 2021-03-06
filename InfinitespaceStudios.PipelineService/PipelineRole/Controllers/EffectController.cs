﻿using MonoGame.RemoteEffect;
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
                Version compiledWith;
                byte[] buf = RunMGCB(value.Code, value.Platform, value.Version, out error, out compiledWith);
                return new Result() { Compiled = buf, Error = error, CompiledWith = compiledWith.ToString () };
            } catch (Exception ex)
            {
                return new Result() { Error = ex.ToString() };
            }
        }

        static Version GetVersion(string version)
        {
            Version result;
            if (!Version.TryParse (version, out result))
            {
                return new Version(3, 5);
            }
            return result;
        }

        static string Find2MGFXForVersion (Version version)
        {
            var root = Path.Combine(HttpContext.Current.Server.MapPath(@"~\"), "Tools");
            var dirs = Directory.EnumerateDirectories(root, "*", SearchOption.TopDirectoryOnly);
            foreach (var dir in dirs.OrderByDescending(x => Version.Parse(Path.GetFileName(x))))
            {
                string result = Path.Combine(dir, "2MGFX.exe");
                var v = Version.Parse(Path.GetFileName(dir));
                if (v == version)
                {
                    return result;
                }
                if (version.Build != -1 && v == new Version (version.Major, version.Minor, version.Build))
                {
                    return result;
                }
                if (v == new Version(version.Major, version.Minor))
                {
                    return result;
                }
            }
            return null;
        }

        static byte[] RunMGCB(string code, string platform, string version, out string error, out Version compiledWith)
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
            var mgfxExe =  Find2MGFXForVersion(compiledWith);
            
            if (string.IsNullOrEmpty (mgfxExe) || !File.Exists (mgfxExe))
            {
                compiledWith = new Version(3, 5);
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
