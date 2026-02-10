Shader "Unlit/GlassToonShader"
{
  
  Properties{
    _mainColor ("Color", Color) = (1,1,1)  
    _rimColor ("Rim light color", Color) = (1,1,1)
    _rimFac ("Rim factor", float) = 0.5
    _scatterFac ("Rim sharpness", float) = 5
    _emissionFac ("Emission strength", float) = 10
  }

  SubShader{
  Tags {"RenderPipeline" = "UniversalPipeline" }
  Pass {
      Name "ForwardLit"
      Tags {"LightMode" = "UniversalForward"}
      HLSLPROGRAM

      #pragma vertex Vertex
      #pragma fragment Fragment
      #pragma multi_compile _ _FORWARD_PLUS
      //shader code goes here

      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
      CBUFFER_START(UnityPerMaterial) 

      //Material Properties
      float3 _mainColor;

      bool _rimLight;
      float _rimFac;
      float _scatterFac;
      float3 _rimColor;

      float _emissionFac;

      CBUFFER_END


      struct VertexInput { 
        float3 positionOS : POSITION0;
        float3 normalOS   : NORMAL;
      };

      struct VertexOutput { 
        float4 positionCS  : SV_POSITION;
        float rimMask : TEXCOORD1; 
      };

      VertexOutput Vertex(VertexInput input) { 
        VertexOutput output;

        VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
        output.positionCS = posInputs.positionCS;
        
        float3 normalWS = normalize(TransformObjectToWorldNormal(input.normalOS));
       

        float3 cameraDirection = normalize(_WorldSpaceCameraPos - posInputs.positionWS);
        output.rimMask = pow(1 - saturate(dot(cameraDirection, normalWS)), _scatterFac) * _rimFac;
      
        return output;
      } 

      float3 Fragment(VertexOutput input) : SV_TARGET{

        return _mainColor * _emissionFac + input.rimMask * _rimColor; 
      }

      ENDHLSL
    }
  }

  Fallback "Diffuse" //used in case custom shader fails
}