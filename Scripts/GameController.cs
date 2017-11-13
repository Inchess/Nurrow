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
        Change1DTo2DArray();
        AddText();
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    void Change1DTo2DArray()
    {
        buttonArray[2, 0] = buttonList[0];
        buttonArray[2, 1] = buttonList[1];
        buttonArray[2, 2] = buttonList[2];
        buttonArray[1, 0] = buttonList[3];
        buttonArray[1, 1] = buttonList[4];a
        buttonArray[1, 2] = buttonList[5];b
        buttonArray[0, 0] = buttonList[6];
        buttonArray[0, 1] = buttonList[7];
        buttonArray[0, 2] = buttonList[8];
    }

    void AddText()
    {
        buttonArray[2, 0].text = "7";
        buttonArray[2, 1].text = "8";
        buttonArray[2, 2].text = "9";
        buttonArray[1, 0].text = "6";
        buttonArray[1, 1].text = "5";
        buttonArray[1, 2].text = "4";
        buttonArray[0, 0].text = "1";
        buttonArray[0, 1].text = "2";
        buttonArray[0, 2].text = "3";
    }

    public void EndTurn()
    {
        Debug.Log("ENnn");
    }

}
