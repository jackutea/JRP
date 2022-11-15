using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    internal static class JRPCameraRendererUtil {

        internal static void RenderCamera(JRPFacades facades, in ScriptableRenderContext ctx, Camera cam) {

            // - Setup Camera
            SetupCamera(facades.CameraBuffer, ctx, cam);

            // - Setup Light
            JRPLightRendererUtil.Setup(facades, ctx);

            // - 相机空间裁剪
            ScriptableCullingParameters cullingParams;
            bool hasCullingParam = cam.TryGetCullingParameters(out cullingParams);
            if (!hasCullingParam) {
                return;
            }

            var shadowDistance = facades.SettingModel.shadowSetting.maxDistance;
            cullingParams.shadowDistance = Mathf.Min(shadowDistance, cam.farClipPlane);

#if UNITY_EDITOR
            // - Scene UI
            EditorPrepareSceneUI(cam);
#endif

            // - 剔除
            var cullingResults = ctx.Cull(ref cullingParams);

            // - 画不透明
            DrawOpaqueObjects(facades, ctx, cam, cullingResults);

            // - 画天空盒
            DrawSkyBox(ctx, cam);

            // - 画透明
            DrawTransparentObjects(facades, ctx, cam, cullingResults);

#if UNITY_EDITOR
            // - 画不支持
            EditorDrawUnsupportedObjects(facades, ctx, cam, cullingResults);

            // - Gizmos
            EditorDrawGizmos(ctx, cam);
#endif

            // - 提交
            Submit(facades.CameraBuffer, ctx);

        }

        // ==== Begin ====
        static void SetupCamera(CommandBuffer cameraBuffer, in ScriptableRenderContext ctx, Camera cam) {

            // - 设置相机属性
            ctx.SetupCameraProperties(cam);

            // - 清除渲染的残留
            var flags = cam.clearFlags;
            cameraBuffer.ClearRenderTarget(
                (flags & CameraClearFlags.Depth) != 0,
                (flags & CameraClearFlags.Color) != 0,
                (flags & CameraClearFlags.Color) != 0 ? cam.backgroundColor.linear : Color.clear);

            // - 设置相机缓冲区
            cameraBuffer.BeginSample(cameraBuffer.name);
            Execute(cameraBuffer, ctx);

        }

        // ==== Draw ====
        static void DrawSkyBox(in ScriptableRenderContext ctx, Camera cam) {

            // - 画天空盒
            CameraClearFlags flags = cam.clearFlags;
            if ((flags & CameraClearFlags.Skybox) != 0) {
                ctx.DrawSkybox(cam);
            }

        }

        static void DrawOpaqueObjects(JRPFacades facades, in ScriptableRenderContext ctx, Camera cam, in CullingResults cullingResults) {

            // - Sort Settings
            SortingSettings sortingSettings = new SortingSettings(cam);
            sortingSettings.criteria = SortingCriteria.CommonOpaque;

            // - Draw Setting
            var shaderSetting = facades.SettingModel.shaderSetting;
            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.sortingSettings = sortingSettings;
            drawingSettings.SetShaderPassName(0, new ShaderTagId(shaderSetting.supportedUnlit));
            drawingSettings.SetShaderPassName(1, new ShaderTagId(shaderSetting.supportedLit));

            // - Filter Setting
            // 处理透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            // - Draw Renderer
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

        static void DrawTransparentObjects(JRPFacades facades, in ScriptableRenderContext ctx, Camera cam, in CullingResults cullingResults) {

            // - Sort Settings
            SortingSettings sortingSettings = new SortingSettings(cam);
            sortingSettings.criteria = SortingCriteria.CommonTransparent;

            // - Draw Setting
            var shaderSetting = facades.SettingModel.shaderSetting;
            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.sortingSettings = sortingSettings;
            drawingSettings.SetShaderPassName(0, new ShaderTagId(shaderSetting.supportedUnlit));

            // - Filter Setting
            // 处理不透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.transparent);

            // - Draw Renderer
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

#if UNITY_EDITOR
        static void EditorDrawUnsupportedObjects(JRPFacades facades, in ScriptableRenderContext ctx, Camera cam, in CullingResults cullingResults) {

            SortingSettings sortingSettings = new SortingSettings(cam);

            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.sortingSettings = sortingSettings;

            var settingModel = facades.SettingModel;
            var shaderSetting = settingModel.shaderSetting;
            var arr = shaderSetting.unsupportedLightModeArray;
            if (arr == null) {
                return;
            }

            var materialSetting = settingModel.materialSetting;
            for (int i = 0; i < arr.Length; i += 1) {
                drawingSettings.SetShaderPassName(i, new ShaderTagId(arr[i]));
                drawingSettings.overrideMaterial = materialSetting.error;
            }

            FilteringSettings filteringSettings = FilteringSettings.defaultValue;

            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

        static void EditorDrawGizmos(in ScriptableRenderContext ctx, Camera cam) {
            bool should = UnityEditor.Handles.ShouldRenderGizmos();
            if (should) {
                ctx.DrawGizmos(cam, GizmoSubset.PreImageEffects);
                ctx.DrawGizmos(cam, GizmoSubset.PostImageEffects);
            }
        }

        static void EditorPrepareSceneUI(Camera cam) {
            if (cam.cameraType == CameraType.SceneView) {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(cam);
            }
        }
#endif

        static void Execute(CommandBuffer cameraBuffer, in ScriptableRenderContext ctx) {
            ctx.ExecuteCommandBuffer(cameraBuffer);
            cameraBuffer.Clear();
        }

        // ==== End ====
        static void Submit(CommandBuffer cameraBuffer, in ScriptableRenderContext ctx) {

            // - 提交
            cameraBuffer.EndSample(cameraBuffer.name);

            Execute(cameraBuffer, ctx);

            ctx.Submit();

        }

    }

}