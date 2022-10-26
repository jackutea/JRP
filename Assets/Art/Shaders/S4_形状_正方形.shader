Shader "Custom/S4_形状_正方形"
{
    Properties
    {
        _XOffset ("X Offset", Range(-1,1)) = 0
        _YOffset ("Y Offset", Range(-1,1)) = 0
        _Scale ("Scale", Range(0,2)) = 1
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

            float _XOffset;
            float _YOffset;
            float _Scale;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 pos: TEXCOORD1;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.pos = v.vertex;
                return o;
            }

            // 逐行
            // 逐像素上色
            float4 frag(v2f i) : SV_Target
            { 
                float2 center = float2(_XOffset, _YOffset);
                float2 pos = i.pos.xy;
                float2 size = float2(0.5, 0.5);
                float2x2 mat_s = MatScale(_Scale);
                size = mul(mat_s, size);
                float in_circle = InRect(pos, center, size);
                if (in_circle == 1) {
                    return float4(1, 1, 0, 1);
                    } else {
                    return float4(0, 0, 0, 1);
                }
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
