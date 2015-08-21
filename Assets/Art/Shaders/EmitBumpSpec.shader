Shader "EmitBumpSpec"
{
	Properties {
		_MainColor("_MainColor", Color) = (0.7573529,0,0,1)
		_EmitColor("_EmitColor", Color) = (0.8676471,0.03189877,0.03189877,1)
		_Diff("_Diff", 2D) = "black" {}
		_Spec("_Spec", 2D) = "black" {}
		_Normal("_Normal", 2D) = "bump" {}
	}
	
	SubShader {
		Tags {
			"Queue"="Geometry"
			"IgnoreProjector"="False"
			"RenderType"="Opaque"
		}

		
		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGBA
		Fog{
		
		}


		CGPROGRAM
		#pragma surface surf BlinnPhongEditor vertex:vert
		#pragma target 2.0


		float4 _MainColor;
		float4 _EmitColor;
		sampler2D _Diff;
		sampler2D _Spec;
		sampler2D _Normal;

		struct EditorSurfaceOutput {
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half3 Gloss;
			half Specular;
			half Alpha;
			half4 Custom;
		};
			
		inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light) {
			half3 spec = light.a * s.Gloss;
			half4 c;
			c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
			c.a = s.Alpha;
			return c;
		}

		inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize (lightDir + viewDir);
			
			half diff = max (0, dot ( lightDir, s.Normal ));
			
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0);
			
			half4 res;
			res.rgb = _LightColor0.rgb * diff;
			res.w = spec * Luminance (_LightColor0.rgb);
			res *= atten * 2.0;

			return LightingBlinnPhongEditor_PrePass( s, res );
		}
		
		struct Input {
			float2 uv_Diff;
			float2 uv_Normal;
			float2 uv_Spec;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o)
			float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
			float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
			float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
			float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
		}
			

		void surf (Input IN, inout EditorSurfaceOutput o) {
			o.Normal = float3(0.0,0.0,1.0);
			o.Alpha = 1.0;
			o.Albedo = 0.0;
			o.Emission = 0.0;
			o.Gloss = 0.0;
			o.Specular = 0.0;
			o.Custom = 0.0;
				
			float4 Tex2D1=tex2D(_Diff,(IN.uv_Diff.xyxy).xy);
			float4 Multiply0=_MainColor * Tex2D1;
			float4 Tex2D0=tex2D(_Normal,(IN.uv_Normal.xyxy).xy);
			float4 UnpackNormal0=float4(UnpackNormal(Tex2D0).xyz, 1.0);
			float4 Multiply1=Tex2D1.aaaa * _EmitColor;
			float4 Tex2D2=tex2D(_Spec,(IN.uv_Spec.xyxy).xy);
			float4 Master0_3_NoInput = float4(0,0,0,0);
			float4 Master0_5_NoInput = float4(1,1,1,1);
			float4 Master0_7_NoInput = float4(0,0,0,0);
			float4 Master0_6_NoInput = float4(1,1,1,1);
			o.Albedo = Multiply0;
			o.Normal = UnpackNormal0;
			o.Emission = Multiply1;
			o.Gloss = Tex2D2;

			o.Normal = normalize(o.Normal);
		}
		ENDCG
	}
	Fallback "Diffuse"
}
