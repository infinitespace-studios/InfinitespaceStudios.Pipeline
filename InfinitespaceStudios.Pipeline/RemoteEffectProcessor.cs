using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content;
using System.Text;
using System.ComponentModel; 

namespace InfinitespaceStudios.Pipeline.Processors
{
	[ContentProcessor (DisplayName = "Remote Effect Processor - Infinitespace Studios")]
	class RemoteEffectProcessor : EffectProcessor
	{
		[DefaultValue("pipeline.infinitespace-studios.co.uk")]
		public string RemoteAddress { get; set; }

		[DefaultValue ("443")]
		public string RemotePort { get; set; }

        [DefaultValue ("https")]
        public string Protocol { get; set; }

		public RemoteEffectProcessor ()
		{
			RemotePort = "443";
            Protocol = "https";
			RemoteAddress = "pipeline.infinitespace-studios.co.uk";
		}

		public override CompiledEffectContent Process (EffectContent input, ContentProcessorContext context)
		{
			if (Environment.OSVersion.Platform != PlatformID.Unix) {
				return base.Process (input, context);
			}
			var code = input.EffectCode;
			var platform = context.TargetPlatform;
			var client = new HttpClient ();
			client.BaseAddress = new Uri (string.Format ("{0}://{1}:{2}/", Protocol, RemoteAddress, RemotePort));
			var response = client.PostAsync ("api/Effect", new StringContent (JsonSerializer (new Data  () {
				Platform = platform.ToString(),
				Code = code
			}), Encoding.UTF8, "application/json")).Result;
			if (response.IsSuccessStatusCode) {
				string data = response.Content.ReadAsStringAsync ().Result;
				var result = JsonDeSerializer (data);
				if (!string.IsNullOrEmpty (result.Error)) {
					throw new Exception (result.Error);
				}
				if (result.Compiled == null || result.Compiled.Length == 0)
					throw new Exception ("There was an error compiling the effect");
				return new CompiledEffectContent (result.Compiled);
			} else {
				throw new Exception (response.StatusCode.ToString ());
			}
			return null;
		}

		public string JsonSerializer(Data objectToSerialize) 
		{ 
			return Newtonsoft.Json.JsonConvert.SerializeObject (objectToSerialize, Newtonsoft.Json.Formatting.None);
		} 
		public Result JsonDeSerializer(string data) 
		{ 
			return (Result)Newtonsoft.Json.JsonConvert.DeserializeObject (data, typeof(Result));
		} 
	}
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


