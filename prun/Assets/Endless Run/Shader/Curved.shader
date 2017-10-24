Shader "Custom/Curved" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_QOffset ("Offset", Vector) = (0,0,0,0)
		_Dist ("Distance", Float) = 100.0
	}
	
	SubShader {
		Tags {"LightMode" = "ForwardBase" }
		Pass
		{
	        Blend SrcAlpha OneMinusSrcAlpha 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _Color;
			uniform float4 _LightColor0;
            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST; //_ST for tiling
            float4 _BumpMap_ST;
			float4 _QOffset;
			float _Dist;
			//float _Brightness;
			
			struct v2f {
			    float4 pos : SV_POSITION;
			    float4 uv : TEXCOORD0;
			    //float3 viewDir : TEXCOORD1;
			    //fixed3 color : COLOR0;
			};

			 struct vertexInput
             {
                 float4 vertex : POSITION;
                 float3 normal : NORMAL;
                 float4 texcoord : TEXCOORD0;
                 float4 tangent : TANGENT;
  
             }; 
  
             struct vertexOutput
             {
                 float4 pos : POSITION;
                 float4 tex : TEXCOORD0;
                 float4 posWorld : TEXCOORD2;
                 float3 lightDirection : TEXCOORD3;
  
             };

			vertexOutput vert (vertexInput v)
			{
			   //v2f o;
			   //UNITY_INITIALIZE_OUTPUT(v2f, o);
			   //float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
			   //float zOff = vPos.z/_Dist;
			   //vPos += _QOffset * zOff * zOff;
			   //o.pos = mul (UNITY_MATRIX_P, vPos);
			   //o.uv = v.texcoord;
			   //return o;
			   vertexOutput o;
			   UNITY_INITIALIZE_OUTPUT(vertexOutput, o);
			   float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
			   float zOff = vPos.z/_Dist;
			   vPos += _QOffset * zOff * zOff;
			   o.pos = mul (UNITY_MATRIX_P, vPos);
               //o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
               o.tex = v.texcoord;
               //o.color = v.normal * 0.5 + 0.5;
               return o;
			}

			half4 frag (vertexOutput i) : COLOR
			{
				// sample the normal map, and decode from the Unity encoding
                //half3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
                // transform normal from tangent to world space
                //half3 worldNormal;
                //worldNormal.x = dot(i.tspace0, tnormal);
                //worldNormal.y = dot(i.tspace1, tnormal);
                //worldNormal.z = dot(i.tspace2, tnormal);
                float4 c = tex2D (_MainTex,  _MainTex_ST.xy * i.tex.xy + _MainTex_ST.zw);  
                 
                 float4 n =  tex2D (_NormalMap, _BumpMap_ST.xy * i.tex.xy + _BumpMap_ST.zw); 
                 fixed3 unpackedNormal;
                 unpackedNormal.xy = n.wy * 2 - 1;
                 unpackedNormal.z = sqrt(1 - unpackedNormal.x*unpackedNormal.x - unpackedNormal.y * unpackedNormal.y);
  
                 float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;
  
                 float diff = saturate (dot (unpackedNormal, normalize(i.lightDirection)));   
                 lightColor += _LightColor0.rgb * (diff); 
                 c.rgb = c.rgb * lightColor * _Color * 1.5f;
                 return c;

                //float4 normalMap = tex2D(_BumpTex, i.uv);
			  	//half4 col = tex2D(_MainTex, i.uv.xy) * _Color;
			  	//return fixed4 (i.color, 1);
				//return col;
				//float3 lightDir = normalize( float3(_WorldSpaceLightPos0 ) );
                //half3  Albedo = tex2D(_MainTex, i.uv.xy ).rgb;
                //half3 pNormal = UnpackNormal( tex2D(_NormalMap, i.uv.xy ) );
                //half pxlAtten = dot( half3(pNormal.rgb), lightDir );
                //half3 diff = Albedo * pxlAtten;
                //return half4( diff, 1 );
			}
			ENDCG
		}
	}
	
	FallBack "Diffuse"
}
