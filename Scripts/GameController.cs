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
    private Image[,] imageArray;
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
    private int clickedButtonX;
    private int clickedButtonY;
    private string clickedButtonArrowName;
    private int newButtonX;
    private int newButtonY;
    private string newButtonNumber;


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
        imageArray = new Image[rows, columns];
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
        imageArray[2, 0] = imageList[0];
        buttonArray[2, 1] = buttonList[1];
        imageArray[2, 1] = imageList[1];
        buttonArray[2, 2] = buttonList[2];
        imageArray[2, 2] = imageList[2];
        buttonArray[1, 0] = buttonList[3];
        imageArray[1, 0] = imageList[3];
        buttonArray[1, 1] = buttonList[4];
        imageArray[1, 1] = imageList[4];
        buttonArray[1, 2] = buttonList[5];
        imageArray[1, 2] = imageList[5];
        buttonArray[0, 0] = buttonList[6];
        imageArray[0, 0] = imageList[6];
        buttonArray[0, 1] = buttonList[7];
        imageArray[0, 1] = imageList[7];
        buttonArray[0, 2] = buttonList[8];
        imageArray[0, 2] = imageList[8];
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
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                int index = rand.Next(0, arrowsToUse.Count);
                imageArray[i, j].sprite = arrowsToUse[index];
                imageArray[i, j].rectTransform.sizeDelta = new Vector2(40, 40);
                SetArrowLocation(arrowsToUse[index], imageArray[i, j]);
                arrowsToUse.RemoveAt(index);
            }
        }
    }

    void SetArrowLocation(Sprite arrow, Image image)
    {
        int x = 0;
        int y = 0;
        if (arrow.name == upArrowName)
        {
            x = 0;
            y = 45;
        } else if (arrow.name == upRightArrowName)
        {
            x = 45;
            y = 45;
        } else if (arrow.name == upLeftArrowName)
        {
            x = -45;
            y = 45;
        } else if (arrow.name == rightArrowName)
        {
            x = 45;
            y = 0;
        } else if (arrow.name == leftArrowName)
        {
            x = -45;
            y = 0;
        } else if (arrow.name == downArrowName)
        {
            x = 0;
            y = -45;
        } else if (arrow.name == downLeftArrowName)
        {
            x = -45;
            y = -45;
        } else if (arrow.name == downRightArrowName)
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
        ChangeButtonsPlaces(number);
    }

    void FindButtonIndexes(string number)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (buttonArray[i, j].text == number)
                {
                    clickedButtonArrowName = imageArray[i, j].sprite.name;
                    clickedButtonX = i;
                    clickedButtonY = j;
                }
            }
        }
    }

    void ChangeButtonsPlaces(string clickedButtonNumber)
    {
        CalculateNewLocation();
        string oldButtonNumber = clickedButtonNumber;
        int oldButtonX = clickedButtonX;
        int oldButtonY = clickedButtonY;
        buttonArray[newButtonX, newButtonY].text = clickedButtonNumber;
        buttonArray[clickedButtonX, clickedButtonY].text = newButtonNumber;
        

    }

    void CalculateNewLocation()
    {
        int xChange = 0;
        int yChange = 0;
        if (clickedButtonArrowName == upArrowName)
        {
            xChange = 1;
            yChange = 0;
        }
        else if (clickedButtonArrowName == upRightArrowName)
        {
            xChange = 1;
            yChange = 1;
        }
        else if (clickedButtonArrowName == upLeftArrowName)
        {
            xChange = 1;
            yChange = -1;
        }
        else if (clickedButtonArrowName == rightArrowName)
        {
            xChange = 0;
            yChange = 1;
        }
        else if (clickedButtonArrowName == leftArrowName)
        {
            xChange = 0;
            yChange = -1;
        }
        else if (clickedButtonArrowName == downArrowName)
        {
            xChange = -1;
            yChange = 0;
        }
        else if (clickedButtonArrowName == downLeftArrowName)
        {
            xChange = -1;
            yChange = -1;
        }
        else if (clickedButtonArrowName == downRightArrowName)
        {
            xChange = -1;
            yChange = 1;
        }


        newButtonX = (clickedButtonX + xChange + rows) % rows;
        newButtonY = (clickedButtonY + yChange + rows) % rows;
        newButtonNumber = buttonArray[newButtonX, newButtonY].text;
        Debug.Log("newX: " + newButtonX + ", newY: " + newButtonY + ", newNumber: " + newButtonNumber);

    }
}
