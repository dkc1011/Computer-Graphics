using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcode
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    public Outcode()
    {
        up = false;
        down = false;
        right = false;
        left = false;
    }

    public Outcode(Vector2 v)
    {
        up = v.y > 1;
        down = v.y < -1;
        left = v.x < -1;
        right = v.x > 1;
    }

    public Outcode(bool Up, bool Down, bool Left, bool Right)
    {
        up = Up;
        down = Down;
        left = Left;
        right = Right;
    }

    //Logical EQUALS
    public static bool operator ==(Outcode a, Outcode b)
    {
        return (a.up == b.up) && (a.down == b.down) && (a.right == b.right) && (a.left == b.left);
    }

    //Logical NOT EQUALS
    public static bool operator !=(Outcode a, Outcode b)
    {
        return !(a == b);
    }

    //Logical AND
    public static Outcode operator *(Outcode a, Outcode b)
    {
        return new Outcode(a.up && b.up, a.down && b.down, a.left && b.left, a.right && b.right);
    }

    //Logical OR
    public static Outcode operator +(Outcode a, Outcode b)
    {
        return new Outcode(a.up || b.up, a.down || b.down, a.left || b.left, a.right || b.right);
    }

}
