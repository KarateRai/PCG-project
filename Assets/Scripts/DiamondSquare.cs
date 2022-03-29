using UnityEngine;

public static class DiamondSquare
{
    //TODO:
    //Offset depending on size of tiles if needed
    public static Vector3[,] DoDiamondSquare(int size, float range = 0)
    {
       
        if (range == 0)
            range = 1;

        Vector3[,] vArray = new Vector3[size, size];
        SetupVectorArray(size, vArray);
        InitializeCorners(range, vArray, size);

        int halfSize;
        //As long as we can split the size in two (i > 1) we will do so
        for (int i = size; i > 1; i/=2)
        {
            halfSize = i / 2;

            //Mid point
            for (int x = halfSize; x < size; x += i)
            {
                for (int y = halfSize; y < size; y += i)
                {
                    vArray[x, y].y = (
                        (
                        vArray[x-halfSize, y-halfSize].y + //Topleft corner of iteration
                        vArray[x-halfSize, y+halfSize].y + //Bottomleft corner of iteration
                        vArray[x+halfSize, y-halfSize].y + //Topright corner of iteration
                        vArray[x+halfSize, y+halfSize].y) //Bottomright corner of iteration
                        / 4 + Random.Range(-range, range)
                        );
                }
            }

            //Square point
            for (int x = 0; x < size; x+=halfSize)
            {
                for (int y = (x+halfSize) % i; y < size; y+=size)
                {
                    
                    float average =
                        vArray[(x-halfSize+size) % size, y].y + //Left side
                        vArray[(x + halfSize) % size, y].y + //Right side
                        vArray[x, (y + halfSize) % size].y + //Bottom
                        vArray[x, (y + halfSize + size) % size].y; //Top
                    average /= 4f;
                    average += Random.Range(-range, range);
                    //Set value of midpoint in diamond
                    vArray[x, y].y = average;

                    if (x == 0) vArray[size-1, y].y = average;
                    if (y == 0) vArray[x, size-1].y = average;
                }
            }
        }
        //int tempSize = size;

        //Diamond(vArray, tempSize / 2, 5);
        //Vector3 v1, v2, v3, v4;

        return vArray;
    }

    private static void SetupVectorArray(int size, Vector3[,] vArray)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                vArray[x, y] = new Vector3(x, 0, y);
            }
        }
    }

    //We initialize all corners to the same value, if you want to have different values on your corners take away comments and comment out the corners part
    private static void InitializeCorners(float range, Vector3[,] vArray, int size)
    {
        //Comment out this if you don't want symmetrical corners
        float corners = Random.Range(-range, range);

        for (int i = 0; i < size; i += size - 1)
        {
            for (int j = 0; j < size; j += size - 1)
            {
                //Comment out this if you don't want symmetrical corners
                vArray[i, j].y = corners;
                //vArray[i, j].z = Random.Range(-range, range);
            }
        }
    }

    //private static void Diamond(Vector3[,] vArray, int size, int depth)
    //{
    //    if (depth == 0)
    //        return;
    //    vArray[size, size].z = Random.value;
    //    Square(vArray, size, depth - 1);
    //}

    //private static void Square(Vector3[,] vArray, int size, int depth)
    //{
    //    if (depth == 0)
    //        return;
    //    for (int x = 0; x < size; x += size-1)
    //    {
    //        for (int y = 0; y < size; y += size-1)
    //        {
    //            vArray[x,y].z
    //        }
    //    }

    //    Diamond(vArray, size / 2, depth - 1);
    //}
}
