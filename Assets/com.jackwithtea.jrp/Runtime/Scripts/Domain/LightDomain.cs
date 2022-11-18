using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class LightDomain {

        public LightDomain() { }

        public void SpawnSunLight(JRPFactory factory, JRPFacades facades, Light light) {
            var lightRepo = facades.Repo.LightRepo;
            var lightRenderer = factory.CreateLightRenderer(light);
            lightRepo.SetSunLight(lightRenderer);
        }

        public void SetupSunLight(JRPFacades facades, ScriptableRenderContext ctx, LightRenderer lightRenderer) {

            lightRenderer.BeginSample();

            SetupGlobalShaderProperties(facades, lightRenderer);

            // TODO: Shadow Setup

            lightRenderer.EndSample();
            lightRenderer.Execute(ctx);

        }

        void SetupGlobalShaderProperties(JRPFacades facades, LightRenderer lightRenderer) {
            var lightSetting = facades.SettingModel.lightSetting;
            lightRenderer.SetGlobalShaderProperty_Intensity(lightSetting.PropLightIntensityID);
            lightRenderer.SetGlobalShaderProperty_Color(lightSetting.PropLightColorID);
            lightRenderer.SetGlobalShaderProperty_Direction(lightSetting.PropLightDirectionID);
        }

    }
}