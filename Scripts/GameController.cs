using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text[] buttonList;
    private int rows = 3;
    private int columns = 3;
    private Text[,] buttonArray;
    private List<string> numbers;
    private List<string> numbersLeft;
    private System.Random rand;

    //          BOARD
    //  \y  0   1   2
    //  x    _________
    //  2   |7   8   9|
    //  1   |6   5   4|
    //  0   |1   2   3|
    //      |_________|

    private void Awake()
    {
        buttonArray = new Text[rows, columns];
        rand = new System.Random();
        numbers = new List<string>(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9" });
        numbersLeft = numbers;
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
        buttonArray[1, 1] = buttonList[4];
        buttonArray[1, 2] = buttonList[5];
        buttonArray[0, 0] = buttonList[6];
        buttonArray[0, 1] = buttonList[7];
        buttonArray[0, 2] = buttonList[8];
    }

    void AddText()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int randomNumber = rand.Next(0, numbersLeft.Count);
                buttonArray[i, j].text = numbersLeft[randomNumber];
                numbersLeft.RemoveAt(randomNumber);
            }
        }
    }

    public void EndTurn()
    {
        Debug.Log("ENnn");
    }

}
