// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/FadeTex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_TopUV("TopUV",Vector) = (0,0,0,0)
		_UnderUV("UnderUV",Vector) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
		Cull Off ZWrite Off ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

                return o;
            }

            sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _TopUV;
			fixed4 _UnderUV;

            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = tex2D(_MainTex, i.uv);

				if (i.uv.x < _TopUV.x)
				{
					if (i.uv.x > _UnderUV.x)
					{
						col = fixed4(0, 0, 0, 1);
						return col;
					}
				}
				return col;
            }
            ENDCG
        }
    }
}
