using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    None = -1,
    Dead = 0,
    Alive = 1,
    //Temp types for CA, will be changed later
}

public enum NeighbourhoodType
{
    Moore,
    Neumann
}

public enum NodeObject
{
    Enemy,
    Item,
    Pathway,
    None
}

public enum AgentStopCondition
{
    ItemsPlaces,
    Time,
    Both
}
