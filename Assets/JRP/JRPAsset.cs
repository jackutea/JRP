using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    [CreateAssetMenu(menuName = "Rendering/Jack Render Pipeline")]
    public class JRPAsset : RenderPipelineAsset {

        protected override RenderPipeline CreatePipeline() {
            return new JRP();
        }

    }

}