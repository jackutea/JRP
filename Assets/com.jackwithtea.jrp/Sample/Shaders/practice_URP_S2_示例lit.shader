Shader "Practice/URP/3D/practice_URP_S2_示例lit" {

    Properties {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _SpecularColor ("Specular Color", Color) = (1,1,1,1)
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }

    SubShader {

        Tags {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
        }

        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float4 _SpecularColor;
            float _Smoothness;
        CBUFFER_END
        ENDHLSL

        Pass {

            Tags {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 posOS : POSITION;
                float4 normalOS : NORMAL;
                float4 uv : TEXCOORD;
            };

            struct v2f {
                float4 posCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 posWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            v2f vert (a2v v) {
                v2f o;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(v.posOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(v.normalOS.xyz);
                o.posCS = positionInputs.positionCS;
                o.posWS = positionInputs.positionWS;
                o.normalWS = normalInputs.normalWS;
                o.viewDirWS = GetCameraPositionWS() - positionInputs.positionWS;
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;

                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);

                // 主光
                Light light = GetMainLight();
                half3 diff = LightingLambert(light.color, light.direction, i.normalWS);
                half3 specular = LightingSpecular(light.color, light.direction, normalize(i.normalWS), normalize(i.viewDirWS), _SpecularColor, _Smoothness);
                half3 color = baseMap.xyz * diff * _BaseColor * specular;
                o.color = float4(color, 1);
                return o;
            }

            ENDHLSL

        }

    }

    Fallback "Diffuse"
}