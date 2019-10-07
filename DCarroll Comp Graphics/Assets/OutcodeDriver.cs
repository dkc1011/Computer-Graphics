using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcodeDriver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool LineClip(ref Vector2 v, ref Vector2 u)
    {
        Outcode inViewPort = new Outcode();
        Outcode vO = new Outcode(v);
        Outcode uO = new Outcode(u);

        //Detect Trivial Acceptance
        if((vO + uO) == inViewPort)
        {
            return true;
        }

        //Detect Trivial Rejection
        if((vO + uO) != inViewPort)
        {
            return false;
        }

        //If vO is in the viewport, invert the points and check again.
        if(vO == inViewPort)
        {
            return LineClip(ref u, ref v);
        }

        Vector2 v1;

        if (vO.up)
        {
            v1 = v;

            v1 = Intercept(u, v, 0);
            return false;
        }
        if (vO.down)
        {
            v1 = v;

            v1 = Intercept(u, v, 1);
            return false;
        }
        if (vO.left)
        {
            v1 = v;

            v1 = Intercept(u, v, 2);
            return false;
        }
        if(vO.right)
        {
             v1 = v;
            v1 = Intercept(u, v, 3);
            return false;
        }
    }

    private static Vector2 Intercept(Vector2 p1, Vector2 p2, int v2)
    {
        //Equation of a line
        // y2 - y1 m(x - x1)  or x - x1 = (1/m)(y-y1)

        /*
        
        m = Slope of the Line
        Top     y = 1       x= x1 + (1/m)(1-y1) 
        Bottom  y = -1      x = x1 + (1/m)(-1-y1)
        Left    x = -1      y = y1 + m(-1 - x1)
        Right   x = 1       y = y1 + m(1 + x1)
          
        */
        float slope = (p2.y - p1.y) / (p2.x - p1.x);

        if (v2 == 0)
        {
            return new Vector2(p1.x + (1 / slope) * (1 - p1.y), 1);
        }

        if (v2 == 1)
        {
            return new Vector2(p1.x + (1 / slope) * (-1 - p1.y), -1);
        }

        if (v2 == 3)
        {
            return new Vector2(-1, p1.y +  slope * (-1 - p1.x));
        }

        return new Vector2(1, p1.y + slope * (1 - p1.x));
    }
}
