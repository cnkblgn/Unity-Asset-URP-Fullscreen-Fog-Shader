#ifndef HLSLCOMPUTELIGHT_INCLUDED
#define HLSLCOMPUTELIGHT_INCLUDED

void GetLight_float(float3 worldPos, out float3 direction, out float3 color, out float shadow)
{
    #ifdef SHADERGRAPH_PREVIEW
    direction = normalize(float3(-0.5,0.5,-0.5));
    color = float3(1,1,1);
    shadow = 1;
    #else
    
    Light mainLight = GetMainLight(TransformWorldToShadowCoord(worldPos), worldPos, unity_ProbesOcclusion);
    shadow = mainLight.shadowAttenuation;
    direction = mainLight.direction;
    color = mainLight.color;
    
    #endif
}

void GetLight_half(half3 worldPos, out half3 direction, out half3 color, out half shadow)
{
    #ifdef SHADERGRAPH_PREVIEW
    direction = normalize(half3(-0.5,0.5,0.5));
    color = half3(1,1,1);
    shadow = 1;

    #else
    
    Light mainLight = GetMainLight(TransformWorldToShadowCoord(worldPos), worldPos, unity_ProbesOcclusion);
    shadow = mainLight.shadowAttenuation;
    direction = mainLight.direction;
    color = mainLight.color;
    
    #endif
}

void GetLightNoShadow_float(out float3 direction, float3 color)
{
    #ifdef SHADERGRAPH_PREVIEW
    direction = normalize(float3(-0.5,0.5,-0.5));
    color = float3(1,1,1);
    #else

    Light mainLight = GetMainLight();
    direction = mainLight.direction;
    color = mainLight.color;
    
    #endif
}

void GetLightShadow_float(float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
   #ifdef SHADERGRAPH_PREVIEW
   Direction = half3(0.5, 0.5, 0);
   Color = 1;
   DistanceAtten = 1;
   ShadowAtten = 1;
   #else
   
   half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
   Light mainLight = GetMainLight(shadowCoord);
   Direction = mainLight.direction;
   Color = mainLight.color;
   DistanceAtten = mainLight.distanceAttenuation;
   ShadowAtten = mainLight.shadowAttenuation;
   
   #endif
}

#endif