Shader "Custom/BasicShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	
	}
	SubShader {
		color [_Color]	
		Pass {}
	}
	FallBack "Diffuse"
}
