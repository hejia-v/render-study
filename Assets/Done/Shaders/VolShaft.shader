Shader "Custom/VolShaft" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        FracTex("Fractral Tex for shaft",2D)="white"{}          //光线材质
        BaseC("Base Color，基本颜色",color)=(1,1,1,1)            //基本颜色
        exL ("Extrusion，挤出强度", Range(0,12)) = 10.0          // 挤出强度
        kP("Factor of Power，光线强度",float)=1                  //光线强度，衰减系数的次方值
    }

    SubShader {
        Tags { "Queue" = "Transparent+10" }

        ZWrite Off
        Offset 1,1

        pass {
            SetTexture[_MainTex]{}
        }
        pass {
            Blend SrcAlpha OneMinusSrcAlpha // 源A + 背景RGBA*(1-源A)
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"

            struct v2f {
                float4 pos:SV_POSITION; //模型顶点
                float3 oP:TEXCOORD0;    //转为齐次坐标
                float exDist:TEXCOORD1; //挤出强度
                float4 oLitP:TEXCOORD2; //对象坐标中的【光源位置】
                float2 uv:TEXCOORD3;
            };
            sampler2D FracTex;  // 光线材质
            float4 BaseC;       // 基本颜色

            float exL;      // 挤出强度
            float kP;       // 光线强度
            float4 litPos;  // 光照位置

            v2f vert( appdata_base v ) : POSITION {
                v2f o;
                // 通过【模型到摄像机】的【逆转矩阵】，将【光源】转到【当前对象】坐标
                // 注：litPos【行向量】，必须【左乘】矩阵
                o.oLitP = mul(litPos, UNITY_MATRIX_IT_MV);
                // 【当前顶点】到【光源】的【单位向量】
                float3 toLit=o.oLitP.xyz - v.vertex.xyz * o.oLitP.w;
                float3 toLight =normalize(toLit);
                // dot点积：平行方向一样为1，方向相反为-1，垂直为0
                // 计算【法线】和【点到入射光方向】的点积
                float backFactor = dot( toLight, v.normal );
                // 当【点到入射光方向】与【法线】夹角大于90度时extrude为1，需要挤出
                // 当【点到入射光方向】与【法线】夹角小于90度时extrude为0，无需挤出
                float extrude = (backFactor < 0.0) ? 1.0 : 0.0;
                // 各方向挤出一点点
                v.vertex.xyz += v.normal*0.05;
                // 根据将【夹角大于90度的顶点】全部挤出。乘以【点到入射光方向】和【挤出增量】
                v.vertex.xyz -= toLight * (extrude * exL);
                // 输出模型顶点到屏幕
                o.pos= UnityObjectToClipPos( v.vertex );
                // 挤出强度
                o.exDist = extrude * exL;
                // 当前XYZ转为齐次坐标，好比在对象坐标中的位置
                o.oP = v.vertex.xyz/v.vertex.w;
                // 默认UV信息，不需要视检器设置
                o.uv = v.texcoord.xy;
                return o;
            }

            float4 frag(v2f i) : COLOR {
                // 取光线贴图的R,取RGBA都行，只要值不规则光线就不规则
                float alp = tex2D(FracTex,i.uv).r;
                // 计算【光源】到【当前点】的位置
                float toL = distance(i.oLitP.xyz,i.oP);
                // 像素点挤出的距离
                float dist = toL - exL;
                // 衰减系数 = 距离/强度（系数递减）
                float att = dist / exL;
                // 系数系数递增，所以光线挤出强度越大，光线越明显
                att = 1 - att;
                // 设置主颜色
                float4 c = BaseC * att;
                // 设置颜色透明度=主色的kp次幂 * 基本色A * 贴图R
                c.a = pow(att,kP) * BaseC.a * alp;
                return c;
            }
            ENDCG
        }
    }
    FallBack Off
}
