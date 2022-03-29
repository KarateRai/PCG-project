using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid
{
    public Node[,] nodegrid;
    public int size;
    public float nodeSize;
    
    public NodeGrid(int size)
    {
        nodegrid = new Node[size,size];
        this.size = size;
        CreateNodeGrid(size);
    }
    public NodeGrid(int size, float nodeSize)
    {
        nodegrid = new Node[size, size];
        this.size = size;
        this.nodeSize = nodeSize;
        CreateNodeGrid(size, nodeSize);
    }

    public Node this[int X, int Y]
    {
        get
        {
            if (X >= 0 && Y >= 0 && X < size && Y < size)
            {
                return nodegrid[X, Y];
            }
            else
            {
                return null;
            }
            
        }
    }

    private void CreateNodeGrid(int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                nodegrid[x, y] = new Node(x, y);
            }
        }
    }

    private void CreateNodeGrid(int size, float nodeSize)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                nodegrid[x, y] = new Node(x, y, nodeSize);
            }
        }
    }

    public void Clear()
    {
        nodegrid = new Node[size, size];
    }
}
