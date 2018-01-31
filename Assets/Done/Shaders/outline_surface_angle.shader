Shader "cel/outline/outline-surface-angle" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Outline ("Outline", Range(0,1)) = 0.4
        _SilhouetteTex ("Silhouette Texture", 2D) = "white" {}
    }
    SubShader {
        Pass {
            Tags { "RenderType"="Opaque" }
            LOD 200

            CGPROGRAM
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _Outline;
            sampler2D _SilhouetteTex;

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldLightDir: TEXCOORD2;
                float3 worldViewDir: TEXCOORD3;
            };

            v2f vert(appdata_full i) {
                v2f o;
                o.pos= UnityObjectToClipPos(i.vertex);
                o.uv = i.texcoord;
                o.worldNormal = mul(i.normal, (float3x3)unity_WorldToObject);  // 把顶点法线变换到世界坐标系
                // 这里Unity Shader封装了一个函数来转换法线到世界坐标UnityObjectToWorldNormal()，替代上面的矩阵乘法写法，不容易出错。
                // o.worldNormal = UnityObjectToWorldNormal(i.normal);
                o.worldLightDir = mul((float3x3)unity_ObjectToWorld, ObjSpaceLightDir(i.vertex));
                o.worldViewDir = mul((float3x3)unity_ObjectToWorld, ObjSpaceViewDir(i.vertex));

                TRANSFER_VERTEX_TO_FRAGMENT(o);

                return o;
            }

            fixed3 GetSilhouetteUseConstant(fixed3 normal, fixed3 vierDir) {
                fixed edge = saturate(dot (normal, vierDir));
                edge = edge < _Outline ? edge/4 : 1;

                return fixed3(edge, edge, edge);
            }

            fixed3 GetSilhouetteUseTexture(fixed3 normal, fixed3 vierDir) {
                fixed edge = dot(normal, vierDir); // edge值在[-1, 1]
                edge = edge * 0.5 + 0.5;  //edge值在[0,1]
                return tex2D(_SilhouetteTex, fixed2(edge, edge)).rgb;
            }

            fixed4 frag(v2f i) : COLOR {
                fixed3 worldNormal = normalize(i.worldNormal);  // normalize(v): 返回v向量的单位向量
                fixed3 worldLigthDir = normalize(i.worldLightDir);
                fixed3 worldViewDir = normalize(i.worldViewDir);

                fixed3 col = tex2D(_MainTex, i.uv).rgb;

                // Use a constant to render silhouette
                // fixed3 silhouetteColor = GetSilhouetteUseConstant(worldNormal, worldViewDir);
                // Or use a one dime silhouette texture
                fixed3 silhouetteColor = GetSilhouetteUseTexture(worldNormal, worldViewDir);

                fixed4 fragColor;
                fragColor.rgb = col * silhouetteColor;
                fragColor.a = 1.0;

                return fragColor;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}


// 对于一些模型，这种方法的轮廓效果很难控制。有的地方轮廓很宽，有的地方却又捕捉不到

// 这种方法的优点在于非常简单快速，我们可以在一个pass里就得到结果，而且还可以使用texture filtering对轮廓线进行抗锯齿。

// 但是也有很多局限性，只适用于某些模型，而对于像cube这样的模型就会有问题。
// 虽然我们可以使用一些变量来控制轮廓线的宽度（如果使用纹理的话就是纹理中黑色的宽度），
// 但实际的效果是依赖于表面的曲率（curvature）的。对于像cube这样表面非常平坦的物体，
// 它的轮廓线会发生突变，要么没有，要么就全黑。
