Shader "JRP/Common/jrp_unlit_transparent" {

    Properties {
        _BaseMap ("BaseMap", 2D) = "white" {}
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        _Cutoff ("Cutoff", Range(0,1)) = 0.5
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Float) = 1.0
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Float) = 0
        [Enum(Off, 0, On, 1)] _ZWrite ("Z Write", Float) = 1
    }

    SubShader {

        Name "JRPUnlit_Transparent"

        Tags {
            "RenderType" = "Transparent"
            "Queue" = "AlphaTest"
            "LightMode" = "JRPUnlit"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
        UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST);
        UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor);
        UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff);
        UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

        ENDHLSL

        Pass {

            Blend [_SrcBlend] [_DstBlend]
            ZWrite [_ZWrite]

            HLSLPROGRAM
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            struct a2v {
                float4 posOS : POSITION;
                float2 uv : TEXCOORD;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 posCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v v) {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
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
                UNITY_SETUP_INSTANCE_ID(i);
                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                float4 color = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
                o.color = baseMap * color;
                clip(o.color.a - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff));
                return o;
            }

            ENDHLSL

        }
    }

    Fallback "Diffuse"

}