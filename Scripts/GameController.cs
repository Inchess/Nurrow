using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text[] buttonList;
    public Image[] imageList;
    public Sprite upArrow;
    public Sprite upLeftArrow;
    public Sprite upRightArrow;
    public Sprite rightArrow;
    public Sprite leftArrow;
    public Sprite downArrow;
    public Sprite downRightArrow;
    public Sprite downLeftArrow;
    private int rows = 3;
    private int columns = 3;
    private Text[,] buttonArray;
    private List<int> numbers;
    private List<int> numbersLeft;
    private System.Random rand;
    private List<Sprite> arrowsList;


    //          BOARD
    //  \y   0   1   2
    //  x    _________
    //  2   |7   8   9|
    //  1   |6   5   4|
    //  0   |1   2   3|
    //      |_________|

    private void Awake()
    {
        InstantiateObjects();
        InstantiateVariables();
        SetGameControllerReferenceOnButtons();
        Change1DTo2DArray();
        AddText();
        AddArrows();
    }

    private void InstantiateObjects()
    {
        rand = new System.Random();
    }

    private void InstantiateVariables()
    {
        buttonArray = new Text[rows, columns];
        numbers = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        numbersLeft = numbers;
        arrowsList = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
            imageList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
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
                int randomNumber = 0;
                do
                {
                    randomNumber = rand.Next(0, numbersLeft.Count);
                } while (i == 0 && randomNumber < rows);
                buttonArray[i, j].text = numbersLeft[randomNumber].ToString();
                numbersLeft.RemoveAt(randomNumber);
            }
        }
    }

    void AddArrows()
    {
        imageList[0].sprite = arrowsList[3];
        imageList[0].rectTransform.anchoredPosition = new Vector2(50, 50);
        imageList[0].rectTransform.sizeDelta = new Vector2(30, 30);
    }

    public void EndTurn()
    {
        Debug.Log("ENnn");
    }

}
