using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public static class JRPShadowRendererUtil {

        internal static void Setup(in ScriptableRenderContext ctx) {

        }

        internal static void ExecuteBuffer(CommandBuffer shadowBuffer, in ScriptableRenderContext ctx) {
            ctx.ExecuteCommandBuffer(shadowBuffer);
            shadowBuffer.Clear();
        }

        static void RenderDirectionalShadows(JRPFacades facades) {
            var shadowSetting = facades.SettingModel.shadowSetting;
            int atlasSize = (int)shadowSetting.atlasSize;
            CommandBuffer shadowBuffer = facades.ShadowBuffer;
            shadowBuffer.GetTemporaryRT(shadowSetting.PropDirectShadowAtlasID,
                                        atlasSize,
                                        atlasSize,
                                        32,
                                        FilterMode.Bilinear,
                                        RenderTextureFormat.Shadowmap);
        }

        internal static void Cleanup(JRPFacades facades, in ScriptableRenderContext ctx) {
            var shadowSetting = facades.SettingModel.shadowSetting;
            CommandBuffer shadowBuffer = facades.ShadowBuffer;
            shadowBuffer.ReleaseTemporaryRT(shadowSetting.PropDirectShadowAtlasID);
            ExecuteBuffer(shadowBuffer, ctx);
        }

    }

}