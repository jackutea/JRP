Shader "JRP/3D/jrp_3d_default_unlit" {

    Properties {
        
    }

    SubShader {

        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            struct a2v {
                float4 posOS : POSITION;
            };

            struct v2f {
                float4 posCS : SV_POSITION;
            };

            v2f vert(a2v v) {
                v2f o;
                o.posCS = UnityObjectToClipPos(v.posOS);
                return o;
            }

            struct output {
                float4 color : SV_TARGET;
            };

            output frag(v2f i) {
                output o;
                o.color = float4(1, 1, 1, 1);
                return o;
            }

            ENDCG

        }
    }

    Fallback "Diffuse"

}