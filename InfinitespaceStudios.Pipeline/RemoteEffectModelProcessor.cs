using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace MonoGame.RemoteEffect
{
	[ContentProcessor (DisplayName = "Remote Effect Model Processor - Infinitespace Studios")]
	public class RemoteEffectModelProcessor : ModelProcessor
	{
		protected override MaterialContent ConvertMaterial (MaterialContent material, ContentProcessorContext context)
		{
			var parameters = new OpaqueDataDictionary ();
			parameters.Add ("ColorKeyColor", ColorKeyColor);
			parameters.Add ("ColorKeyEnabled", ColorKeyEnabled);
			parameters.Add ("GenerateMipmaps", GenerateMipmaps);
			parameters.Add ("PremultiplyTextureAlpha", PremultiplyTextureAlpha);
			parameters.Add ("ResizeTexturesToPowerOfTwo", ResizeTexturesToPowerOfTwo);
			parameters.Add ("TextureFormat", TextureFormat);
			parameters.Add ("DefaultEffect", DefaultEffect);

			return context.Convert<MaterialContent, MaterialContent> (material, typeof(RemoteEffectMaterialProcessor).Name, parameters);
		}
	}
}
