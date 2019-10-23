Shader "Unlit/OutlineWithMaterialColor"
{
	Properties
	{//初期化
		//Inspector上の値が反映されなかった場合の値
		_OutlineSize("OutlineSize",Float) = 0.04
		_MainTex("Texture", 2D) = "white" {}
		_Albedo("Albedo",Color) = (1,1,1,1)
		_DirectionalLight("DirectionalLight",Float) = (1,1,1,1)
	}

		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			//1パス目でアウトライン描画
			Pass
			{
				Cull Front

				CGPROGRAM
				#pragma vertex vert			//頂点シェーダー
				#pragma fragment frag		//フラグメントシェーダー

				#include "UnityCG.cginc"

			//Inspector上で編集できるようにする値
			fixed4  _OutlineColor;		//アウトラインの色
			float	_OutlineSize;		//アウトラインサイズ
			float4 _Albedo;
			sampler2D _MainTex;
			float4 _MainTex_ST;

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
				float3 normal : TEXCOORD1;
				float2 uv	  : TEXCOORD2;
			};

			//頂点シェーダー
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex * _OutlineSize);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			//フラグメントシェーダー
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 tex = tex2D(_MainTex, i.uv);

				//NOTE: 色を変える場合、テクスチャの色から暗い色をブレンドして算出する
				fixed4 col = fixed4(0.5, 0.5, 0.5, 0.5) * _Albedo * tex;
				return col;
			}
			ENDCG
		}//Pass

		//2パス目でモデル本体の描画
		Pass
		{
			Cull Back

			CGPROGRAM
			#pragma vertex vert			//頂点シェーダー
			#pragma fragment frag		//フラグメントシェーダー
		
			#define ONLY_DIRECTIONAL	//Directional Lightのみの影響を受けさせる場合コメントアウトをはずす

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
#ifdef ONLY_DIRECTIONAL
			float4 _DirectionalLight;
#endif
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
				fixed4 tex = tex2D(_MainTex, i.uv);
				float3 E = normalize(_WorldSpaceCameraPos - i.vertexW.xyz);
#ifdef ONLY_DIRECTIONAL
				float3 L = normalize(_DirectionalLight.xyz);
#else
				float3 L = normalize(_WorldSpaceLightPos0.xyz);
#endif
				float3 N = normalize(i.normal);
				float3 H = normalize(L + normalize(E));
				float  S = pow(max(0.0f, dot(L, H)), 1.0f) * 0.075f;

				//ハーフランバート
				float d = saturate(dot(L, N));
				d = d * 0.5f + 0.5f;
				d = d * d;


				half nl = max(0, dot(N, L));
				if (nl <= 0.001f)
				{
					nl = d + 0.25f;
				}
				else if (nl <= 0.2f)
				{//薄影
					nl = d + 0.5f;
				}
				else
				{
					nl = 1.0f;
				}

				//出力カラーを計算
				col = fixed4(nl, nl, nl, 1) * _Albedo * tex + S;
				return col;
			}
			ENDCG
		}//Pass
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"

		}//SubShader
}
