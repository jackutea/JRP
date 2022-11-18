Shader "Practice/practice_S5_线"
{
    Properties
    {
        _Width ("线宽", Range(0.001, 0.1)) = 0.01
        _Thickness ("厚度", Range(0.001, 0.1)) = 0.01
        _Color ("颜色", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 1000

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Include/JackCG2D.cginc"
            
            float _Width;
            float4 _Color;
            float _Thickness;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv: TEXCOORD0;
                float4 pos: TEXCOORD1;
                float4 screenPos: TEXCOORD2;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.pos = v.vertex;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            // 逐行
            // 逐像素上色
            float4 frag(v2f i) : SV_Target
            {
                float2 pos = i.pos.xy;
                float2 uv = i.uv.xy;

                fixed online_1 = OnLine(0.25, pos.y, _Width);
                fixed online_2 = OnLine(0.25, pos.x, _Width);
                fixed incircle_1 = InCircle(pos.xy, float2(-0.1, -0.1), 0.25);
                fixed3 color = online_1 * _Color.rgb;
                color += online_2 * _Color.rgb;
                color += incircle_1 * _Color.rgb;
                return float4(color, 1);
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
