Shader "UI/Blur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurSize("Blur Size", Range(0.0, 10.0)) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _BlurSize;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.position = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                float4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;
                    float4 color = float4(0, 0, 0, 0);

                    float offset = _BlurSize / 100.0;
                    for (int x = -2; x <= 2; x++)
                    {
                        for (int y = -2; y <= 2; y++)
                        {
                            color += tex2D(_MainTex, uv + float2(x, y) * offset);
                        }
                    }

                    color /= 25.0; // 평균값을 계산해 부드러운 블러 효과 적용
                    return color;
                }
                ENDCG
            }
        }
}
