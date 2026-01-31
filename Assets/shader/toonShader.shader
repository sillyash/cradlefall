Shader "Unlit/ToonShader"
{
  
  Properties{
    _treshold ("Shadow treshold", float) = 0.5  
    _shadowFac ("Shadow strength", float) = 4
    _mainColor ("Color", Color) = (1,1,1) 
    _emissionColor ("Emission Color", Color) = (1,1,1)
    _emissionFac ("Emission strength", float) = 10
    _emissionMask ("Emission mask", 2D) = "black" {}
  }

  SubShader{
  Tags {"RenderPipeline" = "UniversalPipeline" }
  Pass {
      Name "ForwardLit"
      Tags {"LightMode" = "UniversalForward"}
      HLSLPROGRAM

      #pragma vertex Vertex
      #pragma fragment Fragment
      //shader code goes here

      #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
      CBUFFER_START(UnityPerMaterial) 

      //Material Properties
      float _treshold;
      float _shadowFac;
      float3 _mainColor;

      sampler2D _emissionMask;
      float _emissionFac;
      float3 _emissionColor;

      CBUFFER_END

      

      struct VertexInput { 
        float3 positionOS : POSITION0;
        float3 normalOS   : NORMAL;
        float2 uv         : TEXCOORD0;
      };

      struct VertexOutput { 
        float4 positionCS  : SV_POSITION;
        float2 uv         : TEXCOORD0;
        float3 normalWS : TEXCOORD1; //User defined channel
        
      };

      VertexOutput Vertex(VertexInput input) { 
        VertexOutput output;

        VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
        output.positionCS = posInputs.positionCS;
        output.normalWS = normalize(TransformObjectToWorldNormal(input.normalOS));
        output.uv = input.uv;

        return output;
      } 

      float3 Fragment(VertexOutput input) : SV_TARGET{ 
        float emissionMask = tex2D(_emissionMask, input.uv).r;

        if(emissionMask > 0.5){
           return _emissionColor * _emissionFac; 
        }

        Light mainLight = GetMainLight();
        float3 direction = normalize(-mainLight.direction);
        _mainColor = _mainColor * mainLight.color;

        float shadowMask = (dot(input.normalWS, direction) + 1)/2;

        if(shadowMask > _treshold){ 
           return _mainColor;
        }

        return _mainColor/_shadowFac; 
      }

      ENDHLSL
    }
  }

  Fallback "Diffuse" //used in case custom shader fails
}