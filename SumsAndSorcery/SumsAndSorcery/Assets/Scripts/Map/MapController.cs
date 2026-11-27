using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    //fields for the map scroll area, node prefab, and that connection prefab
    [SerializeField] private RectTransform mapArea;
    [SerializeField] private MapNodeUI nodePrefab;
    [SerializeField] private Image connectionPrefab;

    //fields for the node spacing, offsets, and how thick the connection is
    [SerializeField] private float horizontalSpacing = 260f;
    [SerializeField] private float verticalSpacing = 110f;
    [SerializeField] private float connectionThickness = 5f;
    [SerializeField] private float randomOffset = 20f;

    //fields for the scenes, add event and stuff later
    [SerializeField] private string combat = "Combat";

    //a vector to hold the position of all the nodes
    private Vector2[] nodePositions;

    private void Start()
    {
        //make a run if there isnt a run yet
        if (!RunState.HasRun)
        {
            RunState.StartNewRun();
        }
        //create the map when the scene loads
        GenerateMap();
    }//end start

    //makes the positions of the nodes and makes the map objects
    private void GenerateMap()
    {
        //store the position of every node
        nodePositions = new Vector2[RunState.TotalNodes];

        int index = 0;

        //loop through each row of nodes
        for (int row = 0; row < RunState.TotalRows; row++)
        {
            int count = RunState.NodesPerRow[row];

            //loop through each node in the row
            for (int column = 0; column < count; column++)
            {
                MapNodeData node = RunState.Nodes[index];

                //get the horizontal position based on the node's coloumn
                float x = (column - (count - 1) / 2f) * horizontalSpacing;

                //then get the vertical spacing based on the row
                float y = -500f + row * verticalSpacing; 

                //move the nodes around a little bit for add some randomness
                if (node.encounterType != "Boss")
                {
                    x += Random.Range(-randomOffset, randomOffset);
                }//end if

                //save the position
                nodePositions[index] = new Vector2(x, y);
                    
                index++;
            }//end inner for
        }//end for

        //resize it to fit in the map
        float totalHeight = 200f + (RunState.TotalRows - 1) * verticalSpacing;
        mapArea.sizeDelta = new Vector2(600f, totalHeight);

        //create the connections and the nodes
        GenerateConnections();
        GenerateNodes();
    }//end GenerateMap



    //makes the lines between the nodes
    private void GenerateConnections()
    {
        //loop through every node on the map
        foreach (MapNodeData node in RunState.Nodes)
        {
            //skip any nodes that were removed
            if (node.removed == true)
            {
                continue;
            }//end if

            //for every node connected to this node:
            foreach (int nextIndex in node.connections)
            {
                //skip if the connected node is logically removed
                if (RunState.Nodes[nextIndex].removed)
                {
                    continue;
                }//end if

                //if the connection isnt in the NEXT row, skip it
                if (RunState.Nodes[nextIndex].row != node.row + 1)
                {
                    continue;
                }//end if

                //get the position of the nodes being connected
                Vector2 start = nodePositions[node.index];
                Vector2 end = nodePositions[nextIndex];

                //Get the direction
                Vector2 direction = end - start;

                //make the line and get its geometry
                Image line = Instantiate(connectionPrefab, mapArea);
                RectTransform lineTransform = line.GetComponent<RectTransform>();

                //place the line between two two points and set the thickness
                lineTransform.anchoredPosition = (start + end) / 2f;
                lineTransform.sizeDelta = new Vector2( direction.magnitude, connectionThickness);

                //get the angle of the connection line and spin it that way
                float angle = Mathf.Atan2( direction.y,direction.x) * Mathf.Rad2Deg;
                lineTransform.localRotation = Quaternion.Euler(0, 0, angle);

                line.raycastTarget = false;
            }//end inner foreach
        }//end foreach
    }//end GenerateConnections


    //create a new node object and set its state
    private void GenerateNodes()
    {
        //go through every node on the map
        foreach (MapNodeData node in RunState.Nodes)
        {
            //if the node was removed, skip it
            if (node.removed)
            {
                continue;
            }//end if

            //create a new map
            MapNodeUI newNode = Instantiate(nodePrefab, mapArea);

            //get the position for the node
            RectTransform nodeTransform = newNode.GetComponent<RectTransform>();

            //set anchors to the center of the map
            nodeTransform.anchorMin = new Vector2(0.5f, 0.5f);
            nodeTransform.anchorMax = new Vector2(0.5f, 0.5f);
            nodeTransform.pivot = new Vector2(0.5f, 0.5f);

            //place the node at its saved positions
            nodeTransform.anchoredPosition = nodePositions[node.index];

            //check if it can be clicked
            bool available = RunState.CanEnterNode(node.index);

            //set up the node's appearance and click
            newNode.Setup(node.index,available,node.completed,OnSelected);
        }//end foreach
    }

    //handles when the player selects a node
    private void OnSelected(int index)
    {
        if (!RunState.EnterNode(index))
        {
            return;
        }//end if

        SceneManager.LoadScene(combat);
    }//end OnNodeSelected

}//end MapController
