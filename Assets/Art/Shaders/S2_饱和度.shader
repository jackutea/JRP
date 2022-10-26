Shader "Custom/S2_饱和度"
{
    Properties
    {
        _ColorA("ColorA", Color) = (1, 1, 1, 1)
        _ColorB("ColorB", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
            fixed4 frag(v2f i) : SV_Target
            {
                fixed3 color = saturate(abs(i.pos*2));
                return fixed4(color, 1);
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
