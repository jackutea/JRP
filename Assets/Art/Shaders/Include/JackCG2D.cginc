#ifndef JACK_CG_2D_INCLUDE
#define JACK_CG_2D_INCLUDE

// return 0 when pt not in circle
// return 1 when pt in circle
float InCircle(float2 pt, float2 center, float radius) {
    float len = length(pt - center);
    return 1.0 - step(radius, len);
}

// return 0 when pt not in rectangle
// return 1 when pt in rectangle
float InRect(float2 pt, float2 center, float2 size) {
    float2 p = pt - center;
    float2 halfSize = size * 0.5;
    float horz = step(-halfSize.x, p.x) - step(halfSize.x, p.x);
    float vert = step(-halfSize.y, p.y) - step(halfSize.y, p.y);
    return horz * vert;
}

#endif /* JACK_CG_2D_INCLUDE */ 