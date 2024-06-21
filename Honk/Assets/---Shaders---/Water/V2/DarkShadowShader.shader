Shader"Custom/DarkerShadowsURPShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
        _ShadowIntensity ("Shadow Intensity", Range(0, 2)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
LOD200

        Pass
        {
Name"ShadowCaster"
            Tags
{"LightMode" = "ShadowCaster"
}

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
};

struct Varyings
{
    float4 positionHCS : SV_POSITION;
    float3 normalWS : TEXCOORD0;
    float4 shadowCoord : TEXCOORD1;
};

            CBUFFER_START(UnityPerMaterial)
float4 _BaseColor;
half _Metallic;
half _Smoothness;
float _ShadowIntensity;
CBUFFER_END

            VaryingsVert(
Attributes IN)
            {
Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.shadowCoord = TransformWorldToShadowCoord(OUT.positionHCS);
                return
OUT;
            }

half4 Frag(Varyings IN) : SV_Target
{
    half3 normal = normalize(IN.normalWS);
    Light mainLight = GetMainLight();
    half3 lightDir = normalize(mainLight.direction);
    half atten = mainLight.distanceAttenuation * mainLight.shadowAttenuation;

                // Modifier l'atténuation des ombres pour les rendre plus sombres
    atten *= _ShadowIntensity;

    half3 color = _BaseColor.rgb * atten;
    return half4(color, _BaseColor.a);
}
            ENDHLSL
        }

        Pass
        {
Name"ForwardLit"
            Tags
{"LightMode" = "UniversalForward"
}

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadow.hlsl"

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
};

struct Varyings
{
    float4 positionHCS : SV_POSITION;
    float3 normalWS : TEXCOORD0;
    float4 shadowCoord : TEXCOORD1;
};

            CBUFFER_START(UnityPerMaterial)
float4 _BaseColor;
half _Metallic;
half _Smoothness;
float _ShadowIntensity;
CBUFFER_END

            VaryingsVert(
Attributes IN)
            {
Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.shadowCoord = TransformWorldToShadowCoord(OUT.positionHCS);
                return
OUT;
            }

half4 Frag(Varyings IN) : SV_Target
{
    half3 normal = normalize(IN.normalWS);
    Light mainLight = GetMainLight();
    half3 lightDir = normalize(mainLight.direction);
    half atten = mainLight.distanceAttenuation * mainLight.shadowAttenuation;

                // Modifier l'atténuation des ombres pour les rendre plus sombres
    atten *= _ShadowIntensity;

    half3 albedo = _BaseColor.rgb;
    half3 diffuse = albedo * _LightColor0.rgb * max(0, dot(normal, lightDir)) * atten;

                // Calculer la specularité en utilisant le modèle standard de Cook-Torrance
    half3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionHCS.xyz);
    half3 halfDir = normalize(lightDir + viewDir);
    half NdotH = max(0, dot(normal, halfDir));
    half NdotV = max(0, dot(normal, viewDir));
    half VdotH = max(0, dot(viewDir, halfDir));
    half m = _Smoothness * _Smoothness;

                // Distribution fonction D
    half D = m * m / (PI * pow(NdotH * NdotH * (m * m - 1) + 1, 2));

                // Fresnel fonction F
    half F = pow(1 - VdotH, 5) * (1 - _Metallic) + _Metallic;

                // Geometry fonction G
    half k = m / 2;
    half G_SchlickSmithGGX = NdotV * NdotH / (NdotV * (1 - k) + k);

    half3 specular = D * F * G_SchlickSmithGGX / (4 * NdotV * max(0.001, dot(normal, lightDir)));

    half3 color = diffuse + specular;
    return half4(color, _BaseColor.a);
}
            ENDHLSL
        }
    }
FallBack"Universal Render Pipeline/Lit"
}
