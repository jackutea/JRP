#ifndef JACK_CG_GENERIC_INCLUDE
    #define JACK_CG_GENERIC_INCLUDE

    float Random(float2 pt) {
        float a = 12.9898;
        float b = 78.233;
        float c = 43758.5453;
        return frac(sin(dot(pt, float2(a,b))) * c);
    }

    float RandomSeed(float2 pt, float seed) {
        float a = 12.9898;
        float b = 78.233;
        float c = 43758.5453;
        return frac(sin(dot(pt, float2(a,b)) + seed) * c);
    }

    float RandomCustom(float2 pt, float rd_a, float rd_b, float rd_c, float seed) {
        return frac(sin(dot(pt, float2(rd_a, rd_b)) + seed) * rd_c);
    }

    float Noise(float2 st) {
        float2 i = floor(st);
        float2 f = frac(st);

        float a = Random(i);
        float b = Random(i + float2(1.0, 0.0));
        float c = Random(i + float2(0.0, 1.0));
        float d = Random(i + float2(1.0, 1.0));

        float2 u = f * f * (3.0 - 2.0 * f);
        
        return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
        
    }

#endif /* JACK_CG_GENERIC_INCLUDE */ 