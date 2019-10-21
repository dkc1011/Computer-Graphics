using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcodeDriver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Create texture on which to draw line -- size can vary, should be square for cube sake
        Texture2D tex = new Texture2D(256, 256);
        GameObject cube = GetComponent<GameObject>();
        GetComponent<Renderer>().material.mainTexture = tex;

        //Starting points
        Vector2 point1 = new Vector2(.5f, 2f);
        Vector2 point2 = new Vector2(.3f, .1f);

        //Apply outcodes and clip line
        Outcode a = new Outcode(point1);
        Outcode b = new Outcode(point2);

        // All are false
        Outcode inViewPort = new Outcode();

        if ((a == inViewPort) && (b == inViewPort))
        {
            Debug.Log("Trivially Accept");
        }

        if (a * b != inViewPort)
        {
            Debug.Log("Trivially Rejected");
        }

        if ((a + b) == inViewPort)
        {
            Debug.Log("Trivially Accept");
        }

        if (!LineClip(ref point1, ref point2))
        {
            Debug.Log(point1.x + "  ,  " + point1.y);
            Debug.Log(point2.x + "  ,  " + point2.y);
        }

        //Create start of line and end of line
        Vector2Int pixelPoint1 = new Vector2Int(17, 28);
        Vector2Int pixelPoint2 = new Vector2Int(-1, 3);

        //Pass start and end point through Breshenham's algorithm to create all other points between start and end point
        List<Vector2Int> list = Breshenham(pixelPoint1, pixelPoint2);

        //Print all points to console 
        foreach (Vector2 v in list)
        {
            Debug.Log(v.x + "  ,  " + v.y);

        }

        //Apply points as pixels to the texture
        for(int i = 0; i < tex.height; i++)
        {
            for(int j = 0; j < tex.width; j++)
            {
                Color color = Color.red;
                int drawX = (int)list[j].x;
                int drawY = (int)list[i].y;
                tex.SetPixel(drawX, drawY, color);
            }
        }

        //Apply the texture to the cube
        tex.Apply();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector2Int> Breshenham(Vector2Int start, Vector2Int finish)
    {
        //Start = first point in line, Finish = last point in line

        // dx = start x coordinate - finish x coordinate
        int dx = finish.x - start.x;

        // If dx is negative, invert the first and final points so that it is positive
        if(dx < 0)
        {
            return Breshenham(finish, start);
        }

        // dy = start y cooridnate - finish y cooridnate 
        int dy = finish.y - start.y;



        if(dy < 0)
        {
            return NegativeY(Breshenham(NegativeY(start), NegativeY(finish)));
        }

        if(dy > dx)
        {
            return SwapXY(Breshenham(SwapXY(start), SwapXY(finish)));
        }

        int a = 2 *  dy;
        int b = 2 * (dy - dx);
        int p = 2 * dy - dx;

        List<Vector2Int> outputList = new List<Vector2Int>();

        int y = start.y;


        //Populates the return list with Vector2Int objects that act as each point in the line
        for(int x = start.x; x <= finish.x; x++)
        {
            outputList.Add(new Vector2Int(x, y));

            if(p > 0)
            {
                y++;
                p += b;
            }
            else
            {
                p += a;
            }
        }

        return outputList;

    }

    public List<Vector2Int> NegativeY(List<Vector2Int> list)
    {
        List<Vector2Int> outputList = new List<Vector2Int>();

        foreach(Vector2Int v in list)
        {
            outputList.Add(NegativeY(v));
        }

        return outputList;
    }

    public Vector2Int NegativeY(Vector2Int point)
    {
        return new Vector2Int(point.x, -point.y);
    }

    public List<Vector2Int> SwapXY(List<Vector2Int> list)
    {
        List<Vector2Int> outputList = new List<Vector2Int>();

        foreach(Vector2Int v in list)
        {
            outputList.Add(SwapXY(v));
        }

        return outputList;
    }

    public Vector2Int SwapXY(Vector2Int point)
    {
        return new Vector2Int(point.y, point.x);
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

        v1 = v;

        v1 = Intercept(u, v, 3);
        return false;
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
