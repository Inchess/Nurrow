using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text[] buttonList;
    public Text[,] buttonArray = new Text[3, 3];

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.GetLength(0); i++)
        {
            for (int j = 0; j < buttonList.GetLength(1); j++)
            {
                buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
            }
        }
    }

    public void EndTurn()
    {
        Debug.Log("ENnn");
    }

}
