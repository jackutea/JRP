using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public static class JRPShadowRendererUtil {

        internal static void Setup(in ScriptableRenderContext ctx) {

        }

        internal static void ExecuteBuffer(CommandBuffer shadowBuffer, in ScriptableRenderContext ctx) {
            ctx.ExecuteCommandBuffer(shadowBuffer);
            shadowBuffer.Clear();
        }

    }

}