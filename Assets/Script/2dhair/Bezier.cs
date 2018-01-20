using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Bezier
{
    static List<Point> anchors = new List<Point>();
    float time = 0;
    float speed = 0.1f;
    float pixels;
    public float x;
    public float y;
    bool pixelBased = false;
    float accuracy = 10;


    // This class is used to calculate any position in a bezier curve
    // based on the time. Time is a value in between 0 and 1. 0 representing
    // the start of the curve and 1 the end.
    //
    // The update() method can be used to increase the value of time. By default
    // the increase uses a relative value speed where 0.1 represents 10% of the curve.
    // You can also controll the the curve on a pixel based speed by passing true as
    // second parameter. This will allow to provide a speed such as 17 meaning 17 pixels
    // per update. Consider that this causes more calculation time for every
    // anchor added to the curve.
    // The third parameter, accuracy, is used to determine the accuracy of the pixel based
    // speed. This is needed since the calculation of the length is simply an approximation
    //
    // paramters:
    // speed: integer. between (excluding) 0 and 1 if pixelsBased is false else any value larger than 0
    // pixelBased: boolean. default = false
    // accuracy: integer. any value larger than 1. default = 10
    public Bezier(float speed, bool pixelBased, float accuracy)
    {
        anchors = new List<Point>();

        this.pixelBased = pixelBased;
        this.accuracy = accuracy;

        this.time = 0;
        this.speed = speed;
        this.pixels = this.speed;
    }

    public void addAnchor(Point point)
    {
        anchors.Add(point);

        if (pixelBased)
        {
            var storeTime = time;
            int i = 0;
            float accuracy = this.accuracy;
            float sum = 0;
            Point a = new Point();
            Point b;

            for (; i < accuracy; i++)
            {
                time = i / accuracy;
                calculate();

                if (i == 0)
                {
                    a = new Point(x, y);
                    continue;
                }

                b = a;
                b = new Point(x, y);

                sum += Point.distance(a, b);
            }

            time = storeTime;
            speed = Math.Min(1, pixels / sum);
        }
    }

    public void calculate()
    {
        float t = time;
        int length = anchors.Count;
        List<Point> points = new List<Point>();
        int i, j;

        for (i = 0; i < length; i++)
        {
            points[i] = new Point(anchors[i].x, anchors[i].y);
        }

        for (j = 1; j < length; ++j)
        {
            for (i = 0; i < length - j; ++i)
            {
                points[i].x = (1 - t) * points[i].x + t * points[i + 1].x;
                points[i].y = (1 - t) * points[i].y + t * points[i + 1].y;
            }
        }

        x = points[0].x;
        y = points[0].y;
    }

    public void update()
    {
        time += speed;

        if (time > 1)
        {
            time = 1;
        }

        calculate();
    }

    public bool isComplete()
    {
        return time >= 1;
    }

    public void setPercent(float value)
    {
        time = value;

        calculate();
    }

}
