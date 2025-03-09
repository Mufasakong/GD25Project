Shader "Custom/ShadowCasterOnly"
{
    SubShader
    {
        Tags {"Queue"="Geometry" "RenderType"="Opaque"}
        Pass
        {
            Tags {"LightMode"="ShadowCaster"}
            ColorMask 0 
        }
    }
}
