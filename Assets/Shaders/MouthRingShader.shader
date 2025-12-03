Shader "Custom/MouthRing" {
    Properties {
        _MainColor ("Ring Color", Color) = (1, 1, 1, 1)
        _RingWidth ("Ring Width", Range(0, 0.5)) = 0.1
        _Smoothness ("Smoothness", Range(0, 0.5)) = 0.05
        _Intensity ("Intensity", Range(0, 1)) = 0.5
        _MinRadius ("Min Radius", Range(0, 1)) = 0.3
        _MaxRadius ("Max Radius", Range(0, 1)) = 0.9
        _MaxBrightness ("Max Brightness", Range(1, 5)) = 2.0
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float _RingWidth;
            float _Smoothness;
            float _Intensity;
            float _MinRadius;
            float _MaxRadius;
            float _MaxBrightness;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2.0 - 1.0; // Remap UVs to -1 to 1
                return o;
            }
            
float4 frag (v2f i) : SV_Target {
    float2 uv = i.uv;
    float dist = length(uv);
    
    // Map intensity to radius range
    float radius = lerp(_MinRadius, _MaxRadius, _Intensity);
    
    // Create ring with smooth edges
    float ring = smoothstep(_RingWidth + _Smoothness, _RingWidth, abs(dist - radius));
    
    // Fade out the ring towards the edges
    float alpha = ring * (1.0 - smoothstep(_MaxRadius, 1.0, dist));
    
    // Enhanced brightness with color shifting
    float3 baseColor = _MainColor.rgb;
    
    // Shift hue slightly based on intensity (makes color more vibrant)
    float hueShift = _Intensity * 0.2; // Small hue shift for more dynamic feel
    float3 shiftedColor = baseColor * (1.0 + hueShift * float3(1.0, 0.5, 0.0)); // Shift towards yellow
    
    // Apply brightness with stronger curve
    float brightness = 1.0 + (_MaxBrightness * 2.0 - 1.0) * pow(_Intensity, 0.5);
    float3 finalColor = shiftedColor * brightness;
    
    // Add a strong white highlight that pulses with intensity
    if (_Intensity > 0.1) {
        float angle = atan2(uv.y, uv.x);
        float highlight = 0.5 * (sin(angle * 10.0 + _Time.y * 4.0) * 0.5 + 0.5) * _Intensity;
        finalColor += highlight;
    }
    
    // Add glow that's more visible with the blue color
    float glow = smoothstep(0.8, 0.0, abs(dist - radius) / _RingWidth) * _Intensity * 0.8;
    finalColor = finalColor * (1.0 - glow * 0.5) + float3(1.0, 1.0, 1.0) * glow;
    
    // Ensure we don't go over full brightness
    finalColor = min(finalColor, float3(2.0, 2.0, 2.0));
    
    return float4(finalColor, _MainColor.a * alpha);
}
            ENDCG
        }
    }
    FallBack "Transparent"
}