Shader "Unlit/FadeToBlack" {
    Properties {
        _Alpha ("Transparency", Range(0, 1)) = 0.2
        _Color ("Color", Color) = (0, 0, 0, 1)
    }
    SubShader {
        //Sets the rendering order. "Transparent+5" means it will be rendered after most other transparent objects.
        //IgnoreProjector: Indicates that the material is not affected by projectors.
        //RenderType: Classified as "Transparent" for rendering purposes.
        Tags { "Queue" = "Transparent+5" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 100

        Pass {
            //Off == render back faces
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            //Always == render the object on top of everything else
            ZTest Always
            //Disables depth writing, important for transparent objects to render correctly.
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //For Single-pass instanced rendering (see OpenXR serrings in Project Setings)
                //Render Mode: Single pass Instanced
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                //For Single-pass instanced rendering (see OpenXR serrings in Project Setings)
                //Render Mode: Single pass Instanced
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float4 _Color;
            float _Alpha;

            v2f vert(appdata v) {
                v2f o;
                // calculates and sets the built-in unity_StereoEyeIndex and unity_InstanceID
                //shader variables to the correct values
                UNITY_SETUP_INSTANCE_ID(v);
                //initializes all v2f values to 0.
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                //tells the GPU which eye in the texture array it should render to,
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            ////When you use a particular stereo rendering method the GPU uses the appropriate texture sampler
            //UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);

            fixed4 frag(v2f i) : SV_Target {
                //You only need to add this macro if you want to use
                //the unity_StereoEyeIndex built-in shader variable
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                fixed4 color = _Color;
                color.a = _Alpha;
                return color;
            }
            ENDCG
        }
    }
}