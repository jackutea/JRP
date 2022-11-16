using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class CameraDomain {

        public CameraDomain() { }

        internal CameraRenderer SpawnCamera(JRPFactory factory, JRPFacades facades, Camera cam) {
            var cameraRepo = facades.Repo.CameraRepo;
            var cameraRenderer = factory.CreateCameraRenderer(cam);
            cameraRepo.Add(cameraRenderer);
            return cameraRenderer;
        }

        internal void SetupCamera(ScriptableRenderContext ctx, CameraRenderer cameraRenderer) {
            cameraRenderer.SetupCamera(ctx);
        }

        internal void CullingAndDraw(JRPFacades facades, ScriptableRenderContext ctx, CameraRenderer cameraRenderer) {

#if UNITY_EDITOR
            EditorPrepareSceneUI(cameraRenderer);
#endif

            var shadowDistancce = facades.SettingModel.shadowSetting.maxDistance;
            bool has = cameraRenderer.TryGetCullingParams(shadowDistancce, out var cullingParams);
            if (!has) {
                return;
            }

            var cullingResults = cameraRenderer.GetCullingResults(ctx, ref cullingParams);

            DrawOpaqueObjects(facades, ctx, cameraRenderer, cullingResults);

            DrawSkyBox(ctx, cameraRenderer);

            DrawTransparentObjects(facades, ctx, cameraRenderer, cullingResults);

#if UNITY_EDITOR
            EditorDrawUnsupportedObjects(facades, ctx, cameraRenderer, cullingResults);

            EditorDrawGizmos(ctx, cameraRenderer);
#endif

            Submit(ctx, cameraRenderer);

        }

        void DrawOpaqueObjects(JRPFacades facades, ScriptableRenderContext ctx, CameraRenderer cameraRenderer, in CullingResults cullingResults) {

            // - Sort Settings
            SortingSettings sortingSettings = new SortingSettings(cameraRenderer.Cam);
            sortingSettings.criteria = SortingCriteria.CommonOpaque;

            // - Draw Setting
            var shaderSetting = facades.SettingModel.shaderSetting;
            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.sortingSettings = sortingSettings;
            drawingSettings.SetShaderPassName(0, new ShaderTagId(shaderSetting.supportedUnlit));
            drawingSettings.SetShaderPassName(1, new ShaderTagId(shaderSetting.supportedLit));

            var otherSupportedArray = shaderSetting.supportedOtherLightModeArray;
            if (otherSupportedArray != null) {
                for (int i = 0; i < otherSupportedArray.Length; i += 1) {
                    drawingSettings.SetShaderPassName(i + 2, new ShaderTagId(otherSupportedArray[i]));
                }
            }

            // - Filter Setting
            // 处理透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            // - Draw Renderer
            cameraRenderer.DrawRenderers(ctx, cullingResults, ref drawingSettings, ref filteringSettings);

        }

        void DrawSkyBox(ScriptableRenderContext ctx, CameraRenderer cameraRenderer) {
            cameraRenderer.DrawSkybox(ctx);
        }

        void DrawTransparentObjects(JRPFacades facades, ScriptableRenderContext ctx, CameraRenderer cameraRenderer, in CullingResults cullingResults) {

            // - Sort Settings
            SortingSettings sortingSettings = new SortingSettings(cameraRenderer.Cam);
            sortingSettings.criteria = SortingCriteria.CommonTransparent;

            // - Draw Setting
            var shaderSetting = facades.SettingModel.shaderSetting;
            DrawingSettings drawingSettings = new DrawingSettings();
            drawingSettings.sortingSettings = sortingSettings;
            drawingSettings.SetShaderPassName(0, new ShaderTagId(shaderSetting.supportedUnlit));
            drawingSettings.SetShaderPassName(1, new ShaderTagId(shaderSetting.supportedLit));

            var otherSupportedArray = shaderSetting.supportedOtherLightModeArray;
            if (otherSupportedArray != null) {
                for (int i = 0; i < otherSupportedArray.Length; i += 1) {
                    drawingSettings.SetShaderPassName(i + 2, new ShaderTagId(otherSupportedArray[i]));
                }
            }

            // - Filter Setting
            // 处理不透明
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.transparent);

            // - Draw Renderer
            cameraRenderer.DrawRenderers(ctx, cullingResults, ref drawingSettings, ref filteringSettings);

        }

        void Submit(ScriptableRenderContext ctx, CameraRenderer cameraRenderer) {
            cameraRenderer.Submit(ctx);
        }

#if UNITY_EDITOR
        void EditorDrawUnsupportedObjects(JRPFacades facades, ScriptableRenderContext ctx, CameraRenderer cameraRenderer, in CullingResults cullingResults) {

            SortingSettings sortingSettings = new SortingSettings(cameraRenderer.Cam);

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

            cameraRenderer.DrawRenderers(ctx, cullingResults, ref drawingSettings, ref filteringSettings);

        }

        void EditorDrawGizmos(ScriptableRenderContext ctx, CameraRenderer cameraRenderer) {
            bool should = UnityEditor.Handles.ShouldRenderGizmos();
            if (should) {
                cameraRenderer.DrawGizmos(ctx, GizmoSubset.PreImageEffects);
                cameraRenderer.DrawGizmos(ctx, GizmoSubset.PostImageEffects);
            }
        }

        void EditorPrepareSceneUI(CameraRenderer cameraRenderer) {
            var cam = cameraRenderer.Cam;
            if (cam.cameraType == CameraType.SceneView) {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(cam);
            }
        }
#endif

    }

}