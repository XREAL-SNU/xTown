Shader "XTown/XRay"
{
    Properties
    {
        _TintColor("Tint Color", color) = (1, 1, 1, 1)
	    _Intensity("Intensity", Range(0, 1.5)) = 0.5
    }  

	SubShader
	{  

        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"          
            "Queue"="Transparent"
        }

    	Pass
    	{  		
            Name "Universal Forward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite Off
            ZTest GEqual

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