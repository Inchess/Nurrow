﻿using System.Collections;
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
    private List<Sprite> arrowsListCopy;
    private List<Sprite> arrowsToUse;
    string upArrowName = "Up Arrow";
    string upRightArrowName = "Up Right Arrow";
    string upLeftArrowName = "Up Left Arrow";
    string rightArrowName = "Right Arrow";
    string leftArrowName = "Left Arrow";
    string downArrowName = "Down Arrow";
    string downRightArrowName = "Down Right Arrow";
    string downLeftArrowName = "Down Left Arrow";


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
        SetArrowsNames();
        SetGameControllerReferenceOnButtons();
        Change1DTo2DArray();
        PrepareArrowsToUse();
        AddNumbersToButtons();
        AddArrowsToButtons();
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
        arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsToUse = new List<Sprite>();
    }

    private void SetArrowsNames()
    {
        upArrow.name = upArrowName;
        upRightArrow.name = upRightArrowName;
        upLeftArrow.name = upLeftArrowName;
        rightArrow.name = rightArrowName;
        leftArrow.name = leftArrowName;
        downArrow.name = downArrowName;
        downLeftArrow.name = downLeftArrowName;
        downRightArrow.name = downRightArrowName;
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

    void PrepareArrowsToUse()
    {
        int numOfButtons = rows * columns;
        int numOfOccurrence = numOfButtons / arrowsList.Count;
        int extraArrows = numOfButtons % arrowsList.Count;
        for (int i = 0; i < numOfOccurrence; i++)
        {
            arrowsToUse.AddRange(arrowsList);
        }
        for (int i = 0; i < extraArrows; i++)
        {
            int randomNumber = rand.Next(0, arrowsListCopy.Count);
            arrowsToUse.Add(arrowsListCopy[randomNumber]);
            arrowsListCopy.RemoveAt(randomNumber);
        }
    }

    void AddNumbersToButtons()
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

    void AddArrowsToButtons()
    {
        for (int i = 0; i < imageList.Length; i++)
        {
            int index = rand.Next(0, arrowsToUse.Count);
            imageList[i].sprite = arrowsToUse[index];
            imageList[i].rectTransform.sizeDelta = new Vector2(40, 40);
            SetArrowLocation(arrowsToUse[index], imageList[i]);
            arrowsToUse.RemoveAt(index);
        }
    }

    void SetArrowLocation(Sprite arrow, Image image)
    {
        string name = arrow.name;
        int x = 0;
        int y = 0;
        if (name == upArrowName)
        {
            x = 0;
            y = 45;
        } else if (name == upRightArrowName)
        {
            x = 45;
            y = 45;
        } else if (name == upLeftArrowName)
        {
            x = -45;
            y = 45;
        } else if (name == rightArrowName)
        {
            x = 45;
            y = 0;
        } else if (name == leftArrowName)
        {
            x = -45;
            y = 0;
        } else if (name == downArrowName)
        {
            x = 0;
            y = -45;
        } else if (name == downLeftArrowName)
        {
            x = -45;
            y = -45;
        } else if (name == downRightArrowName)
        {
            x = 45;
            y = -45;
        }
        SetPosition(image, x, y);
    }

    void SetPosition(Image image, int x, int y)
    {
        image.rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public void MoveNumbersOnClick(Text buttonText, Button button)
    {
        string number = button.GetComponentInChildren<Text>().text;
        FindButtonIndexes(number);
    }

    void FindButtonIndexes(string number)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (buttonArray[i, j].text == number)
                {
                    Debug.Log("i: " + i + ", j: " + j);
                }
            }
        }
    }

}
