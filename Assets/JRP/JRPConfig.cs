using UnityEngine;
using UnityEngine.Rendering;

namespace JackRenderPipeline {

    public static class JRPConfig {

        // - Buffer
        public const string BUFFER_CAMERA = "CameraBuffer";

        // - Shader
        public static readonly ShaderTagId SHADER_TAG_DEFAULT = new ShaderTagId("SRPDefaultUnlit");
        public static readonly ShaderTagId[] SHADER_TAG_UNDUPPORTED_ARRAY = new ShaderTagId[] {
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