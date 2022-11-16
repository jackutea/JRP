using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class CameraRenderer {

        Camera cam;
        public Camera Cam => cam;

        CommandBuffer buffer;

        public CameraRenderer() {
            this.buffer = new CommandBuffer() { name = "Camera" };
        }

        public void Ctor(Camera cam) {
            this.cam = cam;
        }

        public void SetupCamera(ScriptableRenderContext ctx) {

            // - 设置相机属性
            SetupCameraProperties(ctx);

            // - 清除渲染的残留
            ClearRenderTarget();

            // - 设置相机缓冲区
            BeginSample();
            ExecuteBuffer(ctx);

        }

        public void SetupCameraProperties(ScriptableRenderContext ctx) {
            ctx.SetupCameraProperties(cam);
        }

        public void ClearRenderTarget() {
            var flags = cam.clearFlags;
            buffer.ClearRenderTarget(
                (flags & CameraClearFlags.Depth) != 0,
                (flags & CameraClearFlags.Color) != 0,
                (flags & CameraClearFlags.Color) != 0 ? cam.backgroundColor.linear : Color.clear);
        }

        public bool TryGetCullingParams(float shadowDistancce, out ScriptableCullingParameters cullingParams) {
            bool has = cam.TryGetCullingParameters(out cullingParams);
            if (has) {
                cullingParams.shadowDistance = Mathf.Min(shadowDistancce, cam.farClipPlane);
            }
            return has;
        }

        public CullingResults GetCullingResults(ScriptableRenderContext ctx, ref ScriptableCullingParameters cullingParams) {
            return ctx.Cull(ref cullingParams);
        }

        public void DrawRenderers(ScriptableRenderContext ctx , in CullingResults cullingResults, ref DrawingSettings drawingSettings, ref FilteringSettings filteringSettings) {
            ctx.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        }

        public void DrawGizmos(ScriptableRenderContext ctx, GizmoSubset gizmoSubset) {
            ctx.DrawGizmos(cam, gizmoSubset);
        }

        public void DrawSkybox(ScriptableRenderContext ctx) {
            CameraClearFlags flags = cam.clearFlags;
            if ((flags & CameraClearFlags.Skybox) != 0) {
                ctx.DrawSkybox(cam);
            }
        }

        public void BeginSample() {
            buffer.BeginSample(buffer.name);
        }

        public void EndSample() {
            buffer.EndSample(buffer.name);
        }

        public void ExecuteBuffer(ScriptableRenderContext ctx) {
            ctx.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        public void Submit(ScriptableRenderContext ctx) {
            EndSample();
            ExecuteBuffer(ctx);
            ctx.Submit();
        }

    }

}