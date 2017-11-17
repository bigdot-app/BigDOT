// this shader created a basic gradient between an initial color to an
Shader "Chart/WorldSpace/GradientOutline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Angle("Angle", Float) = 0
		_Combine("Combine", Color) = (1,1,1,0)
		_ColorFrom("From", Color) = (1,1,1,1)
		_ColorTo("To", Color) = (1,1,1,1)
		_Ramp("Illumination Ramp (RGB)", 2D) = "gray" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005
		_ChartTiling("Tiling",Float) = 1
	}
	SubShader
	{ 
		UsePass "Chart/WorldSpace/Gradient/FORWARD"
		UsePass "Toon/Basic Outline/OUTLINE"
	}
}