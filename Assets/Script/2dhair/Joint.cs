using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class Joint : Point
{
    Joint predecessor;
    int index;
    public Point force = new Point();
    float distance = 0;
    Motion motion;
    public Joint successor;

    Point top;
    Point bottom;


    public Joint(Joint predecessor, int index)
    {
        float x, y;

        this.predecessor = predecessor;
        this.index = index;
        this.distance = Option.length;
        this.motion = new Motion(5, 2, 10);

        if (predecessor != null)
        {
            x = predecessor.x + this.distance;
            y = predecessor.y;
        }
        else
        {
            x = 0;
            y = 0;
        }

        this.x = x;
        this.y = y;

        this.top = new Point(this);
        this.bottom = new Point(this);
    }

    public void update()
    {

        float thickness = this.index * Option.thickness;
        Point between;

        if (successor != null)
        {
            between = Point.between(this, successor).normalize().multiply(thickness);

            this.top = between.clone().rotate(Math.PI / 2);
            this.bottom = between.clone().rotate(-Math.PI / 2);

            return;
        }

        if (predecessor == null)
            return;

        this.motion.update();

        Point useForce = this.force.clone().add(this.motion.getPosition());

        this.add(useForce);

        between = Point.between(predecessor, this).normalize();

        Point stretch = between.clone().multiply(Option.length);
        Point target = predecessor.clone().add(stretch);

        this.x = target.x;
        this.y = target.y;

        this.top = this.clone().add(between.clone().multiply(thickness).rotate(Math.PI / 2));
        this.bottom = this.clone().add(between.clone().multiply(thickness).rotate(-Math.PI / 2));
    }

}