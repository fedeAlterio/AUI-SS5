Shader "Hidden/Shader/SimplePP"
{
    Properties
    {
        _CameraATex("_CameraATex", 2D) = "yellow" {}
    }



    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    float _Intensity;
    TEXTURE2D_X(_InputTexture);
    sampler2D   _CameraATex;
    float4x4    _FloorNormalizedToCameraClip;


    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float4 normalizedFloorCoordinates = float4(input.texcoord.x, input.texcoord.y, 0.0f, 1.0f);
        float4 cameraClipCoordinates = mul(_FloorNormalizedToCameraClip, normalizedFloorCoordinates);
        cameraClipCoordinates = cameraClipCoordinates / cameraClipCoordinates.w;
        float2 texCoords = float2(1 - cameraClipCoordinates.x, cameraClipCoordinates.y);
        uint2 positionSS = texCoords * _ScreenSize.xy;
        float3 outColor = LOAD_TEXTURE2D_X(_InputTexture, positionSS).xyz;
        //float4 aaa = tex2D(_CameraATex, normalizedCoords);
        return float4(outColor, 1.0f);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "SimplePP"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
