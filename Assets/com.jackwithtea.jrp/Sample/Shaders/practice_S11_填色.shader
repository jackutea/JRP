Shader "Practice/2D/practice_S11_填色" {

    Properties {
        
    }

    SubShader {

        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        LOD 100

        Pass {

            // DX
            // C Graphics
            CGPROGRAM

            // 主函数
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // app -> vertex
            struct a2v {
                float4 posOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            // vertex -> fragment
            struct v2f {
                float4 posCS : SV_POSITION; // Clip Space
                float2 uv : TEXCOORD0;
                float4 posOS : TEXCOORD1; // Object Space
            };

            v2f vert(a2v v) {
                v2f o;
                o.posCS = UnityObjectToClipPos(v.posOS);
                o.posOS = v.posOS;
                o.uv = v.uv;
                return o;
            }

            // fragment -> output
            struct output {
                float4 color : SV_TARGET; // COLOR
            };

            float InSweep(float2 pt, float delta, float2 center, float radius, float lineWidth, float thickness) {
                float2 d = pt - center;
                float2 dir = float2(cos(delta), -sin(delta)) * radius;
                float h = clamp(dot(d, dir) / dot(dir, dir), 0.0, 1.0);
                float l = length(d - dir * h);
                return 1 - smoothstep(lineWidth, lineWidth + thickness, l);
            }

            // 从左下 到 右上
            // 逐像素填色
            output frag(v2f i) {
                output o;
                float2 pt = i.posOS.xy;

                // - Circle
                float2 center = float2(0, 0);
                float radius = 0.5;
                float len = length(pt);
                float in_circle = smoothstep(len - 0.01, len + 0.01, radius);
                o.color = float4(1, 1, 1, 1) * in_circle;

                // - Sweep
                float2 start = float2(0, 0);
                float in_sweep = InSweep(pt, _Time.y, start, 1, 0.01, 0.01);
                o.color = float4(1, 1, 0, 1) * in_sweep;
                return o;
            }

            ENDCG

        }

    }

    // 出错
    Fallback "Diffuse"

}