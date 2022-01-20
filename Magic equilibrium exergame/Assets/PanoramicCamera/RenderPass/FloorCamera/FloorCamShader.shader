Shader "FloorCustomRendererShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Tex("InputTex", 2D) = "white" {}
        _Camera0Tex("Camera0", 2D) = "white" {}
        _Camera1Tex("Camera1", 2D) = "white" {}
     }

     SubShader
     {
        Lighting Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0

            float4      _Color;
            sampler2D   _Tex;
            sampler2D   _Camera0Tex;
            sampler2D   _Camera1Tex;

            float4 frag(v2f_customrendertexture IN) : COLOR
            {
                return _Color * tex2D(_Camera0Tex, IN.localTexcoord.xy);
            }
            ENDCG
            }
    }
}