using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolShaft : MonoBehaviour
{
    public bool bPlay = false;
    public Light shadowLight;
    public Material defaultMat;
    public SkinnedMeshRenderer[] objs;
    public Material[] rayMats;

    void OnPostRender()
    {
        if (!bPlay)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].sharedMaterial = defaultMat;
            }
            return;
        }
        if (!enabled) return;

        Vector4 lightPos;
        if (shadowLight.type == LightType.Directional)
        {
            Vector3 dir = -shadowLight.transform.forward;
            // 将【世界坐标】中的【光源方向】变换到【摄像机坐标】中
            dir = transform.InverseTransformDirection(dir);
            // 【平行光】表示方向，（向量）转为齐次坐标时W是0，表示无限远。
            lightPos = new Vector4(dir.x, dir.y, -dir.z, 0.0f);
        }
        else
        {
            Vector3 pos = shadowLight.transform.position;
            // 将【世界坐标】中的【光位置】变换到【摄像机坐标】中
            pos = transform.InverseTransformPoint(pos);
            // 【点光源】表示位置，(点)转为齐次坐标时W是1
            lightPos = new Vector4(pos.x, pos.y, -pos.z, 1.0f);
        }

        DrawShaft(objs, lightPos, rayMats);
    }

    void DrawShaft(SkinnedMeshRenderer[] objs, Vector4 lightPos, Material[] rayMats)
    {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].sharedMaterial = rayMats[i];
            objs[i].sharedMaterial.SetVector("litPos", lightPos);
            objs[i].sharedMaterial.SetPass(1);
        }
    }

}
