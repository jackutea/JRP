#ifndef JACK_CG_GENERIC_INCLUDE
#define JACK_CG_GENERIC_INCLUDE

float PerlinNoise(float2 pt, float seed) {
    float a = 1;
    float b = 7;
    float c = 43758.5453;
    return frac(sin(dot(pt, float2(a,b)) + seed) * c);
}

float PerlinNoiseCustom(float2 pt, float rd_a, float rd_b, float rd_c, float seed) {
    return frac(sin(dot(pt, float2(rd_a, rd_b)) + seed) * rd_c);
}

#endif /* JACK_CG_GENERIC_INCLUDE */ 