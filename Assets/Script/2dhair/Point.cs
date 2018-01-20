using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Point
{
    public float x;
    public float y;

    public Point()
    {
        this.x = 0;
        this.y = 0;
    }

    public Point(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Point(double x, double y)
    {
        this.x = (float)x;
        this.y = (float)y;
    }

    public Point(Point point)
    {
        this.x = point.x;
        this.y = point.y;
    }

    public Point add(Point point)
    {
        this.x += point.x;
        this.y += point.y;

        return this;
    }

    public Point subtract(Point point)
    {
        this.x -= point.x;
        this.y -= point.y;

        return this;
    }

    public Point multiply(float factor)
    {
        this.x *= factor;
        this.y *= factor;

        return this;
    }

    public Point divide(float factor)
    {
        this.x /= factor;
        this.y /= factor;

        return this;
    }

    public Point clone()
    {
        return new Point(this.x, this.y);
    }

    public float length()
    {
        return (float)Math.Sqrt(x * x + y * y);
    }

    public Point normalize()
    {
        return this.divide(this.length());
    }

    public Point set(Point point)
    {
        this.x = point.x;
        this.y = point.y;

        return this;
    }

    public Point rotate(float angle)
    {
        float cos = (float)Math.Cos(angle);
        float sin = (float)Math.Sin(angle);

        var x = this.x * cos - this.y * sin;
        var y = this.y * cos + this.x * sin;

        this.x = x;
        this.y = y;

        return this;
    }

    public Point rotate(double angle)
    {
        return rotate((float)angle);
    }

    public Point flip()
    {
        this.x *= -1;
        this.y *= -1;

        return this;
    }

    public static Point add(Point a, Point b)
    {
        return a.clone().add(b);
    }

    public static Point subtract(Point a, Point b)
    {
        return a.clone().subtract(b);
    }

    public static float distance(Point a, Point b)
    {
        var x = a.x - b.x;
        var y = a.y - b.y;

        return (float)Math.Sqrt(x * x + y * y);
    }

    public static Point between(Point a, Point b)
    {
        return new Point(b.x - a.x, b.y - a.y);
    }

}
