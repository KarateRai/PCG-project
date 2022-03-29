using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public List<Node> nodesInRoom = new List<Node>();
    public bool isVisited = false;
    public bool isConnected = false;
    int index;

    public Room(int index)
    {
        this.index = index;
    }
    public void AddNode(Node node)
    {
        nodesInRoom.Add(node);
    }
}
