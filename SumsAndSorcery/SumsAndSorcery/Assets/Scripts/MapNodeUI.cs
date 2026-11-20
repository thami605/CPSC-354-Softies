using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapNodeUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text nodeText;
    [SerializeField] private Image nodeImage;

    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color completedColor = Color.green;

    private int nodeIndex;

    public void Setup(
        int index,
        bool available,
        bool completed,
        Action<int> onSelected)
    {
        nodeIndex = index;

        button.onClick.RemoveAllListeners();

        button.interactable = available;

        if (completed)
        {
            nodeText.text = "Completed";
            nodeImage.color = completedColor;
        }
        else if (available)
        {
            nodeText.text = "Combat";
            nodeImage.color = availableColor;
        }
        else
        {
            nodeText.text = "Locked";
            nodeImage.color = lockedColor;
        }

        button.onClick.AddListener(() =>
        {
            onSelected?.Invoke(nodeIndex);
        });
    }
}
