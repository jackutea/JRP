using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    internal static class JRPLightRendererUtil {

        internal static void Setup(JRPFacades facades, in ScriptableRenderContext ctx) {
            
            var lightBuffer = facades.LightBuffer;
            lightBuffer.BeginSample(lightBuffer.name);
            SetupDirectLight(facades, lightBuffer);

            JRPShadowRendererUtil.Setup(ctx);

            lightBuffer.EndSample(lightBuffer.name);
            ctx.ExecuteCommandBuffer(lightBuffer);
            lightBuffer.Clear();
        }

        static void SetupDirectLight(JRPFacades facades, CommandBuffer lightBuffer) {
            Light light = RenderSettings.sun;

            var lightSetting = facades.SettingModel.lightSetting;
            lightBuffer.SetGlobalFloat(lightSetting.PropLightIntensityID, light.intensity);
            lightBuffer.SetGlobalVector(lightSetting.PropLightColorID, light.color.linear);
            lightBuffer.SetGlobalVector(lightSetting.PropLightDirectionID, -light.transform.forward);
        }

    }

}