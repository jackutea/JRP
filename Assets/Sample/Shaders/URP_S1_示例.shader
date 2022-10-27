Shader "Custom/URP/3D/URP_S1_示例" {

    Properties {
        _BaseMap ("Base (RGB)", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
    }

    SubShader {

        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
        }

        HLSLINCLUDE
        // 替代 #include "UnityCG.cginc"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        // 除了贴图外，要暴露在Inspector面板上的变量都需要缓存到CBUFFER中
        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _BaseColor;
        CBUFFER_END
        
        ENDHLSL

        Pass {

            Tags { "LightMode" = "UniversalForward" }


            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 posOS : POSITION;
                float2 uv : TEXCOORD;
            };

            struct v2f {
                float4 posCS : SV_POSITION;
                float2 uv : TEXCOORD;
            };

            TEXTURE2D(_BaseMap); // 相当于 sampler2D _MainTex
            SAMPLER(sampler_BaseMap);

            v2f vert(a2v IN) {
                v2f OUT;
                // 相当于 UnityObjectToClipPos(v.vertex)
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.posOS.xyz);
                OUT.posCS = positionInputs.positionCS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f IN) {
                output OUT;

                // 相当于 tex2D(_MainTex, i.uv)
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                OUT.color = min(baseMap, float4(IN.uv.x, IN.posCS.xy, 1));

                return OUT;
            }

            ENDHLSL

        }

    }

    Fallback "Diffuse"
}