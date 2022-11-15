Shader "JRP/3D/jrp_3d_lit" {

    Properties {
        _Diffuse ("Diffuse", Color) = (1,1,1,1)
        _Specular ("Specular", Color) = (1,1,1,1)
        _Gloss ("Gloss", Range(0,50)) = 0.5
    }

    SubShader {

        Name "JRPLit"

        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
            "LightMode" = "JRPLit"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(_CustomLight)
        float3 _DirectLightDir;
        float3 _DirectLightColor;
        float _DirectLightIntensity;
        CBUFFER_END

        UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
        UNITY_DEFINE_INSTANCED_PROP(float4, _Diffuse)
        UNITY_DEFINE_INSTANCED_PROP(float4, _Specular)
        UNITY_DEFINE_INSTANCED_PROP(float, _Gloss)
        UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

        ENDHLSL

        pass {

            HLSLPROGRAM
            #pragma multi_compile_instancing
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 posOS: POSITION;
                float4 normalOS: NORMAL;
            };

            struct v2f {
                float4 posCS: SV_POSITION;
                float3 normalWS: TEXCOORD0;
                float3 posWS: TEXCOORD1;
            };

            v2f vert(a2v v) {
                v2f o;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.posOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(v.normalOS.xyz);
                o.posCS = vertexInput.positionCS;
                o.normalWS = normalInput.normalWS;
                o.posWS = vertexInput.positionWS;
                return o;
            }

            struct output {
                float4 color: SV_Target;
            };

            output frag(v2f i) {
                output o;

                float3 lightDir = _DirectLightDir;
                float ndot = dot(i.normalWS, normalize(lightDir));
                float nl = max(ndot, 0.0);

                // diffuse
                float3 diff = _DirectLightColor.rgb * _Diffuse.rgb * nl * _DirectLightIntensity;

                // 环境光
                // diff += UNITY_LIGHTMODEL_AMBIENT.rgb;

                // 高光
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.posWS);
                float3 reflectDir = reflect(-lightDir, i.normalWS);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), _Gloss);
                float3 specular = _Specular.rgb * spec * _DirectLightColor.rgb;

                diff += specular;
                
                o.color = float4(diff, 1);

                return o;
            }

            ENDHLSL

        }

    }

    Fallback "Diffuse"
}