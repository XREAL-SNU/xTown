    CBUFFER_START(UnityPerMaterial)
        float4 _TintColor;
        float _Power;
        float _Intensity;
    CBUFFER_END

    struct VertexInput
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct VertexOutput
    {
        float4 vertex : SV_POSITION;
        float3 normal : NORMAL;
        float3 viewDir : TEXCOORD0;
    };

    VertexOutput vert(VertexInput v)
    {
        VertexOutput o;      
        o.vertex = TransformObjectToHClip(v.vertex.xyz);
        o.normal = TransformObjectToWorldNormal(v.normal);
        o.viewDir = normalize(_WorldSpaceCameraPos.xyz 
            - mul(GetObjectToWorldMatrix(), float4(v.vertex.xyz, 1.0)).xyz);

        return o;
    }

    half4 frag(VertexOutput i) : SV_Target
    {
        float NdotL = saturate(1 - dot(i.viewDir, i.normal));
        float rim = pow(NdotL, _Power);
        
        return rim * _TintColor * _Intensity;
    }