#ifndef HLSLCOMPUTELIGHT_INCLUDED
#define HLSLCOMPUTELIGHT_INCLUDED

void Compute_float(float3 worldPosition, out float3 lightDirection, out float3 lightColor)
{
    #ifdef SHADERGRAPH_PREVIEW
    lightDirection = normalize(float3(-0.5,0.5,-0.5));
    lightColor = float3(1,1,1);
    #else
    
    Light mainLight = GetMainLight(TransformWorldToShadowCoord(worldPosition), worldPosition, unity_ProbesOcclusion);
    lightDirection = mainLight.direction;
    lightColor = mainLight.color;
    
    #endif
}

void Compute_float(float3 worldPosition, out half3 lightDirection, out half3 lightColor, out half lightDistanceAttenuation, out half lightShadowAttenuation)
{
   #ifdef SHADERGRAPH_PREVIEW
   lightDirection = half3(0.5, 0.5, 0);
   lightColor = 1;
   lightDistanceAttenuation = 1;
   lightShadowAttenuation = 1;
   #else
   
   half4 shadowCoord = TransformWorldToShadowCoord(worldPosition);
   Light mainLight = GetMainLight(shadowCoord);
   lightDirection = mainLight.direction;
   lightColor = mainLight.color;
   lightDistanceAttenuation = mainLight.distanceAttenuation;
   lightShadowAttenuation = mainLight.shadowAttenuation;
   
   #endif
}

#endif