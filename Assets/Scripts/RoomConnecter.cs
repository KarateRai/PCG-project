using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnecter
{
    NodeGrid grid;
    Node nodeInRoom;
    List<Room> roomsOnMap;
    List<Vector2> checkedDirections;
    //0 == up
    //1 == right
    //2 == down
    //3 == left
    Vector2[] directions = { new Vector2(0,1), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1, 0) };
    Vector2 dir;
   public RoomConnecter()
    {
        checkedDirections = new List<Vector2>();
    }

    //Change name of method?
    public void InitializeConnector(NodeGrid grid, List<Room> roomsOnMap)
    {
        //Set grid
        this.grid = grid;

        //Set rooms on map
        this.roomsOnMap = roomsOnMap;
        for (int k = 0; k < this.roomsOnMap.Count; k++)
        {
            for (int i = 0; i < this.roomsOnMap[k].nodesInRoom.Count; i++)
            {
                if (this.roomsOnMap[k].nodesInRoom[i].room == null)
                {
                    Debug.Log("ROOM IS NULL");
                }
            }
        }
        ConnectRooms();
    }

    public void ConnectRooms()
    {
        //Make private or set checks for variables.

        //Take random room
        for (int r = 0; r < roomsOnMap.Count; r = Random.Range(0, roomsOnMap.Count))
        {
            //Take a random position(node) in room
            int roomIndex = Random.Range(0, roomsOnMap[r].nodesInRoom.Count);
            nodeInRoom = roomsOnMap[r].nodesInRoom[roomIndex];

            if (nodeInRoom.room == null)
            {
                Debug.Log("Error found in ConnectRooms 47");
                Debug.Log(r);
            }
            //Clear checked directions for new room
            if (checkedDirections != null)
            {
                checkedDirections.Clear();
            }
            if (roomsOnMap[r] == null)
                Debug.Log("Room is null");
            //Get a direction to look in
            SetDirection();
            //Look in said direction
            CheckDirection();
        }
    }

    void CheckDirection()
    {
        int distance = 1;
        for (int i = 1; i < grid.size; i++)
        {
            if (grid[nodeInRoom.X + (int)dir.x * i, nodeInRoom.Y + (int)dir.y * i] == null)
            {
                //We did not find any new room in direction, so look in another
                checkedDirections.Add(dir);
                SetDirection();
                CheckDirection();
            }
            //TODO:This gets null reference from time to time, find out what the problem is
            distance++;
            if (grid == null || nodeInRoom == null || dir == null)
            {
                //We break
                Debug.Log("Något gick fel");
                int k = 1;
            }
            if (grid[nodeInRoom.X + (int)dir.x * i, nodeInRoom.Y + (int)dir.y * i].room == null)
            {
                continue;
            }
            if (grid[nodeInRoom.X + (int)dir.x * i, nodeInRoom.Y + (int)dir.y * i].room != nodeInRoom.room)
            {
                //Found other room, connecting rooms and moving to next unconnected room.
                BuildConnection(nodeInRoom.room, distance);
                return;
            }
        }
    }

    void SetDirection()
    {
        if (checkedDirections.Count == 4)
        {
            Debug.Log("No directions to check");

        }
        int dirIndex = Random.Range(0, directions.Length);
        if (dir == directions[dirIndex] || checkedDirections.Contains(directions[dirIndex]))
        {
            SetDirection();
        }
        else
        {
            dir = directions[dirIndex];
        }
    }

    void BuildConnection(Room startRoom, int distance)
    {
        Node currentNode;
        for (int i = 0; i < distance; i++)
        {
            currentNode = grid[nodeInRoom.X + (int)dir.x * i, nodeInRoom.Y + (int)dir.y * i];

            if (currentNode.Alive == false)
            {
                //Add to room
                if (startRoom == null)
                {

                }
                startRoom.AddNode(currentNode);
                currentNode.room = startRoom;
                currentNode.SetShouldChange(true);
                currentNode.Change();
                currentNode.SetNodeObject(NodeObject.Pathway);
            }

            if (roomsOnMap.Contains(currentNode.room))
            {
                roomsOnMap.Remove(currentNode.room);
            }
        }

    }
}
