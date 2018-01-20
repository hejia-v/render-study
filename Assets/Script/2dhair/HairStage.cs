using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class HairStage : MonoBehaviour
{
    public int ellipseSegments = 60;
    private VectorLine bar;
    public float lineWidth = 3;
    public float radius = 100.0f;

    public static Camera cam;
    public GameObject faceCursor;

    void Start()
    {
        cam = Camera.main;
        InitHair();
        //InitFace();
        //DrawFace();
    }

    void InitHair()
    {

    }

    void InitFace()
    {
        bar = new VectorLine("TotalBar", new List<Vector2>(ellipseSegments + 1), null, lineWidth , LineType.Continuous, Joins.Fill);
        bar.color = Color.black;
    }

    void DrawFace()
    {
        Vector3 pos = faceCursor.transform.position;
        bar.MakeArc(pos, radius, radius, -90.0f, 90.0f);
        bar.Draw();
    }

    void OnGUI()
    {
        //DrawFace();
        Vector3 pos = faceCursor.transform.position;
        DrawCircle(pos.x, pos.y, pos.z, radius, 60);
    }

    void Update()
    {
    }

    void DrawCircle(float x, float y, float z, float r, float accuracy)
    {
        GL.PushMatrix();
        //绘制2D图像    
        GL.LoadOrtho();

        float stride = r * accuracy;
        float size = 1 / accuracy;
        float x1 = x, x2 = x, y1 = 0, y2 = 0;
        float x3 = x, x4 = x, y3 = 0, y4 = 0;

        double squareDe;
        squareDe = r * r - Math.Pow(x - x1, 2);
        squareDe = squareDe > 0 ? squareDe : 0;
        y1 = (float)(y + Math.Sqrt(squareDe));
        squareDe = r * r - Math.Pow(x - x1, 2);
        squareDe = squareDe > 0 ? squareDe : 0;
        y2 = (float)(y - Math.Sqrt(squareDe));
        for (int i = 0; i < size; i++)
        {
            x3 = x1 + stride;
            x4 = x2 - stride;
            squareDe = r * r - Math.Pow(x - x3, 2);
            squareDe = squareDe > 0 ? squareDe : 0;
            y3 = (float)(y + Math.Sqrt(squareDe));
            squareDe = r * r - Math.Pow(x - x4, 2);
            squareDe = squareDe > 0 ? squareDe : 0;
            y4 = (float)(y - Math.Sqrt(squareDe));

            //绘制线段
            GL.Begin(GL.LINES);
            GL.Color(Color.blue);
            GL.Vertex(new Vector3(x1 / Screen.width, y1 / Screen.height, z));
            GL.Vertex(new Vector3(x3 / Screen.width, y3 / Screen.height, z));
            GL.End();
            GL.Begin(GL.LINES);
            GL.Color(Color.blue);
            GL.Vertex(new Vector3(x2 / Screen.width, y1 / Screen.height, z));
            GL.Vertex(new Vector3(x4 / Screen.width, y3 / Screen.height, z));
            GL.End();
            GL.Begin(GL.LINES);
            GL.Color(Color.blue);
            GL.Vertex(new Vector3(x1 / Screen.width, y2 / Screen.height, z));
            GL.Vertex(new Vector3(x3 / Screen.width, y4 / Screen.height, z));
            GL.End();
            GL.Begin(GL.LINES);
            GL.Color(Color.blue);
            GL.Vertex(new Vector3(x2 / Screen.width, y2 / Screen.height, z));
            GL.Vertex(new Vector3(x4 / Screen.width, y4 / Screen.height, z));
            GL.End();

            x1 = x3;
            x2 = x4;
            y1 = y3;
            y2 = y4;
        }
        GL.PopMatrix();
    }
}
