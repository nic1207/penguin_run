
Shader "Custom/Curved3" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Bump ("Bump", 2D) = "bump" {}
		//_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
     	//_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_QOffset ("Offset", Vector) = (0,0,0,0)
		_Dist ("Distance", Float) = 100.0
		//_Snow ("Snow Level", Range(0,1) ) = 0
    	//_SnowColor ("Snow Color", Color) = (1.0,1.0,1.0,1.0)
    	//_SnowDirection ("Snow Direction", Vector) = (0,1,0)
    	//_SnowDepth ("Snow Depth", Range(0,0.3)) = 0.1
    	//_Curvature ("Curvature", Float) = 0.001
	}
	SubShader {
		Tags {
			//"Queue"="Transparent"
			//"Queue" = "Transparent"
			//"IgnoreProjector"="True"
			//"RenderType"="Transparent"
			"RenderType"="Opaque"
			//"Queue" = "AlphaTest" 
			//"RenderType" = "TransparentCutout"
		}

		//Pass {
        	//ZWrite On
        	//ColorMask 0
    	//}
		//LOD 150
		//Lighting On
		//ZTest Off
        //ZWrite On
        //Alphatest Greater 0
		//ZWrite Off
		//ColorMask RGB
		//ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha 
		//Cull Back
		//Blend SrcAlpha OneMinusSrcAlpha 
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf CustomDiffuse vertex:vert
		//#pragma surface surf BlinnPhongColor vertex:vert
		//#pragma addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;
		//sampler2D _Bump;
		fixed4 _Color;
		float4 _QOffset;
		float _Dist;
		//float _Snow;
		//float4 _SnowColor;
		//float4 _SnowDirection;
		//uniform float _Curvature;
		//float _SnowDepth;

		struct Input {
			float2 uv_MainTex;
			//float2 uv_Bump;
			float3 worldNormal; INTERNAL_DATA
			//float3 viewDir;
			//float4 pos : SV_POSITION;
		};

		struct SurfaceOutputSpecColor {
            half3 Albedo;
            half3 Normal;
            half3 Emission;
            half Specular;
            half3 GlossColor;
            half Alpha;
        };

		struct v2f
         {
             float4 pos : SV_POSITION;
             float4 uv : TEXCOORD0;
         };

		// This is where the curvature is applied
        void vert( inout appdata_full v)
        {
			float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
			float zOff = vPos.z / _Dist;
			vPos += _QOffset * zOff * zOff;
			v.vertex = mul (vPos, UNITY_MATRIX_IT_MV);
			v.texcoord = mul(UNITY_MATRIX_TEXTURE0, v.texcoord );
		}

		//void surf (Input IN, inout SurfaceOutput o) {
		//	half4 c = tex2D (_MainTex, IN.uv_MainTex);
    	//	o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
       // 	o.Albedo = c.rgb * _Color.rgb;
    //		o.Alpha = 1;//c.a;
   // 		o.Gloss = c.a;
   // 		o.Specular = c.a;
	//	}
		void surf (Input IN, inout SurfaceOutputSpecColor o) {
              half4 c = tex2D (_MainTex, IN.uv_MainTex);
    	//	o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
        	o.Albedo = c.rgb * _Color.rgb;
    		o.Alpha = c.a;
    		//o.Gloss = c.a;
   // 		o.Specular = c.a;
        }

		inline float4 LightingCustomDiffuse (SurfaceOutputSpecColor s, fixed3 lightDir, fixed atten) {
    		float difLight = dot (s.Normal, lightDir);
    		float hLambert = difLight * 0.5 + 0.5;
    		float4 col;
    		col.rgb = s.Albedo * _LightColor0.rgb * (hLambert * atten * 2);
    		col.a = 1;
    		//col.a = s.Alpha;
    		return col;
		}

		//forward lighting function
        inline half4 LightingBlinnPhongColor (SurfaceOutputSpecColor s, half3 lightDir, half3 viewDir, half atten) {
          half3 h = normalize (lightDir + viewDir);
 
          half diff = max (0, dot (s.Normal, lightDir));
 
          float nh = max (0, dot (s.Normal, h));
          float spec = pow (nh, 1);
          half3 specCol = spec * s.GlossColor;
 
          half4 c;
          c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * specCol * s.Specular) * (atten * 2);
          c.a = s.Alpha * dot(s.Normal.xyz,viewDir);
          return c;
        }

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
     	{
			fixed4 c;
         	c.rgb = s.Albedo*0.5f;
         	c.a = s.Alpha;
         	return c;
     	}
		ENDCG
	}
	//Fallback "Mobile/VertexLit"
}
