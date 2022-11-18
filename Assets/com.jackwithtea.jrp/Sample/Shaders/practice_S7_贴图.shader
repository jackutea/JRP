Shader "Practice/practice_S7_贴图" {

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {

        Tags { "RenderType" = "Opaque"}
        LOD 100

        pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
            };

            v2f vert(appdata_base v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;
                float2 uv = i.uv.xy;
                float3 color = tex2D(_MainTex, uv).rgb;
                o.color = float4(color, 1);
                return o;
            }

            ENDCG

        }

    }

    Fallback "Diffuse"

}