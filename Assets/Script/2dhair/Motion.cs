using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion
{
    Point pointA = new Point();
    Point pointB = new Point();
    Point pointC = new Point();

    Point pointAB = new Point();
    Point pointBC = new Point();

    float speed = 0.1f;
    float radius = 10;
    float accuracy = 10;

    Bezier bezier;

    // This class makes a smooth motion while staying within a givin radius.
    //
    // parameters:
    // radius: The radius to which the motion is bound.
    // speed: The speed of the motion (for more detail see Bezier)
    // accuracy: The accuracy for the speed calculation (for more detail see Bezier)
    public Motion(float radius, float speed, float accuracy)
    {
        this.radius = radius;
        this.speed = speed;
        this.accuracy = accuracy;

        this.bezier = new Bezier(this.speed, true, this.accuracy);

        this.pointA = this.getRandomPoint();
        this.pointB = this.getRandomPoint();
        this.pointC = this.getRandomPoint();

        this.pointAB = this.getMiddle(this.pointA, this.pointB);
        this.pointBC = this.getMiddle(this.pointB, this.pointC);

        this.bezier.addAnchor(this.pointAB);
        this.bezier.addAnchor(this.pointB);
        this.bezier.addAnchor(this.pointBC);
    }

    public void update()
    {

        if (this.bezier.isComplete())
        {

            this.bezier = new Bezier(this.speed, true, this.accuracy);

            this.bezier.addAnchor(this.pointBC);
            this.bezier.addAnchor(this.pointC);

            this.pointA = this.pointB;
            this.pointB = this.pointC;
            this.pointC = this.getRandomPoint();

            this.pointAB = this.pointBC;
            this.pointBC = this.getMiddle(this.pointB, this.pointC);

            this.bezier.addAnchor(this.pointBC);

            this.bezier.calculate();

            return;
        }

        this.bezier.update();
    }

    Point getRandomPoint()
    {
        var angle = GRandom.Random() * Math.PI * 2;

        return new Point(
            Math.Cos(angle) * (0.1 + GRandom.Random() * 0.9) * this.radius,
            Math.Sin(angle) * (0.1 + GRandom.Random() * 0.9) * this.radius
        );
    }

    Point getMiddle(Point a, Point b)
    {
        return Point.add(a, b).divide(2);
    }

    public float getX()
    {
        return this.bezier.x;
    }

    public float getY()
    {
        return this.bezier.y;
    }

    public Point getPosition()
    {
        return new Point(this.bezier.x, this.bezier.y);
    }

}
