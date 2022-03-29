using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isChecked = false;
    /// <summary>
    /// This node will hold the information of one "tile" in the array we are creating
    /// </summary>

    public event Action<int, int, Color> OnChangeNodeColor;

    //X and Y position for the Node
    private int x;
    private int y;

    //Getters for the Node position
    public int X => x;
    public int Y => y;

    //Size of node
    private float nodeLength;
    private float nodeWidth;

    //Getters for nodesize
    public float NodeLength => nodeLength;
    public float NodeWidth => nodeWidth;

    //Height is used for DiamondSquare algorithm.
    private float height = 0;
    public float Height => height;

    private float objectQuality = 0;
    public float ObjectQuality { get { return objectQuality; } set { objectQuality = value; } }
    //Node type is used for presentation
    public NodeType nodeType;
    public NodeObject nodeObject = NodeObject.None;
    //Alive is used for CellularAutomata
    private bool alive = false;
    private bool change = false;
    public bool Alive => alive;

    public Room room;


    //TEST AREA
    public Material m;
    public Node(int x, int y, Material m)
    {
        this.m = m;
        this.x = x;
        this.y = y;
        nodeWidth = 1;
        nodeLength = 1;
        alive = UnityEngine.Random.value > 0.5 ? true : false;
    }

    public Node()
    {
        x = 0;
        y = 0;
        nodeWidth = 1;
        nodeLength = 1;
        alive = UnityEngine.Random.value > 0.5 ? true : false;
    }
    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
        nodeWidth = 1;
        nodeLength = 1;
        alive = UnityEngine.Random.value > 0.5 ? true : false;
    }

    public Node(int x, int y, float size)
    {
        this.x = x;
        this.y = y;
        this.nodeWidth = size;
        this.nodeLength = size;
        alive = UnityEngine.Random.value > 0.5 ? true : false;
    }

    public void SetShouldChange(bool status)
    {
        if (alive != status)
        {
            change = !change;
        }
    }
    public void Change()
    {
        if (change == true)
            alive = !alive;
        change = !change;
    }

    public void SetNodeObject(NodeObject nodeObject)
    {
        switch (nodeObject)
        {
            case NodeObject.Enemy:
                nodeObject = NodeObject.Enemy;
                OnChangeNodeColor?.Invoke(x, y, Color.black);
                break;
            case NodeObject.Item:
                nodeObject = NodeObject.Item;
                OnChangeNodeColor?.Invoke(x, y, Color.blue);
                break;
            case NodeObject.Pathway:
                nodeObject = NodeObject.Pathway;
                OnChangeNodeColor?.Invoke(x, y, Color.white);
                break;
        }
    }
}
