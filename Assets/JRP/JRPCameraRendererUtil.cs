using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    internal static class JRPCameraRendererUtil {

        internal static void RenderCamera(JRPFacades facades, ScriptableRenderContext ctx, Camera cam) {

            // - Setup
            Setup(facades.CameraBuffer, ctx, cam);

            // - 相机空间裁剪
            ScriptableCullingParameters cullingParams;
            bool hasCullingParam = cam.TryGetCullingParameters(out cullingParams);
            if (!hasCullingParam) {
                return;
            }

#if UNITY_EDITOR
            // - Scene UI
            EditorPrepareSceneUI(cam);
#endif

            var cullingResults = ctx.Cull(ref cullingParams);

            // - 画不透明
            DrawOpaqueObjects(ctx, cam, cullingResults);

            // - 画天空盒
            DrawSkyBox(ctx, cam);

            // - 画透明
            DrawTransparentObjects(ctx, cam, cullingResults);

#if UNITY_EDITOR
            // - 画不支持
            EditorDrawUnsupportedObjects(ctx, cam, cullingResults);

            // - Gizmos
            EditorDrawGizmos(ctx, cam);
#endif

            // - 提交
            Submit(facades.CameraBuffer, ctx);

        }

        // ==== Begin ====
        static void Setup(CommandBuffer cameraBuffer, ScriptableRenderContext ctx, Camera cam) {

            // - 设置相机属性
            ctx.SetupCameraProperties(cam);

            // - 清除渲染的残留
            var flags = cam.clearFlags;
            cameraBuffer.ClearRenderTarget(
                (flags & CameraClearFlags.Depth) != 0,
                (flags & CameraClearFlags.Color) != 0,
                (flags & CameraClearFlags.Color) != 0 ? cam.backgroundColor.linear : Color.clear);

            // - 设置相机缓冲区
            cameraBuffer.BeginSample(JRPConfig.BUFFER_CAMERA);
            Execute(cameraBuffer, ctx);

        }

        // ==== Draw ====
        static void DrawSkyBox(ScriptableRenderContext ctx, Camera cam) {

            // - 画天空盒
            CameraClearFlags flags = cam.clearFlags;
            if ((flags & CameraClearFlags.Skybox) != 0) {
                ctx.DrawSkybox(cam);
            }

        }

        static void DrawOpaqueObjects(ScriptableRenderContext ctx, Camera cam, in CullingResults cullingResults) {

            // - Sort Settings
            SortingSettings sortingSettings = new SortingSettings(cam);
            sortingSettings.criteria = SortingCriteria.CommonOpaque;

            // - Draw Setting
            DrawingSettings drawingSettings = new DrawingSettings(JRPConfig.SHADER_TAG_DEFAULT, sortingSettings);

            // - Filter Setting
            // 处理透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            // - Draw Renderer
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

        static void DrawTransparentObjects(ScriptableRenderContext ctx, Camera cam, in CullingResults cullingResults) {

            // - Sort Settings
            SortingSettings sortingSettings = new SortingSettings(cam);
            sortingSettings.criteria = SortingCriteria.CommonTransparent;

            // - Draw Setting
            DrawingSettings drawingSettings = new DrawingSettings(JRPConfig.SHADER_TAG_DEFAULT, sortingSettings);

            // - Filter Setting
            // 处理不透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.transparent);

            // - Draw Renderer
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

#if UNITY_EDITOR
        static void EditorDrawUnsupportedObjects(ScriptableRenderContext ctx, Camera cam, in CullingResults cullingResults) {

            SortingSettings sortingSettings = new SortingSettings(cam);

            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.sortingSettings = sortingSettings;

            var arr = JRPConfig.SHADER_TAG_UNDUPPORTED_ARRAY;
            for (int i = 0; i < arr.Length; i += 1) {
                drawingSettings.SetShaderPassName(i, arr[i]);
                drawingSettings.overrideMaterial = JRPConfig.MAT_ERROR;
            }

            FilteringSettings filteringSettings = FilteringSettings.defaultValue;

            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        }

        static void EditorDrawGizmos(ScriptableRenderContext ctx, Camera cam) {
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

        static void Execute(CommandBuffer cameraBuffer, ScriptableRenderContext ctx) {
            ctx.ExecuteCommandBuffer(cameraBuffer);
            cameraBuffer.Clear();
        }

        // ==== End ====
        static void Submit(CommandBuffer cameraBuffer, ScriptableRenderContext ctx) {

            // - 提交
            cameraBuffer.EndSample(cameraBuffer.name);

            Execute(cameraBuffer, ctx);

            ctx.Submit();

        }

    }

}