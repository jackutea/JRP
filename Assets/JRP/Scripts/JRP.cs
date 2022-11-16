using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRP : RenderPipeline {

        JRPFacades facades;
        JRPDomain domain;
        JRPFactory factory;

        public JRP(JRPSettingModel settingModel) {

            // - Ctor
            this.facades = new JRPFacades();
            this.domain = new JRPDomain();
            this.factory = new JRPFactory();

            // - Inject
            this.facades.Inject(settingModel);

            // - Init
            settingModel.Init();

            // - SRP Batch
            var batchingSetting = settingModel.batchingSetting;
            GraphicsSettings.useScriptableRenderPipelineBatching = batchingSetting.isEnableSRPBatching;

            Debug.Log("JRP Created");

        }

        protected override void Render(ScriptableRenderContext ctx, Camera[] cameras) {

            var cameraDomain = domain.CameraDomain;
            var cameraRepo = facades.Repo.CameraRepo;

            var lightDomain = domain.LightDomain;
            var lightRepo = facades.Repo.LightRepo;

            // ==== Spawn ====
            // - Camera Spawn
            for (int i = 0; i < cameras.Length; i += 1) {
                var cam = cameras[i];
                cameraDomain.SpawnCamera(factory, facades, cam);
            }

            // - Light Spawn
            var lightSun = RenderSettings.sun;
            lightDomain.SpawnSunLight(factory, facades, lightSun);

            // ==== Setup ====
            // - Camera Setup
            cameraRepo.ForEach(cameraRenderer => {

                cameraDomain.SetupCamera(ctx, cameraRenderer);

                // - Light Setup SunLight
                lightDomain.SetupSunLight(facades, ctx, lightRepo.SunLight);

                // ==== Culling && Draw ====
                cameraDomain.CullingAndDraw(facades, ctx, cameraRenderer);

            });

            // ==== Release ====
            // - Camera Release
            cameraRepo.ForEach((cameraRenderer) => {
                factory.ReleaseCameraRenderer(cameraRenderer);
            });
            cameraRepo.Clear();

            // - Light Release
            factory.ReleaseLightRenderer(lightRepo.SunLight);
            lightRepo.SetSunLight(null);

            lightRepo.ForEach((lightRenderer) => {
                factory.ReleaseLightRenderer(lightRenderer);
            });
            lightRepo.Clear();

        }

    }

}