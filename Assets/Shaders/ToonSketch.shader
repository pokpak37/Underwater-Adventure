Shader "Custom/ToonSketch" {
	Properties {
		//_Color ("Main Color", Color) = (.5,.5,.5,1)
		//_Outline ("Outline width", float) = .005W
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_SketchTex01 ("Sketch", 2D) = "white" { }
		_SketchTex02 ("Sketch", 2D) = "white" { }
		//_RimPower ("Rim Power", float) = 3.0
		_BrokenTex ("Broken Texture", 2D) = "black" { }
		_Broken ("Broken", Range(0,1.2)) = 0
		_Intensity("Intensity",Range(-1,1)) = 0
		//_Angle ("Rotate", float) = 0
		//_Scale ("Scale", float) = 0.01
		//_Shadow ("Shadow", float) = 0
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		struct a2v
		{
			fixed4 vertex : POSITION;
			fixed3 normal : NORMAL;
			fixed4 texcoord : TEXCOORD0;
		}; 
		struct v2f
		{
			fixed4 pos : POSITION;
			fixed4 tex : TEXCOORD0;
			fixed4 color : COLOR;
		};
		sampler2D _MainTex;
		sampler2D _BrokenTex;
		fixed _Broken;
		
		
		v2f vert (a2v v)
		{			
				const fixed _Outline  = 0.003;
				const fixed3 _Color =fixed3(0.6980,0.6039,0.5529);
				
				v2f o;
				o.tex = v.texcoord;
				fixed4 pos = mul( UNITY_MATRIX_MV, v.vertex);
				fixed3 normal = mul( (fixed3x3)UNITY_MATRIX_IT_MV, v.normal);
				normal.z =0;
				fixed dist = distance(_WorldSpaceCameraPos, mul(_Object2World, v.vertex));
				
				pos = pos + fixed4(normalize(normal),0) * (_Outline+(_Broken*0.05)) * dist;
				o.pos = mul(UNITY_MATRIX_P, pos);
				
				dist = distance(_WorldSpaceCameraPos.z, mul(_Object2World, v.vertex).z);
				
				//if(dist > 17.5 )
				//	o.color = fixed4(lerp(_Color-0.6+0.2,_Color,saturate((dist-16)/16)),1);
				fixed3 colorLight = lerp(_Color-0.6,_Color.rgb,saturate((dist+20)/60));
				o.color= fixed4(lerp(_Color-0.6,colorLight,saturate(dist-10)),1);
				
				//o.color =fixed4(_Color.rgb-0.6,1);
				return o;
		}
	ENDCG
			
	SubShader {
	 	Tags {"IgnoreProjector"="True" "RenderType"="Opaque"}
		Name "BASE"
		Lighting Off
		
		CGPROGRAM
		#pragma surface surf Unlit
		fixed4 LightingUnlit (SurfaceOutput s, fixed3 lightDir, fixed atten) {			
        	fixed4 c;
        	c.rgb = s.Albedo;
        	c.a = s.Alpha;
        	return c;
		}
		
		struct Input {
			fixed4 vertex : POSITION;
			fixed2 uv_MainTex : TEXCOORD0;
			fixed4 screenPos;
			fixed3 viewDir;
		};

        sampler2D _SketchTex01;
        sampler2D _SketchTex02;
        fixed _Intensity;
		
		void surf (Input IN, inout SurfaceOutput o) {
			//_Color =  //_Color =_Color = fixed3(178/255f,154/255f,141/255f);
			fixed textureBroken = tex2D (_BrokenTex, IN.uv_MainTex).r;	
			fixed3 textureColor = tex2D (_MainTex, IN.uv_MainTex).rgb;
		
			if (_Broken>=textureBroken+0.1|| distance(textureColor,fixed3(0,1,0))< 0.5)
				discard;
				
			const fixed3 _Color =fixed3(0.6980,0.6039,0.5529);
        	const fixed _Shadow = 0.09;
        	const fixed _RimPower = 1.4;
			const fixed _Angle =45;
			const fixed _Scale = 0.0165;
			
			fixed2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= fixed2(_ScreenParams.x/_ScreenParams.y*_Scale*1000 ,_Scale*1000);
			
			fixed sinA = sin(_Angle*3.14/180);
			fixed cosA = cos(_Angle*3.14/180);
        	screenUV= mul ( screenUV.xy,fixed2x2( cosA,-sinA,sinA,cosA));
        	
			fixed rim =  saturate(dot (normalize(IN.viewDir), o.Normal));
			fixed rimOutput = pow (rim, _RimPower);
			
			fixed ramp = rimOutput*textureColor;
			
			if (textureColor.r < _Shadow)
				o.Albedo = _Color.rgb-0.6;
			else
				o.Albedo = saturate((lerp(tex2D (_SketchTex02, screenUV).rgb , tex2D (_SketchTex01, screenUV).rgb+0.3,ramp))+(_Color.rgb-0.6)) *(_Color.rgb);					
			//o.Albedo = fixed3(1,1,1)+(_Color.rgb*0.25);
			//o.Albedo *= lerp(tex2D (_SketchTex02,screenUV).rgb,tex2D (_SketchTex01,screenUV).rgb*2+0.5,ramp) *(_Color.rgb*1.5);
			if(_Intensity >= 0)
			o.Albedo = lerp(o.Albedo,_Color.rgb,_Intensity);
			else
			o.Albedo = lerp(_Color.rgb-0.6,o.Albedo,_Intensity+1);
			
			fixed dist = distance(_WorldSpaceCameraPos.z, IN.screenPos.z);
			
			fixed3 colorLight = lerp(o.Albedo,_Color.rgb,saturate((dist+90)/170));
			o.Albedo= lerp(o.Albedo,colorLight,saturate(dist-16.5));
			o.Albedo= lerp(_Color.rgb-0.6,o.Albedo,saturate((dist-11)/4));
		}
        ENDCG
			//SetTexture [_MainTex] {
			//	constantColor [_Color]
			//	Combine texture * constant
			//}
		Pass
		{		
			Cull Front			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
					
			fixed4 frag (v2f IN) : COLOR
			{
				fixed textureBroken = tex2D (_BrokenTex,IN.tex.xy).r;	
				fixed3 textureColor = tex2D(_MainTex, IN.tex.xy).rgb; 
            	if (_Broken*2>=textureBroken+0.1 || distance(textureColor,fixed3(0,1,0))< 0.5)
					discard;
				return IN.color;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
	
}
