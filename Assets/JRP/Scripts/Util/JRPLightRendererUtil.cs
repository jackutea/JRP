using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    internal static class JRPLightRendererUtil {

        internal static void Setup(CommandBuffer lightBuffer, in ScriptableRenderContext ctx) {
            lightBuffer.BeginSample(lightBuffer.name);
            SetupDirectLight(lightBuffer);
            lightBuffer.EndSample(lightBuffer.name);
            ctx.ExecuteCommandBuffer(lightBuffer);
            lightBuffer.Clear();
        }

        static void SetupDirectLight(CommandBuffer lightBuffer) {
            Light light = RenderSettings.sun;
            lightBuffer.SetGlobalVector(JRPConfig.LIGHT_DIR_COLOR_ID, light.color.linear);
            lightBuffer.SetGlobalVector(JRPConfig.LIGHT_DIR_DIR_ID, -light.transform.forward);
        }

    }

}