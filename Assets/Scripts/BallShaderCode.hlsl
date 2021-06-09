#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED



float3 SampleByProgress(float progress, texture2D colorText, SamplerState ss, int part){
    //progress = frac(progress);

    //progress = clamp(progress, 0, 1);

    float2 suv = float2(progress, 0.5);
    

    return SAMPLE_TEXTURE2D(colorText, ss, suv).xyz;

    

    /*float r = (progress > 0.00 && progress <= 0.33)?1:0;
    float g = (progress > 0.33 && progress <= 0.66)?1:0;
    float b = (progress > 0.66 && progress <= 1.00)?1:0;

    return float3(r, g, b);*/

}

float GetPartialColorProgressFromUV(float2 uvIN, float xShift, float noise, float3 pos){
    float2 uv1 = uvIN - float2(xShift, 0.5);

    float ang = atan2(uv1.y, uv1.x);

    ang += length(pos.xyz) * 6.5;


    uv1 = normalize(float2(cos(ang), sin(ang))) * (length(uv1) + (sin(3. * ang) * sin(5. * ang ) * sin(7. * ang ) * 0.02));
    

    float colorProg = clamp(length(uv1) * 2. * 0.75, 0., 0.99); //the 0.75 is estimated from eyebolling
    colorProg = clamp((colorProg + (noise - 0.5) * 0.15), 0., 0.99); //add some randomness

    return colorProg;
}



//PATTERN GENERATION BY UV DISTORTION
float GetColorProgressFromUV(float2 uvO, float noise, float3 pos){

    
    float prog1 = GetPartialColorProgressFromUV(uvO, 0.8, noise, pos);
    float prog2 = GetPartialColorProgressFromUV(uvO, 0.2, noise, pos);

    return min(prog1, prog2);


}

void GetFullColor_float(float2 sphereUV1, float2 sphereUV2, texture2D colorText, SamplerState ss, float3 Pos, float noise, out float3 ColOut){

    //if (Pos.z > 0){

        //ColOut = GetPattern(sphereUV1, colorText, ss, 0, noise);

        float prog = GetColorProgressFromUV(sphereUV1, noise, Pos);

        ColOut =  SampleByProgress(prog, colorText, ss, 0);




    //}else{
    //    ColOut = GetPattern(sphereUV2, colorText, ss, 1, noise);
    //}

    //ColOut = float3(sphereUV.xy, 0);

}

void GetPatterDebug_float(float2 UVIn, texture2D colorText, SamplerState ss, float noise, out float3 ColOut){

    float prog = GetColorProgressFromUV(UVIn, noise, float3(0,0,0));
    ColOut =  SampleByProgress(prog, colorText, ss, 0);


}


float2 SphericalUV(float3 vecIn){
    float u = 0.;
    float v = 0.;

    /*u = 0.5 + atan2((vecIn.y / offset.y), (vecIn.z / offset.z))/ (2 * 1.43);
    v = 0.5 + asin((vecIn.x / offset.x) / 1) / 1.43;*/

    
    u = atan2(vecIn.x, vecIn.z) / (2. * PI) + 0.5;
    v = vecIn.y * 0.5 + 0.5;


    return float2(clamp(u, 0, 1), clamp(v, 0, 1));
}


void SphericalUV_float(float3 Pos, out float2 ShereUV){
    ShereUV  = SphericalUV(normalize(Pos));
}

void GetPattern_float(Texture2D tex, Texture2D hl, float2 uv, float2 pos, float3 col, float noise, SamplerState ss, out float3 o)
{
    o = 0;
    float2 cuv = uv - float2(0.5f, 0.5f);
    if (length(cuv) > 0.5f)
    {
        discard;
    }else
    {

        
        float2 bguv = float2((pos.x - 0.5f) * 0.8f + 0.5f, pos.y * 1.1f);
        bguv += cuv * length(cuv) * 0.3f;

        float2 noiseUV = float2(noise, 1 - noise) * 0.05; 
        
        float3 backC = SAMPLE_TEXTURE2D(tex, ss, bguv + noiseUV).xyz;

        float3 tintC = lerp(col, backC, 0.3f + 0.25 - length(cuv) / 2.) * 0.9 + col * 0.1;
        //float3 tintC = col *  (backC * 0.5) * 0.9 + col * 0.1;
        
        float4 hlc = SAMPLE_TEXTURE2D(hl, ss, uv); 
        o = lerp(tintC, hlc.xyz, hlc.w);
          

        
    }
    
}


#endif
