using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace MonoGame.RemoteEffect
{
	[ContentProcessor (DisplayName = "Remote Effect Material Processor - Infinitespace Studios")]
	public class RemoteEffectMaterialProcessor : MaterialProcessor
	{
		protected override ExternalReference<CompiledEffectContent> BuildEffect (ExternalReference<EffectContent> effect, ContentProcessorContext context)
		{
			return context.BuildAsset<EffectContent, CompiledEffectContent> (effect, typeof(RemoteEffectModelProcessor).Name);
		}
	}
}
