Shader "JSH/2D/JSHSpriteLit" {

    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _LightMap ("Light Texture", 2D) = "white" {}
    }

    SubShader {
        
        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }
        LOD 100

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            sampler2D _MainTex;
            sampler2D _LightMap;

            struct a2v {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 diff : COLOR;
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
                float3 ambient = UNITY_LIGHTMODEL_AMBIENT;
                float3 color = tex2D(_MainTex, i.uv).rgb * (ambient + tex2D(_LightMap, i.uv).rgb);
                o.color = float4(color, 1.0);
                return o;
            }

            ENDCG

        }
    }

    Fallback "Diffuse"

}