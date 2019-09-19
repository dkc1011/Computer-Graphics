using UnityEngine;
using System.Collections;
using System;

public class Transformations : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Initialize the cube
        Vector3[] cube = new Vector3[8];
        cube[0] = new Vector3(1, 1, 1);
        cube[1] = new Vector3(-1, 1, 1);
        cube[2] = new Vector3(-1, -1, 1);
        cube[3] = new Vector3(1, -1, 1);
        cube[4] = new Vector3(1, 1, -1);
        cube[5] = new Vector3(-1, 1, -1);
        cube[6] = new Vector3(-1, -1, -1);
        cube[7] = new Vector3(1, -1, -1);

        //Rotate the Cube
        Vector3 startingAxis = new Vector3(15, 5, 5);
        startingAxis.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(47, startingAxis);
        Matrix4x4 rotationMatrix =
            Matrix4x4.TRS(new Vector3(0,0,0),
                            rotation,
                            Vector3.one);
        printMatrix(rotationMatrix);

        Vector3[] imageAfterRotation =
            MatrixTransform(cube, rotationMatrix);
        printVerts(imageAfterRotation);


        //Scale the Cube
        Matrix4x4 scaleMatrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(15, 3, 5));

        printMatrix(scaleMatrix);


        Vector3[] ImageAfterScaling = MatrixTransform(imageAfterRotation, scaleMatrix);

        printVerts(ImageAfterScaling);


        //Translate the Cube
        Matrix4x4 translationMatrix = Matrix4x4.TRS(new Vector3(2, 4, -4), Quaternion.identity, new Vector3(1, 1, 1));

        printMatrix(translationMatrix);

        Vector3[] ImageAfterTranslation = MatrixTransform(ImageAfterScaling, translationMatrix);

        printVerts(ImageAfterTranslation);

        //Combine the Matrices

        Matrix4x4 singleMatrixOfTransformations = translationMatrix * scaleMatrix * rotationMatrix;

        printMatrix(singleMatrixOfTransformations);

        Vector3[] ImageAfterTransformations = MatrixTransform(cube, singleMatrixOfTransformations );

        printVerts(ImageAfterTransformations);


        //Viewing Matrix
        Vector3 CameraPos = new Vector3(17, 8, 55);
        Vector3 CameraLookRotation = new Vector3(5, 15, 5);
        Vector3 CameraUp = new Vector3(-4, 5, 15);

        Vector3 lookRotationDirection = CameraLookRotation - CameraPos;
        Quaternion cameraRotation = Quaternion.LookRotation(lookRotationDirection.normalized, CameraUp.normalized);

        Matrix4x4 viewingMatrix = Matrix4x4.TRS(new Vector3(0, 0, 0), cameraRotation, CameraPos);
        printMatrix(viewingMatrix);

        Vector3[] ImageAfterViewing = MatrixTransform(ImageAfterTransformations, viewingMatrix);
        printVerts(ImageAfterViewing);

    }

    private void printVerts(Vector3[] newImage)
    {
        for (int i = 0; i < newImage.Length; i++)
            print(newImage[i].x + " , " +
                newImage[i].y + " , " +
                newImage[i].z);

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

    private void printMatrix(Matrix4x4 matrix)
    {
        for (int i = 0; i < 4; i++)
            print(matrix.GetRow(i).ToString());
    }



    // Update is called once per frame
    void Update () {
	
	}
}
