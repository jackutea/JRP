Shader "Custom/3D/S8_顶点变换" {

    Properties {
        _Size ("Size", Range(0.0, 1.0)) = 0.5
    }

    SubShader {

        Tags { "RenderType" = "Opaque"}
        LOD 100

        pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Assets/Include/JackCG3D.cginc"

            float _Size;
            sampler3D _MainTex;

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 pos: TEXCOORD1;
            };

            v2f vert(appdata_base v) {
                v2f o;
                float4 vt = v.vertex;
                float dt = _SinTime.w;
                vt = lerp(vt, float4(normalize(vt.xyz) * _Size, vt.w), dt);
                o.vertex = UnityObjectToClipPos(vt);
                o.uv = v.texcoord;
                o.pos = v.vertex;
                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;
                o.color = float4(i.uv.xyz, 1);
                return o;
            }

            ENDCG

        }

    }

    Fallback "Diffuse"
}