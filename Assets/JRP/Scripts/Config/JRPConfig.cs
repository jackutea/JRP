using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public static class JRPConfig {

        // - Buffer
        public const string BUFFER_CAMERA = "CameraBuffer";

        // - Shader
        public static readonly ShaderTagId[] SHADER_TAG_SUPPORTED_ARRAY = new ShaderTagId[] {
            SHADER_TAG_UNLIT,
            SHADER_TAG_LIT,
        };

        public static readonly ShaderTagId SHADER_TAG_UNLIT = new ShaderTagId("JRPUnlit");
        public static readonly ShaderTagId SHADER_TAG_LIT = new ShaderTagId("JRPLit");

        public static readonly ShaderTagId[] SHADER_TAG_UNSUPPORTED_ARRAY = new ShaderTagId[] {
            new ShaderTagId("Always"),
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM")
        };

        // - Material
        public static readonly Material MAT_ERROR = new Material(Shader.Find("Hidden/InternalErrorShader"));

    }
}