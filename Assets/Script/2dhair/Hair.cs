using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

class Hair
{
    static List<Joint> joints = new List<Joint>();
    public int thickness = 25;
    Point force = new Point();
    Point origin = new Point();

    public Point Origin { get { return origin; } set { origin = value; } }

    public Point Force { get { return force; } set { force = value; } }

    Hair(Point origin)
    {
        Origin = origin;

        Joint joint = null;
        int amount = 5;

        for (var i = 0; i < amount; i++)
        {
            joint = new Joint(joint, ((amount - i) / amount));
            joint.force = force;

            joints.Add(joint);
        }

        joints[0].successor = joints[1];
    }

    void Draw()
    {
        int length = joints.Count;
        var first = joints[0];

        // context.save();

        // context.beginPath();
        // context.strokeStyle = "rgb( 0, 0, 0 )";
        // context.fillStyle = options.color;
        // context.lineWidth = 2;
        // context.translate(origin.x, origin.y);

        // for (var i = 0; i < length; i++)
        // {
        //     joints[i].update();
        // }

        // context.moveTo(first.top.x, first.top.y);

        // bezier(joints, context, "top");
        // bezier(reverse(joints), context, "bottom");

        // var topBetween = Point.between(first, first.top);
        // var bottomBetween = Point.between(first, first.bottom);

        // context.arc(
        //     first.x,
        //     first.y,
        //     options.thickness,
        //     Math.atan2(bottomBetween.y, bottomBetween.x),
        //     Math.atan2(topBetween.y, topBetween.x),
        //     true
        // );

        // context.closePath();
        // context.fill();
        // context.stroke();

        // context.restore();




    }
}
