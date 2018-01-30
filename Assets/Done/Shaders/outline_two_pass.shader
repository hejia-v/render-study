Shader "cel/outline/two-pass" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.03)) = .005
		_MainTex ("Base (RGB)", 2D) = "white" { }
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	}

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    };

    struct v2f {
        float4 pos : POSITION;
        float4 color : COLOR;
    };

    uniform float _Outline;  // uniform变量, 外部程序传递给shader的变量.
    uniform float4 _OutlineColor;

    v2f vert(appdata v) {
        // 复制一份顶点数据，并沿法线方向放大
        v2f o;
        // 传递进来的顶点坐标是模型坐标系中的坐标值，需要经过矩阵转换成相机的剪裁空间中的齐次坐标
        o.pos = UnityObjectToClipPos(v.vertex);

        // 法向量是方向不是点，法向量需乘modelview的逆转置矩阵UNITY_MATRIX_IT_MV,可以保证变换后法向量仍然垂直于平面
        float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
        float2 offset = TransformViewToProjection(norm.xy);  // z是深度方向，没考虑?

        o.pos.xy += offset * o.pos.z * _Outline;
        o.color = _OutlineColor;
        return o;
    }
    ENDCG

	SubShader {
		Tags { "Queue" = "Transparent" }

		// note that a vertex shader is specified here but its using the one above
		Pass {
			Name "OUTLINE"  // UsePass command 可以通过pass名称来引用pass
			Tags { "LightMode" = "Always" }
			Cull Off  // 可优化
			ZWrite Off
			ZTest Always // 默认是LEqual:小于等于。Always指的是直接将当前像素颜色(不是深度)写进颜色缓冲区中

			// you can choose what kind of blending mode you want for the outline
			Blend SrcAlpha OneMinusSrcAlpha // Normal
			//Blend One One // Additive
			//Blend One OneMinusDstColor // Soft Additive
			//Blend DstColor Zero // Multiplicative
			//Blend DstColor SrcColor // 2x Multiplicative

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(v2f i) : COLOR {
                return i.color;
            }
            ENDCG
        }

        CGPROGRAM
        #pragma surface surf Lambert
        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
        };
        sampler2D _MainTex;
        sampler2D _BumpMap;
        uniform float3 _Color;
        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
        }
        ENDCG
	}
	FallBack "Diffuse"
}
