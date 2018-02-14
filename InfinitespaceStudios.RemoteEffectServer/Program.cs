using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using InfinitespaceStudios.Pipeline.Processors;

namespace InfinitespaceStudios.RemoteEffectServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var port = args.Length == 1 ? int.Parse (args [0]) : 8001;

            var listener = new System.Net.HttpListener();
            // for Non Admin rights
            // netsh http add urlacl url=http://*:8001/api/Effect user=DOMAIN/user
            listener.Prefixes.Add(string.Format("http://*:{0}/api/Effect/", port));
            listener.Start();
            ThreadPool.QueueUserWorkItem((o) =>
            {
                try
                {
                    Console.WriteLine("Webserver running...");
                    while (listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                var contentLength = (int)ctx.Request.ContentLength64;
                                var buffer = new byte[contentLength];
                                ctx.Request.InputStream.Read(buffer, 0, contentLength);

                                var obj = Encoding.ASCII.GetString(buffer);
                                var d = (Data)JsonConvert.DeserializeObject(obj, typeof(Data));
								string error = String.Empty;
								byte[] buf = RunMGCB(d.Code, d.Platform, out error);

								var result = JsonConvert.SerializeObject (new Result() { Compiled = buf, Error = error });

                                ctx.Response.ContentLength64 = result.Length;
								ctx.Response.OutputStream.Write (Encoding.UTF8.GetBytes (result), 0, result.Length);
                            }
                            catch (Exception ex) {
                                Console.WriteLine(ex.ToString());
                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, listener.GetContext());
                    }
                }
                catch { }
            });
            
            Console.Read();

            listener.Stop();
            listener.Close();
		}



		static byte[] RunMGCB(string code,  string platform, out string error)
		{
            string[] platforms = new string[]
            {
                "DesktopGL",
                "Android",
                "iOS",
                "tvOS",
                "OUYA",
            };
			error = String.Empty;
			var tempPath = Path.GetFileName( Path.ChangeExtension(Path.GetTempFileName (), ".fx"));
			var xnb = Path.ChangeExtension (tempPath, ".mgfx");
			var tempOutput = Path.GetTempPath ();
            File.WriteAllText(Path.Combine(tempOutput, tempPath), code);
            var startInfo = new ProcessStartInfo ();
			var progFiles = Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86);
			startInfo.FileName = Path.Combine (progFiles,"MSBuild", "MonoGame", "v3.0", "Tools", "2MGFX.exe");
            startInfo.WorkingDirectory = tempOutput;
			startInfo.CreateNoWindow = true;
            //startInfo.Arguments = string.Format ("/build:{0} /outputDir:. /platform:{1} /importer:EffectImporter /processor:EffectProcessor",
            //	tempPath, platform);
            var profile = platforms.Contains(platform) ? "OpenGL" : "DirectX_11";
            startInfo.Arguments = string.Format("./{0} ./{1} /Profile:{2}", tempPath, xnb, profile);
            var process = Process.Start (startInfo);
			try {
			    process.WaitForExit ();
			    if (File.Exists (Path.Combine (tempOutput, xnb))) {
				    return File.ReadAllBytes (Path.Combine(tempOutput, xnb));
			    }
			} catch (Exception ex) {
				error = ex.ToString ();
				Console.WriteLine (ex.ToString ());
			}
			finally {
				File.Delete (Path.Combine(tempOutput, tempPath));
				File.Delete (Path.Combine(tempOutput, xnb));
			}
			return new byte[0];
		}
	}
}
namespace InfinitespaceStudios.Pipeline.Processors {
	public class Data {
		public string Platform { get; set; }
		public string Code { get; set; }
	}

	public class Result
	{
		public byte[] Compiled { get; set;  }
		public string Error { get; set;  }
	}
}
