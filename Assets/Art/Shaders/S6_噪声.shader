Shader "Custom/S6_噪声"
{

    Properties {

    }

    SubShader {

        Tags { "RenderType" = "Opaque"}
        LOD 100

        pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Include/JackCGGeneric.cginc"

            struct vert_input {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            vert_input vert(appdata_base v) {
                vert_input o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            struct frag_output {
                float4 color : SV_TARGET;
            };

            frag_output frag(vert_input i) {
                frag_output o;
                float3 color = float3(1, 1, 1);
                float rd = PerlinNoiseCustom(i.uv, 1, 48, 4800, _Time.y);
                o.color = float4(color * rd, 1);
                return o;
            }

            ENDCG

        }

    }

    Fallback "Diffuse"
    
}