Shader "Custom/S1_内描边"
{
    Properties
    {
        _ColorA("ColorA", Color) = (1, 1, 1, 1)
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

            fixed4 _ColorA;

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half2 uv : TEXCOORD0;
                float4 color: COLOR;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color;
                return o;
            }

            struct fragout {
                fixed4 color : SV_Target;
            };

            // 逐行
            // 逐像素上色
            // 下方函数等同于 fixed4 frag(v2f i) : SV_Target{}
            fragout frag(v2f i)
            {
                fragout o;

                half2 uv = i.uv;
                half dx = smoothstep(0.02, 0.98, uv.x);
                half dy = smoothstep(0.02, 0.98, uv.y);
                if (dx == 0 || dx == 1 || dy == 0 || dy == 1) {
                    o.color = _ColorA;
                } else {
                    o.color = i.color;
                }

                return o;
            }

            ENDCG

        }

        
    }
    FallBack "Diffuse"
}
