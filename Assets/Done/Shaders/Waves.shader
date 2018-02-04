// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Waves" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _A ("振幅(最大和最小的幅度)", Range(0, 1)) = 0.3
        _W ("角速度(圈数)", Range(0, 50)) = 10
        _Speed ("移动速度", Range(0, 30)) = 10
    }

    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma target 3.0

            float _A;
            float _W;
            float _Speed;

            struct vertOut {
                float4 pos:SV_POSITION;
                float4 srcPos:TEXCOORD0;
            };

            vertOut vert(appdata_base v) {
                vertOut o;
                o.pos = UnityObjectToClipPos (v.vertex);
                // 将顶点转换到屏幕位置
                o.srcPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag(vertOut i) : COLOR0 {
                // 背景颜色
                fixed3 colorBg = fixed3(0.0,0.0,0.3);
                // 单元格颜色
                fixed3 colorCell = fixed3(0.0,0.5,0.0);
                // 单元格宽，想象全屏幕高宽为1
                float cellWidth = 0.05;
                // 可以想象成UV平铺到屏幕
                float2 uv = (i.srcPos.xy/i.srcPos.w);
                // 定义输出色，背景色，波浪色
                fixed3 finalColor = fixed3(1.0, 1.0, 1.0);
                fixed3 bgColor = fixed3(0.0, 0.0, 0.0);
                fixed3 waveColor = fixed3(0.0, 0.0, 0.0);
                // 公式：取余
                // C1=0,1,2,3,4,5,6,7,8,9,0,1,2,3,4......
                float c1 = fmod(uv.x, 2.0 * cellWidth);
                // 公式：如果a>x结果是0, 否则是1
                // C1=0,0,0,0,0,1,1,1,1,1,0,0,0.....
                c1 = step(cellWidth, c1);

                // C2=0,1,2,3,4,5,6,7,8,9,0,1,2,3,4......
                float c2 = fmod(uv.y, 2.0 * cellWidth);
                // C2=0,0,0,0,0,1,1,1,1,1,0,0,0.....
                c2 = step(cellWidth, c2);
                // C1*C2，可表示下图：所以0为背景色，1为单元格色
                // 0,0,0,0,0
                // 0,1,0,1,0
                // 0,0,0,0,0
                // 0,1,0,1,0
                bgColor = lerp(uv.x * colorBg, uv.y * colorCell, c1*c2);
                // 综上所述：对单元格x2取余，然后将单元格与余数对比取0或1，可形成虚线效果

                float waveWidth = 0.01;
                // 为了方便曲线公式计算
                // 将UV的值域[0,1]变成[-1,1]
                uv = -1.0 + 2.0 * uv;

                // 这个时候其实所以的点都在进行曲线运动
                uv.y += (_A * sin(_W * uv.x + _Speed * _Time.y));
                // 绝对值，让靠近中心的颜色值才显示
                // y值域为[-1,1]，所以越靠近中心wave_width值越大
                waveWidth = abs(1.0 / (50.0 * uv.y));
                // 乘以一定的系数，只是为了上些颜色
                waveColor += fixed3(waveWidth * 0.5, waveWidth * 1.8, waveWidth * 1.5);
                // 融合
                finalColor = bgColor + waveColor;
                return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
    FallBack Off
}
