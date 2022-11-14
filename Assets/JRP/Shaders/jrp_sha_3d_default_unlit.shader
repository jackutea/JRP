Shader "JRP/3D/jrp_3d_default_unlit" {

    Properties {
        _BaseMap ("BaseMap", 2D) = "white" {}
    }

    SubShader {

        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "LightMode" = "JRPLightMode"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
        CBUFFER_END

        ENDHLSL

        Pass {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            struct a2v {
                float4 posOS : POSITION;
                float2 uv : TEXCOORD;
            };

            struct v2f {
                float4 posCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v) {
                v2f o;
                VertexPositionInputs posInput = GetVertexPositionInputs(v.posOS.xyz);
                o.posCS = posInput.positionCS;
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;
                o.color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                return o;
            }

            ENDHLSL

        }
    }

    Fallback "Diffuse"

}