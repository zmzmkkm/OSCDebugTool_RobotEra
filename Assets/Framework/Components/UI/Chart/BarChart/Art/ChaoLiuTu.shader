// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader/ChaoLiuTu"
{
	Properties
	{
		[HDR]_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		_EmissionTex("EmissionTex", 2D) = "white" {}
		[Toggle]_Keyword0("明\暗", Float) = 1
		_Num("段数", Float) = 2.61
		_Speed("速度", Range( 0 , 10)) = 0
		[Toggle(_KEYWORD1_ON)] _Keyword1("是否启用渐隐", Float) = 0
		_LineFade("LineFade", Float) = 25
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 2
		[Enum(Off,0,On,1)]_ZWriteMode("ZWriteMode", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)]_ZTestMode("ZTestMode", Float) = 4
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_CullMode]
		ZWrite [_ZWriteMode]
		ZTest [_ZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha , One One
		BlendOp Add
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _KEYWORD1_ON
		#pragma exclude_renderers xboxone xboxseries playstation ps4 switch 
		#pragma surface surf Standard keepalpha noshadow nolightmap  nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float _CullMode;
		uniform float _ZWriteMode;
		uniform float _ZTestMode;
		uniform float4 _EmissionColor;
		uniform sampler2D _EmissionTex;
		uniform float _Speed;
		uniform float _Num;
		uniform float _Keyword0;
		uniform float _LineFade;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult494 = (float2(( _Speed * -1.0 ) , 0.0));
			float2 appendResult432 = (float2(_Num , 1.0));
			float2 uv_TexCoord483 = i.uv_texcoord * appendResult432;
			float2 panner425 = ( 1.0 * _Time.y * appendResult494 + uv_TexCoord483);
			float4 tex2DNode14 = tex2D( _EmissionTex, panner425 );
			o.Emission = ( ( _EmissionColor * tex2DNode14 ) * (( _Keyword0 )?( 1.0 ):( 0.5 )) ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 temp_output_5_0_g10 = ( ( ase_worldPos - float3( 0,0,0 ) ) / ( _LineFade * 100.0 ) );
			float dotResult8_g10 = dot( temp_output_5_0_g10 , temp_output_5_0_g10 );
			float smoothstepResult576 = smoothstep( 0.0 , 1.5 , 1.0);
			#ifdef _KEYWORD1_ON
				float staticSwitch581 = ( 1.0 - pow( saturate( dotResult8_g10 ) , smoothstepResult576 ) );
			#else
				float staticSwitch581 = 1.0;
			#endif
			o.Alpha = ( tex2DNode14.a * staticSwitch581 );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;509;835.5467,-178.0403;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;579;500.0331,-517.2546;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;425;-39.33556,-391.3362;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;568;-8.216282,69.35546;Inherit;True;SphereMask;-1;;10;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;0;False;12;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;157.5468,-420.0403;Inherit;True;Property;_EmissionTex;EmissionTex;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;483;-290.0913,-526.3835;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;432;-498.0913,-504.3831;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;431;-665.091,-577.3841;Inherit;False;Property;_Num;段数;4;0;Create;False;0;0;0;False;0;False;2.61;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;430;-661.0911,-434.3831;Inherit;False;Constant;_V_Speed;V_Speed;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;494;-206.6018,-209.6053;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;495;-400.6017,-125.6053;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;489;-667.6027,-309.6051;Inherit;False;Property;_Speed;速度;5;0;Create;False;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;591;-533.9366,-221.2862;Inherit;False;Constant;_Float5;Float 0;3;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;590;-340.9362,-273.286;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;578;238.0331,-657.2546;Inherit;False;Property;_EmissionColor;EmissionColor;0;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;566;-396.3964,2.582359;Inherit;False;Property;_LineFade;LineFade;7;0;Create;True;0;0;0;False;0;False;25;25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;574;-398.7859,75.5535;Inherit;False;Constant;_Multiple;Multiple;10;0;Create;True;0;0;0;False;0;False;100;100;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;573;-196.5621,18.27116;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;576;-211.5035,211.9887;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;570;-356.6526,206.5152;Inherit;False;Constant;_Float4;Float 4;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;507;315.5467,-111.0403;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;592;505.6439,-171.7237;Inherit;False;Property;_Keyword0;明\暗;3;0;Create;False;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1579.506,-231.0519;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Shader/ChaoLiuTu;False;False;False;False;False;False;True;False;True;False;False;False;False;False;True;False;False;False;False;False;False;Back;1;True;_ZWriteMode;3;True;_ZTestMode;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;7;d3d11;glcore;gles;gles3;metal;vulkan;ps5;True;True;True;True;0;False;;False;2;False;_Ref;2;False;_ReadMask;255;False;_ReadMask;2;False;;3;False;;3;False;;3;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;_BlendMode_Src;10;False;_BlendMode_Dst;4;1;False;;1;False;;1;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;2;-1;-1;-1;0;False;0;0;True;_CullMode;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.OneMinusNode;571;320.9028,71.81198;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;583;327.3223,-7.745422;Inherit;False;Constant;_Float3;Float 3;17;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;581;494.7102,19.52508;Inherit;True;Property;_Keyword1;是否启用渐隐;6;0;Create;False;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;512;306.5467,-190.0403;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;0.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;577;840.1624,-42.66949;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;593;1929.582,-149.6175;Inherit;False;Property;_CullMode;CullMode;8;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;594;1913.481,-50.38419;Inherit;False;Property;_ZWriteMode;ZWriteMode;9;1;[Enum];Create;True;0;2;Off;0;On;1;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;595;1913.17,50.83672;Inherit;False;Property;_ZTestMode;ZTestMode;10;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CompareFunction;True;0;False;4;4;0;0;0;1;FLOAT;0
WireConnection;509;0;579;0
WireConnection;509;1;592;0
WireConnection;579;0;578;0
WireConnection;579;1;14;0
WireConnection;425;0;483;0
WireConnection;425;2;494;0
WireConnection;568;14;573;0
WireConnection;568;12;576;0
WireConnection;14;1;425;0
WireConnection;483;0;432;0
WireConnection;432;0;431;0
WireConnection;432;1;430;0
WireConnection;494;0;590;0
WireConnection;494;1;495;0
WireConnection;590;0;489;0
WireConnection;590;1;591;0
WireConnection;573;0;566;0
WireConnection;573;1;574;0
WireConnection;576;0;570;0
WireConnection;592;0;512;0
WireConnection;592;1;507;0
WireConnection;0;2;509;0
WireConnection;0;9;577;0
WireConnection;571;0;568;0
WireConnection;581;1;583;0
WireConnection;581;0;571;0
WireConnection;577;0;14;4
WireConnection;577;1;581;0
ASEEND*/
//CHKSM=3D10967BA532594EF55B8F1F5DF799A131130016