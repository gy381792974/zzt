 
Shader "UI/ImageGreyShader"
{
    Properties
    {
        [PerRendererData] 
		//_MainTex("Sprite Texture", 2D) = "white" {}
		 _MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
		_IsGray("IsGray",Int)=1
		//MASK SUPPORT ADD
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
		//MASK SUPPORT END
 
    }
 
        SubShader
    {
        Tags
     {
        "Queue" = "Transparent"
        "IgnoreProjector" = "True"
        "RenderType" = "Transparent"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "True"
	 }
 
 
		//MASK SUPPORT ADD
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
		ColorMask [_ColorMask]
		//MASK SUPPORT END
		ZWrite Off
		ZTest[unity_GUIZTestMode]
	 // 源rgba*源a + 背景rgba*(1-源A值)   
        Blend SrcAlpha OneMinusSrcAlpha
       
 
        Pass
    {
        CGPROGRAM
		#pragma vertex vert     
		#pragma fragment frag     
		#include "UnityCG.cginc"     
 
        struct appdata_t
    {
        float4 vertex   : POSITION;
        float4 color    : COLOR;
        float2 texcoord : TEXCOORD0;
    };
 
    struct v2f
    {
        float4 vertex   : SV_POSITION;
        fixed4 color : COLOR;
        half2 texcoord  : TEXCOORD0;
    };
 
    sampler2D _MainTex;
    fixed4 _Color;
	int _IsGray;
    v2f vert(appdata_t IN)
    {
        v2f OUT;
        OUT.vertex = UnityObjectToClipPos(IN.vertex);
        OUT.texcoord = IN.texcoord;
#ifdef UNITY_HALF_TEXEL_OFFSET     
        OUT.vertex.xy -= (_ScreenParams.zw - 1.0);
#endif     
        OUT.color = IN.color * _Color;
        return OUT;
    }
 
    fixed4 frag(v2f IN) : SV_Target
    {
        half4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
		if(_IsGray>0)
		{
			return color;
		}
		else
		{
//#ifdef UNITY_UI_CLIP_RECT
//			color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
//#endif
			float grey = dot(color.rgb, fixed3(0.22, 0.707, 0.071));

			return half4(grey*_Color.r,grey*_Color.g,grey*_Color.b,color.a);
		}
       
    } 

 //  //处理Mask2D
 //  inline float UnityGet2DClipping(in float2 position, in float4 clipRect)
	//{
	//	// 判断当前点是否在矩形中，返回inside.x * inside.y 如果有任意一点不在那么返回值为0
	//	float2 inside = step(clipRect.xy, position.xy) * step(position.xy, clipRect.zw);
	//	return inside.x * inside.y;
	//}
        ENDCG
    }
    }
}
 
 
