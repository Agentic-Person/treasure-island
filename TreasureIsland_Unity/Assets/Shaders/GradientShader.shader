Shader "Custom/BlueWhiteGradient"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _BottomColor ("Bottom Color", Color) = (0,0.5,1,1)
        _GradientHeight ("Gradient Height", Range(0.1, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        struct Input
        {
            float3 worldPos;
        };

        fixed4 _TopColor;
        fixed4 _BottomColor;
        float _GradientHeight;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Get the local Y position relative to object
            float localY = IN.worldPos.y;
            
            // Calculate gradient based on world position
            float gradient = saturate((localY + _GradientHeight) / (_GradientHeight * 2));
            
            // Lerp between bottom (blue) and top (white)
            fixed4 c = lerp(_BottomColor, _TopColor, gradient);
            
            o.Albedo = c.rgb;
            o.Metallic = 0;
            o.Smoothness = 0.5;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}