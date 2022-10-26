#ifndef JACK_CG_2D_INCLUDE
#define JACK_CG_2D_INCLUDE

// ==== Shape ====
// return 0 when pt not in circle
// return 1 when pt in circle
float InCircle(float2 pt, float2 center, float radius) {
    float len = length(pt - center);
    return 1.0 - step(radius, len);
}

float InCircleSmooth(float2 pt, float2 center, float radius, float smooth) {
    float len = length(pt - center);
    return 1.0 - smoothstep(radius - smooth, radius + smooth, len);
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

// ==== Matrix ====
float2x2 MatRotate(float angle) {
    float c = cos(angle);
    float s = sin(angle);
    return float2x2(c, -s, s, c);
}

float2x2 MatScale(float scale) {
    return float2x2(scale, 0, 0, scale);
}

#endif /* JACK_CG_2D_INCLUDE */ 