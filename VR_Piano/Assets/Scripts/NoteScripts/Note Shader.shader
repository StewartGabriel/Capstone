Shader "Custom/FadeDistance"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _FadeStart ("Fade Start Distance", Float) = 10.0
        _FadeEnd ("Fade End Distance", Float) = 20.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        // Enable transparency
        Pass
        {
            Tags { "Queue"="Overlay" }

            // Set transparency blend mode
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            uniform float _FadeStart;
            uniform float _FadeEnd;
            uniform float4 _Color;

            float distanceFromCamera(float3 worldPos)
            {
                return length(_WorldSpaceCameraPos - worldPos);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                float dist = distanceFromCamera(v.vertex.xyz);
                float alpha = 1.0;

                // Calculate fade based on distance
                if (dist > _FadeStart)
                {
                    alpha = 1.0 - (dist - _FadeStart) / (_FadeEnd - _FadeStart);
                    alpha = saturate(alpha);  // Ensure alpha stays between 0 and 1
                }
                o.color = _Color;
                o.color.a *= alpha;

                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }

    // Fallback in case a valid shader couldn't be compiled
    Fallback "Diffuse"
}
