Shader "Unlit/WobblyLinesWithOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ImperfectionStrength("Imperfection Strength", float) = 0
        _WobbleStrength("Wobble Strength", float) = 0
        _FrameRate("Frame Rate", int) = 1
        _ManualEnthickenLines("Manual Enthicken Lines", float) = 0

        // Outline settings
        [Toggle(_ENABLE_OUTLINE)] _EnableOutline("Enable Outline", Float) = 0
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline Thickness", Range(0,10)) = 2
    }
    SubShader
    {
        Tags {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "ClassicNoise2D.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            float _EnthickenLines;
            float _ManualEnthickenLines;
            float _ImperfectionStrength;
            float _WobbleStrength;
            int _FrameRate;

            float _EnableOutline;
            float4 _OutlineColor;
            float _OutlineThickness;

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float noice(float2 pos, float offset) {
                return cnoise(pos + float2(offset, offset));
            }

            float ImperfectionFunc(float2 pos, float time) {
                float val = noice(pos * 100, time) + noice(pos * 60, time * 2);
                val = round(abs(val) * 4) / 4;
                return val;
            }

            float OffsetFunc(float2 pos, float time) {
                if (_WobbleStrength < 0.0001)
                    return 0;
                float wob = 1 / _WobbleStrength;
                time = time * 8;
                return noice(pos * 24 * wob, time) * 0.3 + noice(pos * 48 * wob, time) * 0.2;
            }

            v2f vert (appdata IN)
            {
                v2f OUT;
                float3 pos = IN.vertex.xyz;

                if (_EnableOutline > 0.5)
                {
                    // Expand the mesh by OutlineThickness in clip space
                    float2 expand = (_OutlineThickness * 2) * _MainTex_TexelSize.xy;
                    pos.xy += normalize(IN.vertex.xy) * expand;
                }

                OUT.vertex = UnityObjectToClipPos(float4(pos,1));
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                return OUT;
            }


            float4 WobblyLineFunc(float2 pos) {
                float screenTextureRatio = _MainTex_TexelSize.zw / _ScreenParams.xy;
                float2 screenAdjustedPos = pos * screenTextureRatio * 0.15;
                float modTime = round(_Time.y*_FrameRate) / _FrameRate;
                float2 imperfectionCoord = float2(ImperfectionFunc(screenAdjustedPos, 0), ImperfectionFunc(screenAdjustedPos, 1)) * 0.0025 * _ImperfectionStrength;
                float2 wobbleCoord = float2(OffsetFunc(screenAdjustedPos, modTime), OffsetFunc(screenAdjustedPos, modTime * 3.14)) * 0.0025 * _WobbleStrength;
                float aspectCorrection = (_MainTex_TexelSize.z / _MainTex_TexelSize.w);

                float2 finalCoord = pos + (imperfectionCoord + wobbleCoord) * aspectCorrection * (1/screenTextureRatio) * 10;

                // ✅ instead of discarding, just clamp
                return tex2D(_MainTex, clamp(finalCoord, 0.0, 1.0));
            }


            float4 NeighborSample(float2 pos, float deltaX, float deltaY)
            {
                float4 s = WobblyLineFunc(pos + float2(deltaX, deltaY));
                if (s.r > 0.2f)
                    return float4(0, 0, 0, 0);
                return s;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                float2 snappedCoord = IN.texcoord;

                float enthicken = _EnthickenLines;
                if (_ManualEnthickenLines > 0)
                    enthicken = _ManualEnthickenLines;

                // Base line render
                fixed4 c =
                    WobblyLineFunc(snappedCoord) +
                    step(0.5, enthicken) * NeighborSample(snappedCoord, -_MainTex_TexelSize.x, 0) +
                    step(0.5, enthicken) * NeighborSample(snappedCoord, 0, -_MainTex_TexelSize.y) +
                    step(0.9, enthicken) * NeighborSample(snappedCoord, _MainTex_TexelSize.x, 0) +
                    step(0.9, enthicken) * NeighborSample(snappedCoord, 0, _MainTex_TexelSize.y);

                // Outline effect (smooth and thick)
                if (_EnableOutline > 0.5)
                {
                    float2 pixelSize = _MainTex_TexelSize.xy;
                    float outlineMask = 0.0;

                    // Sample in a circular pattern
                    for (int x = -3; x <= 3; x++)
                    {
                        for (int y = -3; y <= 3; y++)
                        {
                            float2 offset = float2(x, y) * pixelSize * _OutlineThickness;
                            outlineMask += WobblyLineFunc(snappedCoord + offset).a;
                        }
                    }

                    if (outlineMask > 0.0 && c.a < 0.1) // only outside main lines
                        c = _OutlineColor;
                }

                c *= IN.color;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }
    }
}
