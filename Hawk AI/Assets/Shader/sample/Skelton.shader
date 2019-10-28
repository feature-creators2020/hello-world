Shader "Unlit/Skelton"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		AlphaTest Off
		ZTest Always
		ZWrite On
		//Blend SrcAlpha SrcAlpha

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
				float3 normal : TEXCOORD0;
				float3 eye    : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 eye    : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = normalize(v.normal);
				o.eye =  -_WorldSpaceCameraPos;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = fixed4(0.3, 0.3, 1.0, 0.3);
				float3 N = normalize(i.normal);
				float3 E = normalize(i.eye);	
				//ハイライトの計算
				float3 H = normalize(-ObjSpaceLightDir(i.vertex) + E);
				float S = pow(max(0.0f, dot(N, H)), 30.0f) * 5.0f;
				col.rgb = normalize(col.rgb + S);
				return col;
            }
            ENDCG
        }
    }
}
