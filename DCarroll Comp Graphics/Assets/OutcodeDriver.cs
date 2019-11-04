using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcodeDriver : MonoBehaviour
{
    //Create texture on which to draw line -- size can vary, should be square for cube sake
    Texture2D tex;


    Vector2 start;
    Vector2 finish;

    //CUBE INITIALIZATION
    //Initialize the cube
    Vector3[] cube = new Vector3[8];

    int rotationAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        tex = new Texture2D(500, 500);
        GetComponent<Renderer>().material.mainTexture = tex;

        //Starting points
        Vector2 point1 = new Vector2(.5f, 2f);
        Vector2 point2 = new Vector2(.3f, .1f);

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

        cube[0] = new Vector3(1, 1, 1);
        cube[1] = new Vector3(-1, 1, 1);
        cube[2] = new Vector3(-1, -1, 1);
        cube[3] = new Vector3(1, -1, 1);
        cube[4] = new Vector3(1, 1, -1);
        cube[5] = new Vector3(-1, 1, -1);
        cube[6] = new Vector3(-1, -1, -1);
        cube[7] = new Vector3(1, -1, -1);

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate the Cube
        Vector3 startingAxis = new Vector3(15, 5, 5);
        startingAxis.Normalize();

        if(rotationAngle < 360)
        {
            rotationAngle++;
        }
        else
        {
            rotationAngle = 0;
        }

        Quaternion rotation = Quaternion.AngleAxis(rotationAngle, startingAxis);
        Matrix4x4 rotationMatrix =
            Matrix4x4.TRS(new Vector3(0, 0, 0),
                            rotation,
                            Vector3.one);

        Vector3[] imageAfterRotation =
            MatrixTransform(cube, rotationMatrix);


        //Scale the Cube
        Matrix4x4 scaleMatrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(2, 3, 4));


        Vector3[] ImageAfterScaling = MatrixTransform(imageAfterRotation, scaleMatrix);



        //Translate the Cube
        Matrix4x4 translationMatrix = Matrix4x4.TRS(new Vector3(2, 4, -4), Quaternion.identity, new Vector3(1, 1, 1));


        Vector3[] ImageAfterTranslation = MatrixTransform(ImageAfterScaling, translationMatrix);


        //Combine the Matrices

        Matrix4x4 singleMatrixOfTransformations = translationMatrix * scaleMatrix * rotationMatrix;

        Vector3[] ImageAfterTransformations = MatrixTransform(cube, singleMatrixOfTransformations);

        //Viewing Matrix
        Vector3 CameraPos = new Vector3(0, 0, 55);
        Vector3 CameraLookRotation = new Vector3(0, 0, 0);
        Vector3 CameraUp = new Vector3(0, 1, 0);

        Vector3 lookRotationDirection = CameraLookRotation - CameraPos;
        Quaternion cameraRotation = Quaternion.LookRotation(lookRotationDirection.normalized, CameraUp.normalized);

        Matrix4x4 viewingMatrix = Matrix4x4.TRS(-CameraPos, cameraRotation, Vector3.one);


        Vector3[] ImageAfterViewing = MatrixTransform(ImageAfterTransformations, viewingMatrix);


        Matrix4x4 projectionMatrix = Matrix4x4.Perspective(45, 1.6f, 1, 1000);


        Vector3[] ImageAfterProjection = MatrixTransform(ImageAfterViewing, projectionMatrix);


        Matrix4x4 SuperMatrix = projectionMatrix * viewingMatrix * translationMatrix * scaleMatrix * rotationMatrix;


        Vector3[] imageAfterSuperMatrix = MatrixTransform(cube, SuperMatrix);


        Vector2[] finalPostDevisionImage = DivideByZ(imageAfterSuperMatrix);

        Destroy(tex);

        tex = new Texture2D(500, 500);
        GetComponent<Renderer>().material.mainTexture = tex;


        start = finalPostDevisionImage[0];
        finish = finalPostDevisionImage[1];


        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[0];
        finish = finalPostDevisionImage[4];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[0];
        finish = finalPostDevisionImage[3];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[1];
        finish = finalPostDevisionImage[2];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[1];
        finish = finalPostDevisionImage[5];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[2];
        finish = finalPostDevisionImage[3];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[2];
        finish = finalPostDevisionImage[6];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[3];
        finish = finalPostDevisionImage[7];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[4];
        finish = finalPostDevisionImage[7];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[4];
        finish = finalPostDevisionImage[5];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[5];
        finish = finalPostDevisionImage[6];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }

        start = finalPostDevisionImage[6];
        finish = finalPostDevisionImage[7];

        if (LineClip(ref start, ref finish))
        {
            Plot(Breshenham(ConvertToScreenSpace(start), ConvertToScreenSpace(finish)));
        }


    }

    private void Plot(List<Vector2Int> list)
    {
        foreach(Vector2Int v in list)
        {
            Color color = Color.red;
            tex.SetPixel(v.x, v.y, color);

        }

        tex.Apply();
    }

    private Vector2Int ConvertToScreenSpace(Vector2 v)
    {
        int x = (int)Math.Round(((v.x + 1) / 2) * (Screen.width - 1));

        int y = (int)Math.Round(((1 - v.y) / 2) * (Screen.height - 1));

        return new Vector2Int(x, y);
    }

    private Vector2[] DivideByZ(Vector3[] finalImage)
    {
        List<Vector2> outputList = new List<Vector2>();

        foreach(Vector3 v in finalImage)
        {
            outputList.Add(new Vector2(v.x / v.z, v.y / v.z));
        }

        return outputList.ToArray();
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

    private Vector3[] MatrixTransform(
    Vector3[] meshVertices,
    Matrix4x4 transformMatrix)
    {
        Vector3[] output = new Vector3[meshVertices.Length];
        for (int i = 0; i < meshVertices.Length; i++)
            output[i] = transformMatrix *
                new Vector4(
                meshVertices[i].x,
                meshVertices[i].y,
                meshVertices[i].z,
                    1);

        return output;
    }
}
