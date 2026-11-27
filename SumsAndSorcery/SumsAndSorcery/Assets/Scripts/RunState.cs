using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapNodeData
{
    //holds the index, row, and column of the node
    public int index;
    public int row;
    public int column;

    //holds the list of connections of the nodes
    public List<int> connections = new List<int>();

    public string encounterType = "Combat"; //maybe make an enum later for all the types

    //booleans for if the nodes has been completed, or if it has been removed
    public bool completed;
    public bool removed;
}

public static class RunState
{
    //the number of nodes on the map
    public static readonly int[] NodesPerRow =
    {
        2, 3, 4, 3, 4, 4, 3, 4, 3, 2, 1
    };

    //Get the total rows
    public static int TotalRows => NodesPerRow.Length;

    //gets the total number of nodes to be created
    public static int TotalNodes
    {
        get
        {
            int total = 0;

            foreach (int count in NodesPerRow)
                total += count;

            return total;
        }
    }//end TotalNodes

    //Make a new list of MapNodeData
    public static List<MapNodeData> Nodes { get; private set; } = new List<MapNodeData>();

    //Keep track of the current row and current node index
    public static int CurrentRow { get; private set; } = 0;
    public static int CurrentNodeIndex { get; private set; } = -1;

    public static bool HasRun => Nodes.Count == TotalNodes;

    //A boolean to keep track of if the run is done
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
        //loop through all the rows
        for (int row = 0; row < TotalRows; row++)
        {
            //go through each column in the row
            for (int column = 0; column < NodesPerRow[row]; column++)
            {
                //Create a new MapNodeData
                MapNodeData node = new MapNodeData();

                //set the index, row, and column
                node.index = index;
                node.row = row;
                node.column = column;

                //Makes the last node a boss encounter
                node.encounterType = row == TotalRows - 1 ? "Boss" : "Combat"; //need to update this later to randomize the rows

                //add the node
                Nodes.Add(node);

                //increase the index
                index++;
            }//end for
        }//end for

        GenerateConnections(); //Connects the nodes together
        RemoveUnconnectedNodes(); //gets rid of any nodes that had no connections
    }

    //connects all the nodes together
    private static void GenerateConnections()
    {
        int currentStart = 0;

        //go through each row
        for (int row = 0; row < TotalRows - 1; row++)
        {
            //get the number of nodes in this row and the number of nodes in the next row
            int currentRowCount = NodesPerRow[row];
            int nextRowCount = NodesPerRow[row + 1];

            ///get the index of the next row start
            int nextRowStart = currentStart + currentRowCount;

            //get the offset for generation
            int offset = Random.Range(0, nextRowCount);

            //go through each node in the current row
            for (int i = 0; i < currentRowCount; i++)
            {
                //create an in for the targeted column for the connection
                int targetColoumn;

                //if there are less or the same amount of nodes in this row as the next:
                if (currentRowCount <= nextRowCount)
                {
                    //spread the next row of nodes across the current row of nodes
                    int sourceCol = i;
                    int targetIndex = Mathf.FloorToInt((i + 1) * nextRowCount / (float)currentRowCount) - 1;

                    targetColoumn = (targetIndex + offset) % nextRowCount;

                    //give a node its connection
                    Nodes[currentStart + sourceCol].connections.Add(nextRowStart + targetColoumn);
                }
                else
                {
                    //if there were more nodes in the current row than the next row do this:
                    targetColoumn = Mathf.FloorToInt(i * nextRowCount / (float)currentRowCount);

                    targetColoumn = (targetColoumn + offset) % nextRowCount;

                    Nodes[currentStart + i].connections.Add(nextRowStart + targetColoumn);
                }
            }

            //some nodes get extra branches, go through each node in the row
            for (int i = 0; i < currentRowCount; i++)
            {
                //get a random value to randomly generate the extra connections
                if (Random.value > 0.5f)
                {
                    continue;
                }//end if

                MapNodeData source = Nodes[currentStart + i];

                if (source.connections.Count >= 2)
                    continue;

                int targetCol = Random.Range(0, nextRowCount);
                int targetIndex = nextRowStart + targetCol;

                if (!source.connections.Contains(targetIndex))
                    source.connections.Add(targetIndex);
            }

            currentStart = nextRowStart;
        }
    }//end GenerateConnections

    //removes any nodes that dont have a connection
    private static void RemoveUnconnectedNodes()
    {
        foreach (MapNodeData node in Nodes)
        {
            if (node.row == 0)
            {
                continue;
            }//end if

            //boolean to keep track of connections
            bool hasConnection = false;

            //go through each node
            foreach (MapNodeData previous in Nodes)
            {
                //if it has a previous connection, mark hasConnection as true
                if (previous.connections.Contains(node.index))
                {
                    hasConnection = true;
                    break;
                }//end if

            }//end foreach

            //set the node's removed tracker to true or false depending
            node.removed = !hasConnection;
        }//end foreach

        // Remove connections pointing to removed nodes.
        foreach (MapNodeData node in Nodes)
        {
            node.connections.RemoveAll(index => Nodes[index].removed);
        }//end foreach
    }//end RemoveUnconnectedNodes


    //checks if you can enter a node
    public static bool CanEnterNode(int index)
    {
        //if the index is invalid (less than 0 or higher than possible) you can't enter it
        if (index < 0 || index >= Nodes.Count)
        {
            return false;
        }//end if

        //get the node at the index
        MapNodeData node = Nodes[index];

        //if the run or node is done/completed, you can't enter it
        if (RunCompleted || node.completed)
        {
            return false;
        }//end if

        //if the node isnt in the current row, you can't enter it
        if (node.row != CurrentRow)
        {
            return false;
        }//end if


        //if the starting row of nodes:
        if (CurrentRow == 0)
        {
            return true;
        }//end if


        //check the connections of completed nodes
        foreach (MapNodeData previous in Nodes)
        {
            if (previous.completed && previous.connections.Contains(index))
            {
                return true;
            }//end if
        }//end foreach

        return false;
    }//end CanEnterNode

    //Enters a node (if it can be entered)
    public static bool EnterNode(int index)
    {
        if (!CanEnterNode(index))
        {
            return false;
        }//end if

        //update the index
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
        }//end if
        else
        {
            CurrentRow++; //move to the next row of nodes otherwise
        }//end else
    }//end CompleteCurrentNode


    //THIS IS FOR TESTING PURPOSES, I ran into an issue where the nodes weren't reseting between tests so i have this here
    //likely this bug would happen in games too, but maybe that would be intentional for quitting and resuming?
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        Nodes.Clear();

        CurrentRow = 0;
        CurrentNodeIndex = -1;
        RunCompleted = false;
    }//end resetStatistics

}//end RunState
