using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public class LightRenderer {

        Light light;
        CommandBuffer buffer;

        public LightRenderer() {
            this.buffer = new CommandBuffer() { name = "Light" };
        }

        public void Ctor(Light light) {
            this.light = light;
        }

        public void SetGlobalShaderProperty_Intensity(int propertyId) {
            buffer.SetGlobalFloat(propertyId, light.intensity);
        }

        public void SetGlobalShaderProperty_Color(int propertyId) {
            buffer.SetGlobalColor(propertyId, light.color.linear);
        }

        public void SetGlobalShaderProperty_Direction(int propertyId) {
            buffer.SetGlobalVector(propertyId, -light.transform.forward);
        }

        public void BeginSample() {
            buffer.BeginSample(buffer.name);
        }

        public void EndSample() {
            buffer.EndSample(buffer.name);
        }

        public void Execute(ScriptableRenderContext ctx) {
            ctx.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

    }

}