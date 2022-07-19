Shader "XTown/XRay"
{
    Properties
    {
        [IntRange] _StencilID("Stencil ID", Range(0,255)) = 0

        _TintColor("Tint Color", color) = (1, 1, 1, 1)
	    _Intensity("Intensity", Range(0, 1.5)) = 0.5
    }  

	SubShader
	{  

        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"          
            "Queue"="Geometry"
        }

    	Pass
    	{  		
            Name "Universal Forward"
            Tags { "LightMode" = "UniversalForward" }

            Blend Zero One
            ZWrite Off
            ZTest GEqual

            Stencil{
                Ref [_StencilID]
                Comp Always
                Pass Keep
                Fail Keep
                ZFail Replace
            }

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"        
            #include "XRay.hlsl"  	

            ENDHLSL  
    	}
     }

}