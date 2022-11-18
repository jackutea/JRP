using System;

namespace JackRenderPipeline {

    [Serializable]
    public class JRPShaderSettingModel {

        /*
            "JRPLit"
            "JRPUnlit"
        */
        public string supportedLit;
        public string supportedUnlit;
        public string[] supportedOtherLightModeArray;

        /*
            "SRPDefaultUnlit"
            "Always"
            "ForwardBase"
            "PrepassBase"
            "Vertex"
            "VertexLMRGBM"
            "VertexLM"
        */
        public string[] unsupportedLightModeArray;

    }
    
}