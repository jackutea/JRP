Shader "JSH/2D/PointLight" {

    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (1,1,1,1)
        _LightRadius ("Light Radius", Range(0, 10)) = 1
        _LightIntensity ("Light Intensity", Range(0, 10)) = 1
    }

    SubShader {

        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        LOD 100

        Lighting Off
        ZWrite Off
        Blend One One

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Assets/com.jackwithtea.jrp/Sample/Include/JackCG2D.cginc"

            sampler2D _MainTex;
            float4 _LightColor;
            float _LightRadius;
            float _LightIntensity;

            struct a2v {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 pos : TEXCOORD1;
            };

            v2f vert(a2v i) {
                v2f o;
                o.vertex = UnityObjectToClipPos(i.vertex * _LightRadius);
                o.uv = i.uv;
                o.pos = i.vertex.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float in_circle = InCircleSmooth(i.pos.xy*2, float2(0, 0), _LightRadius, 0);
                fixed4 color = tex2D(_MainTex, i.uv);
                return color * in_circle;
            }

            ENDCG
        }

    }

    Fallback "Diffuse"

}