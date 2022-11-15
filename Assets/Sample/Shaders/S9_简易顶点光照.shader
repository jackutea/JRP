Shader "Custom/3D/S9_简易顶点光照" {

    Properties {

        // 漫反射
        _Diffuse ("Diffuse", Color) = (1,1,1,1)

        // 高光
        _Specular ("Specular", Color) = (1,1,1,1)

        _Gloss ("Gloss", Range(8, 200)) = 10
        
    }

    SubShader {

        Tags {
            "RenderType" = "Opaque"
            "LightMode" = "JRPLit"
        }

        LOD 100

        Pass {

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            float4 _Diffuse;
            float4 _Specular;
            float _Gloss;

            struct v2f {
                float4 vertex : SV_POSITION;
                float3 diff : COLOR;
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

                // 漫反射光照
                float3 diff = nl * _LightColor0.rgb * _Diffuse.rgb;

                // 环境光
                diff += UNITY_LIGHTMODEL_AMBIENT.rgb;

                // ==== 视野内的光照计算 ====
                // 高光
                float3 reflectDir = normalize(reflect(-lightDir, worldNormal));

                float3 viewDir = normalize(_WorldSpaceCameraPos - mul(v.vertex, unity_WorldToObject).xyz);

                float spec = pow(max(dot(viewDir, reflectDir), 0.0), _Gloss);
                float3 specColor = _LightColor0 * _Specular * spec;

                diff += specColor;

                o.diff = diff;

                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;
                o.color = float4(i.diff, 1);
                return o;
            }

            ENDCG

        }

    }

    Fallback "Diffuse"

}