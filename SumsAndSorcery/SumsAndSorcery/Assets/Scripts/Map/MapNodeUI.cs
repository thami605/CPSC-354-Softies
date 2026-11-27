using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapNodeUI : MonoBehaviour
{
    //Fields for the button object, the text, and the image for the node (eventually need to make prefabs for the other types)
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text nodeText;
    [SerializeField] private Image nodeImage;

    //Fields for the colors of the node, avaiable if it can be clicked, locked if it cant be, and completed if its been done
    [SerializeField] private Color availableColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color completedColor = Color.green;

    //stores which node this one is
    private int nodeIndex;

    //sets up the appearance/color of the node when its clicked
    public void Setup(int index, bool available, bool completed, Action<int> onSelected)
    {
        nodeIndex = index;

        //clear old onClicks
        button.onClick.RemoveAllListeners();

        //let the player click it node if its available
        button.interactable = available;

        //set the color and text depending on its status
        if (completed == true)
        {
            nodeText.text = "Completed";
            nodeImage.color = completedColor;
        }//end if
        else if (available == true)
        {
            nodeText.text = "Combat";
            nodeImage.color = availableColor;
        }//end else if
        else
        {
            nodeText.text = "Locked";
            nodeImage.color = lockedColor;
        }//end else if else

        //call the event when its clicked (combat for combat, event for event, etc)
        button.onClick.AddListener(() =>{onSelected?.Invoke(nodeIndex);});
    }//end setup

}//end MapNodeUI
