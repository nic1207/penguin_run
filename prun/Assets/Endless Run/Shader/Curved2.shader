
Shader "Custom/Curved2" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Bump ("Bump", 2D) = "bump" {}
		//_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
     	//_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_QOffset ("Offset", Vector) = (0,0,0,0)
		_Dist ("Distance", Float) = 100.0
		_Snow ("Snow Level", Range(0,1) ) = 0
    	_SnowTex ("Snow Texture", 2D) = "white" {}
    	_SnowDirection ("Snow Direction", Vector) = (0,1,0)
    	//_SnowDepth ("Snow Depth", Range(0,0.3)) = 0.1
    	_Curvature ("Curvature", Float) = 0.001
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150
		//Blend SrcAlpha OneMinusSrcAlpha 
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert
		//#pragma addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		//#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Bump;
		fixed4 _Color;
		float4 _QOffset;
		float _Dist;
		float _Snow;
		sampler2D _SnowTex;
		float4 _SnowDirection;
		uniform float _Curvature;
		//float _SnowDepth;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Bump;
			float2 uv_SnowTex;
			float3 worldNormal; INTERNAL_DATA
			//float3 viewDir;
			//float4 pos : SV_POSITION;
		};
		struct v2f
         {
             float4 pos : SV_POSITION;
             float4 uv : TEXCOORD0;
         };

		// This is where the curvature is applied
        void vert( inout appdata_full v)
        {
        	
        	//v2f o;
        	//if(dot(v.normal, _SnowDirection.xyz) <= lerp(-1,1, _Snow)) {
			//	v.vertex.xyz += (_SnowDirection.xyz + v.normal) * _SnowDepth * _Snow;
			//}
			float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
			float zOff = vPos.z / _Dist;
			vPos += _QOffset * zOff * zOff;
			v.vertex = mul (vPos, UNITY_MATRIX_IT_MV);
			v.texcoord = mul(UNITY_MATRIX_TEXTURE0, v.texcoord );
			//o.pos = mul (UNITY_MATRIX_P, vPos);
 			//o.uv = mul(UNITY_MATRIX_TEXTURE0, v.texcoord );

 			//float4 vv = mul( _Object2World, v.vertex );
 			//vv.xyz -= _WorldSpaceCameraPos.xyz;
 			//vv = float4( 0.0f, (vv.z * vv.z) * - _Curvature, 0.0f, 0.0f );
  
            // Now apply the offset back to the vertices in model space
            //v.vertex += mul(_World2Object, vv);
		}

		void surf (Input IN, inout SurfaceOutput o) {

			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 s = tex2D (_SnowTex, IN.uv_SnowTex);
    		o.Normal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
    		if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) > lerp(1,-1,_Snow)) {
        		o.Albedo = s.rgb * _Color.rgb;
    		} else {
        		o.Albedo = c.rgb * _Color.rgb;
    		}
    		o.Alpha = c.a;
    		o.Gloss = c.a;
    		o.Specular = 0;
		}
		inline float4 LightingCustomDiffuse (SurfaceOutput s, fixed3 lightDir, fixed atten) {
    		float difLight = dot (s.Normal, lightDir);
    		float hLambert = difLight * 0.5 + 0.5;
    		float4 col;
    		col.rgb = s.Albedo * _LightColor0.rgb * (hLambert * atten * 2);
    		col.a = s.Alpha;
    		return col;
		}

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
     	{
			fixed4 c;
         	c.rgb = s.Albedo*0.5f;
         	return c;
     	}
		ENDCG
	}
	//Fallback "Mobile/VertexLit"
}
