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

            var globalSetting = settingModel.bufferSetting;
            CommandBuffer cameraBuffer = new CommandBuffer() { name = globalSetting.cameraBufferName };
            CommandBuffer lightBuffer = new CommandBuffer() { name = globalSetting.lightBufferName };
            CommandBuffer shadowBuffer = new CommandBuffer() { name = globalSetting.shadowBufferName };

            // - Inject
            this.facades.Inject(settingModel, cameraBuffer, lightBuffer, shadowBuffer);

            // - Init
            settingModel.Init();

            // - SRP Batch
            var batchingSetting = settingModel.batchingSetting;
            GraphicsSettings.useScriptableRenderPipelineBatching = batchingSetting.isEnableSRPBatching;

            Debug.Log("JRP Created");

        }

        protected override void Render(ScriptableRenderContext ctx, Camera[] cameras) {

            facades.SetRenderContext(ctx);

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

                cameraDomain.SetupCamera(facades, cameraRenderer);

                // - Light Setup SunLight
                lightDomain.SetupSunLight(facades, lightRepo.SunLight);

                // ==== Culling && Draw ====
                cameraDomain.CullingAndDraw(facades, cameraRenderer);

            });

            // ==== Release ====
            // - Camera Release
            cameraRepo.ForEach((cameraRenderer) => {
                factory.ReleaseCameraRenderer(cameraRenderer);
            });
            cameraRepo.Clear();

            // - Light Release
            lightRepo.ForEach((lightRenderer) => {
                factory.ReleaseLightRenderer(lightRenderer);
            });
            lightRepo.Clear();

        }

    }

}