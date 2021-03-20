Shader "Unlit/Normalshader"
{
    Properties
    {
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

            struct appdata
            {
                float4 vertex : POSITION;
                float4 nor : NORMAL;
            };

            struct v2f
            {
                float4 nor : NORMAL;
                float4 vertex : SV_POSITION;
            };


            void vert (in appdata v,out v2f o)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.nor=v.nor;
            }

            void frag (in v2f i,out fixed4 col) : SV_Target
            {
                fixed4 col = fixed4(i.nor.xyz,1);
                return col;
            }
            ENDCG
        }
    }
}
