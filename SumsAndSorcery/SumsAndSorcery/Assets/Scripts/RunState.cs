using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNodeData
{
    public int index;
    public int row;
    public int column;

    public List<int> connections = new List<int>();

    public string encounterType = "Combat"; //maybe make an enum later for all the types

    public bool completed;
}

public static class RunState
{
    //the number of nodes on the map
    public static readonly int[] NodesPerRow =
    {
        3, 4, 4, 3, 4,
        4, 3, 4, 3, 4, 1
    };

    public static int TotalRows => NodesPerRow.Length;

    public static int TotalNodes
    {
        get
        {
            int total = 0;

            foreach (int count in NodesPerRow)
                total += count;

            return total;
        }
    }

    public static List<MapNodeData> Nodes { get; private set; }
        = new List<MapNodeData>();

    public static int CurrentRow { get; private set; } = 0;

    public static int CurrentNodeIndex { get; private set; } = -1;

    public static bool HasRun => Nodes.Count == TotalNodes;

    public static bool RunCompleted { get; private set; }

    //Starts a new run
    public static void StartNewRun()
    {
        Nodes.Clear(); //clear old map

        CurrentRow = 0;  //reset old info variables
        CurrentNodeIndex = -1;
        RunCompleted = false;

        int index = 0; //reset the index

        //create all the map nodes <--- ONLY MAKES COMBAT NODES AS OF NOW (and the boss one)
        for (int row = 0; row < TotalRows; row++)
        {
            for (int col = 0; col < NodesPerRow[row]; col++)
            {
                MapNodeData node = new MapNodeData();

                node.index = index;
                node.row = row;
                node.column = col;

                //Makes the last node a boss encounter
                node.encounterType =
                    row == TotalRows - 1 ? "Boss" : "Combat";

                Nodes.Add(node);

                index++;
            }
        }

        GenerateConnections(); //Connects the nodes together
    }

    //connects all the nodes together
    private static void GenerateConnections()
    {
        int currentStart = 0;

        for (int row = 0; row < TotalRows - 1; row++)
        {
            int currentCount = NodesPerRow[row];
            int nextCount = NodesPerRow[row + 1];

            int nextStart = currentStart + currentCount;

            int offset = Random.Range(0, nextCount);

            //Generate the connection pattern
            for (int i = 0; i < currentCount; i++)
            {
                int targetCol;

                if (currentCount <= nextCount)
                {
                    //spread the next row of nodes across the current row of nodes
                    int sourceCol = i;
                    int targetIndex = Mathf.FloorToInt((i + 1) * nextCount / (float)currentCount) - 1;

                    targetCol = (targetIndex + offset) % nextCount;

                    //give a node its connection
                    Nodes[currentStart + sourceCol].connections.Add(nextStart + targetCol);
                }
                else
                {
                    //if there were more nodes in the current row than the next row do this
                    targetCol = Mathf.FloorToInt(
                        i * nextCount / (float)currentCount
                    );

                    targetCol = (targetCol + offset) % nextCount;

                    Nodes[currentStart + i]
                        .connections.Add(nextStart + targetCol);
                }
            }

            //some nodes get extra branches
            for (int i = 0; i < currentCount; i++)
            {
                if (Random.value > 0.5f)
                    continue;

                MapNodeData source = Nodes[currentStart + i];

                if (source.connections.Count >= 2)
                    continue;

                int targetCol = Random.Range(0, nextCount);
                int targetIndex = nextStart + targetCol;

                if (!source.connections.Contains(targetIndex))
                    source.connections.Add(targetIndex);
            }

            currentStart = nextStart;
        }
    }

    //checks if you can enter a node
    public static bool CanEnterNode(int index)
    {
        if (index < 0 || index >= Nodes.Count)
            return false;

        MapNodeData node = Nodes[index];

        if (RunCompleted || node.completed)
            return false;

        if (node.row != CurrentRow)
            return false;

        //if the starting row of nodes:
        if (CurrentRow == 0)
            return true;

        //check the connections of completed nodes
        foreach (MapNodeData previous in Nodes)
        {
            if (previous.completed &&
                previous.connections.Contains(index))
            {
                return true;
            }
        }

        return false;
    }

    //Enters a node (if it can be entered)
    public static bool EnterNode(int index)
    {
        if (!CanEnterNode(index))
            return false;

        CurrentNodeIndex = index;

        return true;
    }

    //Finishes the current node 
    public static void CompleteCurrentNode()
    {
        if (CurrentNodeIndex < 0)
            return;

        bool wasBoss =
            Nodes[CurrentNodeIndex].encounterType == "Boss";

        Nodes[CurrentNodeIndex].completed = true;

        CurrentNodeIndex = -1;

        if (wasBoss) //if it was a boss encounter, finish the run
        {
            RunCompleted = true;
        }
        else
        {
            CurrentRow++; //move to the next row of nodes otherwise
        }
    }
}
