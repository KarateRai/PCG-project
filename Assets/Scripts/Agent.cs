using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //Private
    private Dictionary<string, Vector3Int> directions = new Dictionary<string, Vector3Int>();
    private AgentStopCondition stopCondition;
    private float changeDirection = 0.05f;
    private float addObject = 0.05f;
    private int items;
    private float activeTime;
    private Vector3Int move;
    //Agent current XY location on grid
    private int x, y;

    //Public
    public Node currentNode;
    public NodeGrid grid;

    private void Start()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.z;
        currentNode = grid[x, y];
        directions.Add("UP", new Vector3Int(0, 0, 1));
        directions.Add("RIGHT", new Vector3Int(1, 0, 0));
        directions.Add("DOWN", new Vector3Int(0, 0, -1));
        directions.Add("LEFT", new Vector3Int(-1, 0, 0));
        RandomDirection();
    }
    // Update is called once per frame
    void Update()
    {
        //Stop condition
        switch (stopCondition)
        {
            case AgentStopCondition.ItemsPlaces:
                if (items == 0)
                {
                    StopAgent();
                }
                break;
            case AgentStopCondition.Time:
                if (activeTime <= 0)
                {
                    StopAgent();
                }
                else
                {
                    activeTime -= Time.deltaTime;
                }
                break;
            case AgentStopCondition.Both:
                if (activeTime <= 0 || items == 0)
                {
                    StopAgent();
                }
                break;
        }

        Move();
        currentNode = grid[(int)transform.position.x, (int)transform.position.z];

        //Check if we should change direction
        if (Random.value < changeDirection)
            ChangeDirection();
        else
            changeDirection += 0.05f;

        if (Random.value < addObject)
        {
            AddObject();
        }
        else
            addObject += 0.05f;
    }

    private void StopAgent()
    {
        Destroy(this);
    }

    private void AddObject()
    {
        //First we check so the current node is viable for an object and if not, we just increase posibility of adding object
        if (currentNode.Alive != true)
        {
            addObject += 0.1f;
            return;
        }

        //There is a 50% base chance to add item/enemy
        //Height of node affects if it has a bigger chance of being an item or enemy
        if (Random.value > (0.5f + currentNode.Height / 10f))
        {
            //Add item
            currentNode.SetNodeObject(NodeObject.Item);
        }
        else
        {
            //Add enemy
            currentNode.SetNodeObject(NodeObject.Enemy);
        }
        addObject = 0f;
        if (items > 0)
        {
            items--;
        }
    }

    private void ChangeDirection()
    {
        //If we are moving up or down we want to move left or right.
        if (move == directions["UP"] || move == directions["DOWN"])
        {
            if (Random.value > 0.5f)
                move = directions["RIGHT"];
            else 
                move = directions["LEFT"];
        }
        //Else we are moving right or left.
        else
        {
            if (Random.value > 0.5f)
            {
                move = directions["DOWN"];
            }
            else
            {
                move = directions["UP"];
            }
        }
        //Validate movement
        if (grid[x + move.x, y + move.z] == null)
        {
            //Can't move here, find new direction
            changeDirection += 0.05f;
        }
        else
            changeDirection = 0f;
    }

    private void RandomDirection()
    {
        int dir = Random.Range(1, 5);
        switch (dir)
        {
            case 1:
                move = directions["UP"];
                break;
            case 2:
                move = directions["RIGHT"];
                break;
            case 3:
                move = directions["DOWN"];
                break;
            case 4:
                move = directions["LEFT"];
                break;
        }
        if (grid[x + move.x, y + move.z] == null)
        {
            //Can't move here, find new direction
            RandomDirection();
        }
    }

    private void Move()
    {
        if (grid[x + move.x,y + move.z] == null)
        {
            changeDirection += 0.05f;
        }
        else
        {
            transform.position += move;
        }
        x = (int)transform.position.x;
        y = (int)transform.position.z;
    }

    public void SetStopCondition(AgentStopCondition stopCondition, int items, float time)
    {
        this.stopCondition = stopCondition;
        this.items = items;
        this.activeTime = time;
    }
}
