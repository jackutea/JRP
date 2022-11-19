using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class JRPFactory {

        Pool<CameraRenderer> cameraPool;
        Pool<LightRenderer> lightPool;

        public JRPFactory() {
            this.cameraPool = new Pool<CameraRenderer>(() => new CameraRenderer(), 4);
            this.lightPool = new Pool<LightRenderer>(() => new LightRenderer(), 1);
        }

        // - Camera
        public CameraRenderer CreateCameraRenderer(Camera cam) {
            var cameraRenderer = cameraPool.Take();
            cameraRenderer.Ctor(cam);
            return cameraRenderer;
        }

        public void ReleaseCameraRenderer(CameraRenderer cameraRenderer) {
            this.cameraPool.Release(cameraRenderer);
        }

        // - Light
        public LightRenderer CreateLightRenderer(Light light) {
            var lightRenderer = lightPool.Take();
            lightRenderer.Ctor(light);
            return lightRenderer;
        }

        public void ReleaseLightRenderer(LightRenderer lightRenderer) {
            if (lightRenderer == null) {
                return;
            }
            this.lightPool.Release(lightRenderer);
        }

    }

}