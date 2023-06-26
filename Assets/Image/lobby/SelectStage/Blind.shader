Shader "Custom/Blind"
{
    Properties //에디터에서 변수의 값을 변경할 수 있도록 인터페이스 추가.
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Tiling ("Tiling", float) = 10
    }
    SubShader //셰이더 작성
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM //CG언어로 작성됩니다.
        
        //#pragma 전처기 과정.
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        //내부에 포함될 데이터 작성.
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        float _Tiling;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        //셰이더 작성.
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, floor(IN.uv_MainTex*_Tiling)/(_Tiling));
            o.Emission = c;
            o.Alpha = c.a; 
        }
        ENDCG
    }
    FallBack "Diffuse"
}
