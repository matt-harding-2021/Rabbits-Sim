// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Stencil/Outline"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _BodyColor("BodyColor", Color) = (1,1,1,1)
        _Thickness("Thickness", float) = 10
    }
        
    SubShader
    {

        Tags { "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        ZTest always
        Pass
        {
            Stencil {
                Ref 1
                Comp always
                Pass replace
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct v2g
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
                float3  viewT : TANGENT;
                float3  normals : NORMAL;
            };

            struct g2f
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
                float3  viewT : TANGENT;
                float3  normals : NORMAL;

            };

            v2g vert(appdata_base v)
            {
                v2g OUT;
                OUT.pos = UnityObjectToClipPos(v.vertex);
                OUT.uv = v.texcoord;
                OUT.normals = v.normal;
                OUT.viewT = ObjSpaceViewDir(v.vertex);

                return OUT;
            }

            sampler2D _mainTex;
            half4 frag(g2f IN) : COLOR
            {
                fixed4 col = tex2D(_mainTex, IN.uv);
                col.r = 0.6;
                col.g = 0.6;
                col.b = 0.6;
                col.a = 1;
                return col;
            }
            ENDCG
        }
        Pass{
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            Material{
            Diffuse[_BodyColor]
            Ambient[_BodyColor]
        }
        Lighting On
        SetTexture[_MainTex]{
            Combine texture * primary DOUBLE, texture * primary
        }
        }
        Pass
        {
            Stencil {
                Ref 0
                Comp equal
            }
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma target 4.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag


            half4 _Color;
            float _Thickness;

            struct v2g
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewT : TANGENT;
                float3 normals : NORMAL;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewT : TANGENT;
                float3 normals : NORMAL;
            };

            v2g vert(appdata_base v)
            {
                v2g OUT;
                OUT.pos = UnityObjectToClipPos(v.vertex);

                OUT.uv = v.texcoord;
                OUT.normals = v.normal;
                OUT.viewT = ObjSpaceViewDir(v.vertex);

                return OUT;
            }

            void geom2(v2g start, v2g end, inout TriangleStream<g2f> triStream)
            {
                float thisWidth = _Thickness / 100;
                float4 parallel = end.pos - start.pos;
                normalize(parallel);
                parallel *= thisWidth;

                float4 perpendicular = float4(parallel.y,-parallel.x, 0, 0);
                perpendicular = normalize(perpendicular) * thisWidth;
                float4 v1 = start.pos - parallel;
                float4 v2 = end.pos + parallel;
                g2f OUT;
                OUT.pos = v1 - perpendicular;
                OUT.uv = start.uv;
                OUT.viewT = start.viewT;
                OUT.normals = start.normals;
                triStream.Append(OUT);

                OUT.pos = v1 + perpendicular;
                triStream.Append(OUT);

                OUT.pos = v2 - perpendicular;
                OUT.uv = end.uv;
                OUT.viewT = end.viewT;
                OUT.normals = end.normals;
                triStream.Append(OUT);

                OUT.pos = v2 + perpendicular;
                OUT.uv = end.uv;
                OUT.viewT = end.viewT;
                OUT.normals = end.normals;
                triStream.Append(OUT);
            }

            [maxvertexcount(12)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
            {
                geom2(IN[0],IN[1],triStream);
                geom2(IN[1],IN[2],triStream);
                geom2(IN[2],IN[0],triStream);
            }

            half4 frag(g2f IN) : COLOR
            {
                _Color.a = 1;
                return _Color;
            }

            ENDCG

        }
    }
        FallBack "Diffuse"
}