Shader "Unlit/UIWalkLight"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LightTex("Light",2D) = "black"{}
		_Color("Color",color) = (0,0,0,1)
		_Intensity("Intensity",float) = 1
		_Speed("Speed",float) = 1
		_Angle("Angle",float) = 0
	}
		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}
			LOD 100

			Pass
			{
				Cull Off
				Lighting Off
				ZWrite Off
				Fog {Mode off}
				Offset -1,-1
				Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog

			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _LightTex;
			float4 _LightTex_ST;
			float4 _Color;
			float _Speed, _Angle, _Intensity;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				//float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				/*float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;*/
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;


			};

			v2f o;

			v2f vert(appdata v)
			{
				//v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : COlOR
			{
				// sample the texture
				fixed4 main = tex2D(_MainTex,i.texcoord);
				float2 newUV = i.texcoord.xy - float2(0.5, 0.5);
				newUV = float2(newUV.x * cos(_Angle) - newUV.y * sin(_Angle), newUV.y * cos(_Angle) + newUV.x * sin(_Angle));
				newUV += float2(0.5, 0.5);
				half lightU = newUV.x - frac(_Time.y * _Speed);
				half2 lightUV = half2(lightU, newUV.y) * _LightTex_ST.xy + _LightTex_ST.zw;
				fixed4 light = tex2D(_LightTex, lightUV) * _Intensity;
				fixed4 col = main + main * light.r * _Color;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return col * i.color;
				return col;
			}
			ENDCG
		}
		}
			SubShader
			{
				LOD 100

				Tags
				{
					"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
			}
				Pass
				{

					Cull Off
					Lighting Off
					ZWrite Off
					Fog {Mode off}
					Offset -1,-1
					ColorMask RGB
					Blend SrcAlpha OneMinusSrcAlpha
					ColorMaterial AmbientAndDiffuse

			SetTexture[_MainTex]
			{
				Combine Texture * Primary
			}

			}
			}

}
