using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRP : RenderPipeline {

        protected override void Render(ScriptableRenderContext ctx, Camera[] cameras) {

            Dictionary<int, object> all = new Dictionary<int, object>();

            bool has0 = all.TryGetValue(0, out object value);
            bool has1 = all.TryGetValue(1, out _);

            for (int i = 0; i < cameras.Length; i += 1) {
                var cam = cameras[i];
                DrawOneCamera(ctx, cam);
            }

            // 提交
            ctx.Submit();

        }

        static void DrawOneCamera(ScriptableRenderContext ctx, Camera cam) {

            // - 画天空盒
            DrawOneCamera_SkyBox(ctx, cam);

            // - 相机空间裁剪
            bool hasCullingParam = cam.TryGetCullingParameters(out ScriptableCullingParameters cullingParams);
            if (hasCullingParam) {
                var cullingResults = ctx.Cull(ref cullingParams);

                DrawOneCamera_Opaque(ctx, cullingResults);
                DrawOneCamera_Transparent(ctx, cullingResults);
            }

        }

        static void DrawOneCamera_SkyBox(ScriptableRenderContext ctx, Camera cam) {

            // - 设置相机属性
            ctx.SetupCameraProperties(cam);

            // - 画天空盒
            CameraClearFlags flags = cam.clearFlags;
            if ((flags & CameraClearFlags.Skybox) != 0) {
                ctx.DrawSkybox(cam);
            }

        }

        static void DrawOneCamera_Opaque(ScriptableRenderContext ctx, in CullingResults cullingResults) {

            // - Draw Setting
            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.SetShaderPassName(1, new ShaderTagId("SRPDefaultUnlit")); // LightMode 为 SRPDefaultUnlit 的 Shader
            drawingSettings.sortingSettings = new SortingSettings { criteria = SortingCriteria.CommonOpaque };

            // - Filter Setting
            // 处理透明与不透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            // - Draw Renderer
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

        static void DrawOneCamera_Transparent(ScriptableRenderContext ctx, in CullingResults cullingResults) {

            // - Draw Setting
            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.SetShaderPassName(1, new ShaderTagId("SRPDefaultUnlit")); // LightMode 为 SRPDefaultUnlit 的 Shader
            drawingSettings.sortingSettings = new SortingSettings { criteria = SortingCriteria.CommonTransparent };

            // - Filter Setting
            // 处理透明与不透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.transparent);

            // - Draw Renderer
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

    }

}