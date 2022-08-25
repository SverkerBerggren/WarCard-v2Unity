// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/CellTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.normal = IN.normal;
                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
            

                fixed4 col = tex2D(_MainTex, i.uv);

                //float StepSize = 0.0005;
                //if (col.r != tex2D(_MainTex, i.uv + float2(StepSize, StepSize)).r)
                //{
                //    col = fixed4(0, 0, 0, 0);
                //}

                //float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
                //float DotResult = dot(normalize(forward), normalize(mul(unity_ObjectToWorld, -i.normal)));
                //if (DotResult < 0)
                //{
                //    DotResult = 0;
                //}
                //if (DotResult < 0.4) 
                //{
                //    col = fixed4(0, 0, 0, 0);
                //}
                
                //col = tex2D(_MainTex, i.uv);
                //col += fixed4(,1,1,1);
                //col.rgb = fixed3(1, 1, 1);

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
