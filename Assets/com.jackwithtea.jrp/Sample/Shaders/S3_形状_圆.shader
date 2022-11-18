Shader "Custom/S3_形状_圆"
{
    Properties
    {
        _XOffset ("X Offset", Range(-1,1)) = 0
        _YOffset ("Y Offset", Range(-1,1)) = 0
        _Smooth ("Smooth", Range(0,1)) = 0
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
            #include "Assets/Src/Include/JackCG2D.cginc"

            float _XOffset;
            float _YOffset;
            float _Smooth;

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
                float in_circle = InCircleSmooth(i.pos.xy, center, 0.5, _Smooth);
                return float4(1, 1, 0, 1) * in_circle;
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
