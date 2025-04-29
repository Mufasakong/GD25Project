Shader "Custom/WavySprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Float) = 0.1
        _Frequency ("Frequency", Float) = 1.0
        _Speed ("Speed", Float) = 1.0
        _WaveOffset ("Wave Offset", Float) = 0.0
        _Sharpness ("Sharpness", Float) = 1.0  // For controlling sharpness
        _UseSharpWaves ("Use Sharp Waves", Float) = 0.0  // Toggle for sharp waves
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amplitude;
            float _Frequency;
            float _Speed;
            float _WaveOffset;
            float _Sharpness;
            float _UseSharpWaves;  // Toggle to choose wave style

            v2f vert (appdata v)
            {
                v2f o;

                float3 pos = v.vertex.xyz;

                float wave;

                // Toggle between smooth sine wave and sharp triangle wave
                if (_UseSharpWaves > 0.5)
                {
                    // Sharp wave (triangle wave)
                    wave = abs(sin((pos.x * _Frequency) + (_Time.y * _Speed) + _WaveOffset));
                    wave = pow(wave, _Sharpness);  // Apply sharpness
                }
                else
                {
                    // Smooth sine wave
                    wave = sin((pos.x * _Frequency) + (_Time.y * _Speed) + _WaveOffset);
                }

                // Move only top vertices
                if (pos.y > 0.0)
                {
                    pos.y += wave * _Amplitude;
                }

                o.vertex = UnityObjectToClipPos(float4(pos, 1.0));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
