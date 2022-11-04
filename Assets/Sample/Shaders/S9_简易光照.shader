Shader "Custom/3D/S9_简易光照" {

    Properties {

        // 漫反射
        _Diffuse ("Diffuse", Color) = (1,1,1,1)

        // 环境光
        _Ambient ("Ambient", Color) = (0.5,0.5,0.5,1)

        // 高光
        _Specular ("Specular", Color) = (1,1,1,1)
        
    }

    SubShader {

        Tags {
            "LightMode" = "ForwardBase"
            "RenderType" = "Opaque"
        }

        LOD 100

        Pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            float4 _Diffuse;
            float4 _Ambient;
            float4 _Specular;

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 diff : COLOR;
            };

            v2f vert(appdata_base v) {
                
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 worldNormal = UnityObjectToWorldNormal(v.normal);

                // 光方向与顶点法线的点积
                // 1 表示光线与法线完全一致，0 表示垂直，-1 表示完全相反
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                float ndot = dot(worldNormal, normalize(lightDir));
                float nl = max(0, ndot);

                // 光照
                float3 diff = nl * _LightColor0.rgb;

                // 漫反射光照
                diff *= _Diffuse.rgb;

                // 环境光
                diff += _Ambient.rgb;

                // ==== 视野内的光照计算 ====
                // 高光
                float3 reflectDir = normalize(reflect(-lightDir, worldNormal));

                float3 viewDir = normalize(_WorldSpaceCameraPos - v.vertex.xyz);

                float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
                float3 specColor = _Specular * spec;

                diff += specColor;

                o.diff = float4(diff, 1);

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