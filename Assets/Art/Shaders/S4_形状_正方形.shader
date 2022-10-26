Shader "Custom/S4_形状_正方形"
{
    Properties
    {
        _XOffset ("X Offset", Range(-1,1)) = 0
        _YOffset ("Y Offset", Range(-1,1)) = 0
        _Scale ("Scale", Range(0,2)) = 1
        _Rotate ("Rotate", Range(-1, 1)) = 0
        _Anchor ("Anchor", Vector) = (0,0,0,0)
        _Size ("Size", Vector) = (1,1,1,1)
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
            float _Rotate;
            float4 _Anchor;
            float4 _Size;

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
                float2 pos = i.pos * 2.0;
                float2 size = _Size;
                float2x2 mat_s = MatScale(_Scale);
                float2x2 mat_r = MatRotate(_Rotate * UNITY_PI);
                float2x2 mat = mul(mat_r, mat_s);
                pos = mul(mat, pos);
                float in_circle = InRectAnchor(pos.xy, _Anchor.xy, center, size);
                return float4(1, 1, 0, 1) * in_circle;
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
