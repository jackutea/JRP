Shader "JSH/2D/JSHSpriteLit" {

    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }

    SubShader {
        
        Tags {
            "RenderType" = "Opaque"
        }
        LOD 100

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            struct a2v {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(a2v i) {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                return o;
            }

            struct output {
                float4 color : SV_Target;
            };

            output frag(v2f i) {
                output o;
                o.color = tex2D(_MainTex, i.uv);
                return o;
            }

            ENDCG

        }
    }

    Fallback "Diffuse"

}