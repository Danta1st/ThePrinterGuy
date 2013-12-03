Shader "Custom/ProgressBar" {

Properties {
	_Color ("Color", Color) = (1,1,1,1)
	_BackgroundTex ("Background (RGBA)", 2D) = "white" {}
	_ForegroundTex ("Foreground (RGBA)", 2D) = "white" {}
	_Progress ("Progress", Range(0.0,1.0)) = 1.0
}

SubShader {
		Tags { "Queue"="Transparent" }
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
        Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

uniform sampler2D _BackgroundTex;
uniform sampler2D _ForegroundTex;

uniform float4 _Color;
uniform float _Progress;

struct v2f {
	float4 pos : POSITION;
	float2 uv : TEXCOORD0;
};

v2f vert (appdata_base v)
{
    v2f o;
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    
    o.uv = TRANSFORM_UV(0);
		
    return o;
}


half4 frag( v2f i ) : COLOR
{
	half4 b = tex2D( _BackgroundTex, i.uv);
	half4 f = tex2D( _ForegroundTex, i.uv);
	
	half4 color = b;
	if( i.uv.x < _Progress ){
		color = (1.0 - f.a)*color + f.a*f;
	}
	
    return color*_Color;
}



ENDCG

    }
}

}