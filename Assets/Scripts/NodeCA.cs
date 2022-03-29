using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeCA
{
    public static NeighbourhoodType neighbourhoodType;
    private static int M = 0;
    private static int T = 0;
    /// <summary>
    /// Checks the CA neighbourhood around any node
    /// </summary>
    /// <param name="node">The node to check</param>
    /// <param name="size">The neighbourhood size</param>
    
    public static void StartCA(NodeGrid grid, int N, int neighbourhood, int threshold)
    {
        for (int i = 0; i < N; i++)
        {
            for (int x = 0; x < grid.size; x++)
            {
                for (int y = 0; y < grid.size; y++)
                {
                    CheckNeighbours(grid[x, y], grid, neighbourhood, threshold);
                }
            }
            UpdateGrid(grid);
        }
    }
    private static void CheckNeighbours(Node node, NodeGrid grid, int neighbourhood, int threshold)
    {
        M = 0;
        T = threshold;
        switch (neighbourhoodType)
        {
            case NeighbourhoodType.Moore:
                CheckMooreNeighbourhood(node, grid, neighbourhood);
                break;
            case NeighbourhoodType.Neumann:
                CheckNeumannNeighbourhood(node, grid, neighbourhood);
                break;
        }
        ChangeNodeState(node);
        M = 0;
    }

    private static void CheckNeumannNeighbourhood(Node node, NodeGrid grid, int neighbourhood)
    {
        for (int i = 0; i < neighbourhood; i++)
        {
            if (grid[node.X - 1, node.Y] != null && grid[node.X - 1, node.Y].Alive)
            {
                M++;
            }
            if (grid[node.X + 1, node.Y] != null && grid[node.X + 1, node.Y].Alive)
            {
                M++;
            }
            if (grid[node.X, node.Y - 1] != null && grid[node.X, node.Y - 1].Alive)
            {
                M++;
            }
            if (grid[node.X, node.Y + 1] != null && grid[node.X, node.Y + 1].Alive)
            {
                M++;
            }
        }
    }

    private static void CheckMooreNeighbourhood(Node node, NodeGrid grid, int neighbourhood)
    {
        for (int i = -neighbourhood; i <= neighbourhood; i++)
        {
            for (int j = -neighbourhood; j <= neighbourhood; j++)
            {
                if (grid[node.X + i, node.Y + j] == null ||
                    grid[node.X + i, node.Y + j] == node)
                {
                    continue;
                }
                else if (grid[node.X + i, node.Y + j].Alive)
                    M++;
            }
        }
    }

    private static void ChangeNodeState(Node node)
    {
        if (M >= T)
        {
            node.SetShouldChange(true);
        }
        else
            node.SetShouldChange(false);
    }

    private static void UpdateGrid(NodeGrid grid)
    {
        for (int x = 0; x < grid.size; x++)
        {
            for (int y = 0; y < grid.size; y++)
            {
                grid[x, y].Change();
            }
        }
    }
}
