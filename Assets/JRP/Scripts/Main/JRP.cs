using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRP : RenderPipeline {

        JRPFacades facades;

        public JRP(JRPSettingModel settingModel) {

            // - Ctor
            this.facades = new JRPFacades();

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

        // 1. 相机剔除
        // 2. 画不透明
        // 3. 画透明
        // 4. 画无效
        // 4. 画 UI
        // 5. 画后处理
        // 6. 画天空盒
        protected override void Render(ScriptableRenderContext ctx, Camera[] cameras) {

            Dictionary<int, object> all = new Dictionary<int, object>();

            bool has0 = all.TryGetValue(0, out object value);
            bool has1 = all.TryGetValue(1, out _);

            for (int i = 0; i < cameras.Length; i += 1) {
                var cam = cameras[i];
                JRPCameraRendererUtil.RenderCamera(facades, ctx, cam);
            }

        }

    }

}