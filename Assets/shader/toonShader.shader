Shader "Unlit/ToonShader"
{
  
  Properties{
    _shadowTreshold ("Shadow treshold", float) = 0.5  
    _shadowFac ("Shadow strength", float) = 4
    _mainColor ("Color", Color) = (1,1,1)
    [Toggle] _rimLight ("Rim light", float) = 0
    _rimTreshold ("Rim light treshold", float) = 0.5  
    _rimColor ("Rim light color", Color) = (1,1,1)
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
      float _shadowTreshold;
      float _shadowFac;
      float3 _mainColor;

      bool _rimLight;
      float _rimTreshold;
      float3 _rimColor;

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
        float3 normalWS : TEXCOORD1; 
        float3 cameraDirection : TEXCOORD2;
        
      };

      VertexOutput Vertex(VertexInput input) { 
        VertexOutput output;

        VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS);
        output.positionCS = posInputs.positionCS;
        
        output.normalWS = normalize(TransformObjectToWorldNormal(input.normalOS));
        
        output.uv = input.uv;

        output.cameraDirection = normalize(_WorldSpaceCameraPos - posInputs.positionWS);

        return output;
      } 

      float3 Fragment(VertexOutput input) : SV_TARGET{ 
        float2 uv = saturate(input.uv);   // clamps UV to 0–1

        float emissionMask = tex2D(_emissionMask, uv).r;

        if(emissionMask > 0.5){
           return _emissionColor * _emissionFac; 
        }

        Light mainLight = GetMainLight();
        float3 direction = normalize(mainLight.direction);  

        float shadowMask = (dot(input.normalWS, direction) + 1)/2;

        if(shadowMask <= _shadowTreshold){ 
           _mainColor /= _shadowFac; 
        }

        if(_rimLight){
            float rimMask = dot(input.normalWS, input.cameraDirection);

            if(rimMask <= _rimTreshold){
                _mainColor = _rimColor;
            }
        }

        return _mainColor * mainLight.color;
      }

      ENDHLSL
    }
  }

  Fallback "Diffuse" //used in case custom shader fails
}