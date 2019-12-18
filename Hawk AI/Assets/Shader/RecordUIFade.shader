Shader "Hidden/RecordUIFade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MaskTexture("MaskTexture", 2D) = "white" {}
		_MaskTexUV("MaskTexUV",Vector) = (0,0,0,0)

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
			sampler2D _MaskTexture;
			float2 _MaskTexUV;

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col;

				col = tex2D(_MainTex, i.uv);

				if(_MaskTexUV.x >= i.uv.x)
				col = tex2D(_MaskTexture, i.uv);
				
                return col;
            }
            ENDCG
        }
    }
}
