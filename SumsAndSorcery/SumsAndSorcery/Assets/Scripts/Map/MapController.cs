using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [Header("Map Objects")]
    [SerializeField] private RectTransform mapArea;
    [SerializeField] private MapNodeUI nodePrefab;
    [SerializeField] private Image connectionPrefab;

    [Header("Map Layout")]
    [SerializeField] private float horizontalSpacing = 260f;
    [SerializeField] private float verticalSpacing = 110f;
    [SerializeField] private float connectionThickness = 5f;
    [SerializeField] private float randomOffset = 20f;

    [Header("Scene")]
    [SerializeField] private string combatSceneName = "Combat";

    private Vector2[] nodePositions;

    private void Start()
    {

        if (!RunState.HasRun)
        {
            RunState.StartNewRun();
        }

        GenerateMap();
    }

    private void GenerateMap()
    {
        nodePositions = new Vector2[RunState.TotalNodes];

        int index = 0;

        for (int row = 0; row < RunState.TotalRows; row++)
        {
            int count = RunState.NodesPerRow[row];

            for (int col = 0; col < count; col++)
            {
                MapNodeData node = RunState.Nodes[index];

                float x = (col - (count - 1) / 2f) * horizontalSpacing;

                float y = -500f + row * verticalSpacing; 

                if (node.encounterType != "Boss")
                {
                    x += Random.Range(-randomOffset, randomOffset);
                }

                nodePositions[index] = new Vector2(x, y);
                    
                index++;
            }
        }

        //resize Content to fit the entire map.
        float totalHeight =
            200f + (RunState.TotalRows - 1) * verticalSpacing;

        mapArea.sizeDelta = new Vector2(600f, totalHeight);


        GenerateConnections();
        GenerateNodes();
    }

    private void GenerateConnections()
    {
        foreach (MapNodeData node in RunState.Nodes)
        {
            if (node.removed)
                continue;
            foreach (int nextIndex in node.connections)
            {
                if (RunState.Nodes[nextIndex].removed)
                    continue;

                if (RunState.Nodes[nextIndex].row != node.row + 1)
                    continue;

                Vector2 start = nodePositions[node.index];
                Vector2 end = nodePositions[nextIndex];

                Vector2 direction = end - start;

                Image line = Instantiate(
                    connectionPrefab,
                    mapArea
                );

                RectTransform lineTransform =
                    line.GetComponent<RectTransform>();

                lineTransform.anchoredPosition =
                    (start + end) / 2f;

                lineTransform.sizeDelta = new Vector2(
                    direction.magnitude,
                    connectionThickness
                );

                float angle = Mathf.Atan2(
                    direction.y,
                    direction.x
                ) * Mathf.Rad2Deg;

                lineTransform.localRotation =
                    Quaternion.Euler(0, 0, angle);

                line.raycastTarget = false;
            }
        }
    }

    private void GenerateNodes()
    {
        foreach (MapNodeData node in RunState.Nodes)
        {
            if (node.removed)
                continue;
            MapNodeUI newNode = Instantiate(
                nodePrefab,
                mapArea
            );

            RectTransform nodeTransform = newNode.GetComponent<RectTransform>();

            nodeTransform.anchorMin = new Vector2(0.5f, 0.5f);
            nodeTransform.anchorMax = new Vector2(0.5f, 0.5f);
            nodeTransform.pivot = new Vector2(0.5f, 0.5f);

            nodeTransform.anchoredPosition = nodePositions[node.index];

            bool available = RunState.CanEnterNode(node.index);

            newNode.Setup(
                node.index,
                available,
                node.completed,
                OnNodeSelected
            );
        }
    }

    private void OnNodeSelected(int index)
    {
        if (!RunState.EnterNode(index))
            return;

        SceneManager.LoadScene(combatSceneName);
    }
}
