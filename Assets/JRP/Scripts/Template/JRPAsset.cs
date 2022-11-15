using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    [CreateAssetMenu(menuName = "Rendering/Jack Render Pipeline")]
    public class JRPAsset : RenderPipelineAsset {

        [SerializeField] JRPSettingModel settingModel;

        protected override RenderPipeline CreatePipeline() {
            return new JRP(settingModel);
        }

    }

}