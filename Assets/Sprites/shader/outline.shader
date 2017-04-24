// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Outline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[MaterialToggle] HighQuality("HighQuality", Float) = 0

        // Add values to determine if outlining is enabled and outline color.
        [PerRendererData] _Outline ("Outline", Float) = 0
        [PerRendererData] _OutlineColor("Outline Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
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
            #pragma multi_compile _ PIXELSNAP_ON
            //#pragma shader_feature ETC1_EXTERNAL_ALPHA
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            float _Outline;
            fixed4 _OutlineColor;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float4 _MainTex_TexelSize;
			bool HighQuality;

            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);

                #if ETC1_EXTERNAL_ALPHA
                // get the color from an external texture (usecase: Alpha support for ETC1 on android)
                color.a = tex2D (_AlphaTex, uv).r;
                #endif //ETC1_EXTERNAL_ALPHA

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				fixed4 o = fixed4(1, 1, 1, 0) * _OutlineColor;
                // If outline is enabled and there is a pixel, try to draw an outline.
                if (_Outline > 0 && c.a != 0) {
					o.a = 1;
                    // Get the neighbouring four pixels.
                    /*fixed4 pixelUp = tex2D(_MainTex, IN.texcoord + fixed2(0, _MainTex_TexelSize.y));
                    fixed4 pixelDown = tex2D(_MainTex, IN.texcoord - fixed2(0, _MainTex_TexelSize.y));
                    fixed4 pixelRight = tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x, 0));
                    fixed4 pixelLeft = tex2D(_MainTex, IN.texcoord - fixed2(_MainTex_TexelSize.x, 0));

                    // If one of the neighbouring pixels is invisible, we render an outline.
                    if (pixelUp.a * pixelDown.a * pixelRight.a * pixelLeft.a == 0) {
                        c.rgba = fixed4(1, 1, 1, 1) * _OutlineColor;
                    }*/
					float alphaOutlineX = 1.0f;
					float alphaOutlineY = 1.0f;
					#define N 3
					if (HighQuality)
					{
						[unroll]
						for (int i = -N; i <= N; i++)
							[unroll]
							for (int j = -N; j < N; j++)
						{
							#if !defined(SHADER_API_OPENGL)
							fixed4 uv = fixed4(IN.texcoord + fixed2(_MainTex_TexelSize.x*i, _MainTex_TexelSize.y*j), 0, 0);
							#else
							fixed3 uv = fixed3(IN.texcoord + fixed2(_MainTex_TexelSize.x*i, _MainTex_TexelSize.y*j), 0);
							#endif
								
							float a = tex2Dlod(_MainTex, uv).a;
							alphaOutlineX *= a < 0.8 ? a : 1.0f;
						}
						o.a = (1.0 - alphaOutlineX);
					}
					else
					{
						[unroll]
						for (int i = -N; i <= 0; i++)
						{
							#if !defined(SHADER_API_OPENGL)
							fixed4 uv = fixed4(IN.texcoord + fixed2(_MainTex_TexelSize.x*i, 0), 0, 0);
							#else
							fixed3 uv = fixed4(IN.texcoord + fixed2(_MainTex_TexelSize.x*i, 0), 0);
							#endif
							alphaOutlineX *= tex2Dlod(_MainTex, uv).a;
						}
						[unroll]
						for (int ip = 1; ip <= N; ip++)
						{
							#if !defined(SHADER_API_OPENGL)
							fixed4 uv = fixed4(IN.texcoord + fixed2(_MainTex_TexelSize.x*ip, 0), 0, 0);
							#else
							fixed3 uv = fixed4(IN.texcoord + fixed2(_MainTex_TexelSize.x*ip, 0), 0);
							#endif
							alphaOutlineX *= tex2Dlod(_MainTex, uv).a;
						}
						[unroll]
						for (int j = -N; j < 0; j++)
						{
							#if !defined(SHADER_API_OPENGL)
							fixed4 uv = fixed4(IN.texcoord + fixed2(0, _MainTex_TexelSize.y*j), 0, 0);
							#else
							fixed3 uv = fixed4(IN.texcoord + fixed2(0, _MainTex_TexelSize.y*j), 0);
							#endif
							alphaOutlineY *= tex2Dlod(_MainTex, uv).a;
						}
						[unroll]
						for (int jp = 0; jp <= N; jp++)
						{
							#if !defined(SHADER_API_OPENGL)
							fixed4 uv = fixed4(IN.texcoord + fixed2(0, _MainTex_TexelSize.y*jp), 0, 0);
							#else
							fixed3 uv = fixed4(IN.texcoord + fixed2(0, _MainTex_TexelSize.y*jp), 0);
							#endif
							alphaOutlineY *= tex2Dlod(_MainTex, uv).a;
						}
						o.a = floor(1.0-alphaOutlineX * alphaOutlineY);
					}
                }

                c.rgb = lerp(c.rgba, o.rgba, o.a)*c.a;
                return c;
            }
            ENDCG
        }
    }
}
