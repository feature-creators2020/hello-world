Shader "Custom/SpotLightWall"
{
    Properties
    {
        _Color ("LightColor", Color) = (1,1,1,1)
		_LightPos("LightPos", Float) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
        //_Metallic ("Metallic", Range(0,1)) = 0.0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Pass 
		{
		CGPROGRAM
		#pragma vertex vert			//頂点シェーダー
		#pragma fragment frag		//フラグメントシェーダー

		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		//#pragma surface surf Standard alpha:fade 

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		//sampler2D _MainTex;

		//struct Input
		//{
		//	float2 uv_MainTex;
		//};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _LightPos;
		fixed4 _Color;
		#include "UnityCG.cginc"

		//頂点シェーダーの引数
		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 uv	  : TEXCOORD0;
		};

		//フラグメントシェーダーの引数
		struct v2f
		{
			float4 vertex : SV_POSITION;
			float4 vertexW: TEXCOORD0;
			float3 normal : TEXCOORD1;
			float2 uv	  : TEXCOORD2;
		};

		sampler2D _MainTex;
		//XXX:UnityCG.cgincで使われている
		float4 _MainTex_ST;
		float4 _Albedo;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_BUFFER_START(Props)
		//	// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

			//頂点シェーダー
			v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.vertexW = mul(unity_ObjectToWorld, v.vertex);
			o.normal = UnityObjectToWorldNormal(v.normal);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}

		//フラグメントシェーダー
		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col;
			//fixed4 tex = tex2D(_MainTex, i.uv);
			float3 E = normalize(_WorldSpaceCameraPos - i.vertexW.xyz);
#ifdef ONLY_DIRECTIONAL
				float3 L = normalize(_DirectionalLight.xyz);
#else
				float3 L = normalize(_LightPos.xyz);
#endif
				float3 N = normalize(i.normal);
				float3 H = normalize(L + normalize(E));
				float  S = pow(max(0.0f, dot(L, H)), 1.0f) * 0.075f;


				//出力カラーを計算
				_Albedo.rgb = 0;
				col = /*fixed4(nl, nl, nl, 1) * */_Albedo /** tex*/ + (_Color * S);
				return col;
		}
		//void surf(Input IN, inout SurfaceOutputStandard o)
		//{
		//	//if (unity_LightPosition[0].w == 0.0) { // `w`が0の場合はDirectional light.
		//	//	float3 lightPos = normalize(mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz);
		//	//}
		//	//else { // それ以外はSpot lightかPoint light.
		//	//	float3 lightPos = normalize(mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz - vertex.xyz);
		//	//}

		//	// Albedo comes from a texture tinted by color
		//	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		//	o.Albedo = _Color;
		//	//o.Albedo = fixed4(0.0, 1.0, 1.0, 0.0);

		//	// Metallic and smoothness come from slider variables
		//	o.Metallic = _Metallic;
		//	o.Smoothness = _Glossiness;
		//	o.Alpha = 0;

		//}
		ENDCG
		}

    }
    FallBack "Diffuse"
}
