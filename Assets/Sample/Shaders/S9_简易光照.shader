Shader "Custom/3D/S9_简易光照" {

    Properties {

    }

    SubShader {

        Tags { "RenderType" = "Opaque"}
        LOD 100

        Pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 diff : COLOR0;
            };

            v2f vert(appdata_base v) {
                
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);

                // 光方向与顶点法线的点积
                // 1 表示光线与法线完全一致，0 表示垂直，-1 表示完全相反
                half ndot = dot(worldNormal, _WorldSpaceLightPos0.xyz);
                half nl = max(0.15, ndot);
                o.diff = nl * _LightColor0;

                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;
                o.color = float4(1, 1, 0, 1);
                o.color *= i.diff;
                return o;
            }

            ENDCG

        }

    }

    Fallback "Diffuse"

}