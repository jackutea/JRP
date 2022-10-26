Shader "Custom/S5_线"
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
                float2 uv = i.uv.xy;
                fixed online = OnLine(uv.x, uv.y, _Width, _Thickness);
                fixed3 color = lerp(0, _Color.rgb, online);
                return float4(color, 1);
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
