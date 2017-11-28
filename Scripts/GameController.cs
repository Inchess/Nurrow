using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {
    
    public Sprite upArrow;
    public Sprite upLeftArrow;
    public Sprite upRightArrow;
    public Sprite rightArrow;
    public Sprite leftArrow;
    public Sprite downArrow;
    public Sprite downRightArrow;
    public Sprite downLeftArrow;
    private int columns = 5;
    private int rows = 5;
    private Text[,] buttonTextArray;
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
    private int newPositionX;
    private int newPositionY;
    private string newButtonNumber;
    private int numberOfMoves = 20;
    public GameObject gridSpacePrefab;
    public GameObject canvasObject;
    public GameObject boardPanel;
    public GameObject divisionLine;
    public GameObject textPrefab;
    public GameObject image;
    private int maxBoardSize = 512;
    private int boardSizeHeight;
    private int boardSizeWidth;
    private int gridSize;
    private int divisionLineWidth = 0;
    private int numberOfDivisionsInWidth;
    private int numberOfDivisionsInHeight;
    private int gridAssetFromDivisions;
    

    //          BOARD
    //  \x   0   1   2
    //  y    _________
    //  2   |7   8   9|
    //  1   |6   5   4|
    //  0   |1   2   3|
    //      |_________|

    private void Awake()
    {
        CheckCorrectRowsAndColumns();
        InstantiateObjects();
        InstantiateVariables();
        SetArrowsNames();
        //SetGameControllerReferenceOnButtons();
        //Change1DTo2DArray();
        //AddNumbersToButtons();
        //AddArrowsToButtons();
    }

    private void CheckCorrectRowsAndColumns()
    {
        if (1 >= columns || columns >= 6 || 1 >= rows || rows >= 6)
        {
            throw new ArgumentException("Incorrect number of grids! Grids in row: " + columns + ", grids in column: " + rows);
        } else if (Math.Abs(rows - columns) > 1)
        {
            throw new ArgumentException("Too big difference between columns and rows! Grids in row: " + columns + ", grids in column: " + rows);
        } else if(rows > columns)
        {
            throw new ArgumentException("There are more grids in column than grids in row! Grids in row: " + columns + ", grids in column: " + rows);
        }

    }

    private void InstantiateObjects()
    {
        rand = new System.Random();
    }

    private void InstantiateVariables()
    {
        buttonTextArray = new Text[columns, rows];
        imageArray = new Image[columns, rows];
        numbers = new List<int>();
        numbersLeft = new List<int>();
        AddNumbersToList(numbers);
        AddNumbersToList(numbersLeft);
        arrowsList = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsToUse = new List<Sprite>();
    }

    private void AddNumbersToList(List<int> list)
    {
        for (int i = 1; i <= columns * rows; i++)
        {
            list.Add(i);
        }
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

    void Start()
    {
        ModifyVariablesValues();
        PrepareArrowsToUse();
        CalculateNumberOfDivisions();
        gridSize = CalculateGridSize();
        ModifySizeAndMovePrefabGridSize();
        CalculateBoardSizes();
        ResizeBoard();
        CreateAndArrangeGrids();
    }

    private void ModifyVariablesValues()
    {
        if (columns == 2)
        {
            gridAssetFromDivisions = 20;
        } else if (columns == 3)
        {
            gridAssetFromDivisions = 14;
        } else if (columns == 4)
        {
            gridAssetFromDivisions = 7;
        } else if (columns == 5)
        {
            gridAssetFromDivisions = 1;
        } else
        {
            throw new Exception("Incorrect number of columns: " + columns);
        }
    }

    void PrepareArrowsToUse()
    {
        int numOfButtons = columns * rows;
        int allArrowsOccurrence = numOfButtons / arrowsList.Count;
        int extraArrows = numOfButtons % arrowsList.Count;
        for (int i = 0; i < allArrowsOccurrence; i++)
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

    private void CalculateNumberOfDivisions()
    {
        numberOfDivisionsInWidth = rows - 1;
        numberOfDivisionsInHeight = columns - 1;
    }

    private int CalculateGridSize()
    {
        int totalWidthForGrids = maxBoardSize - (numberOfDivisionsInWidth * divisionLineWidth) - (rows * 2 * gridAssetFromDivisions);
        int gridWidth = totalWidthForGrids / rows;
        int totalHeightForGrids = maxBoardSize - (numberOfDivisionsInHeight * divisionLineWidth) - (columns * 2 * gridAssetFromDivisions);
        int gridHeigth = totalHeightForGrids / columns;
        return Math.Min(gridHeigth, gridWidth);
    }

    private void ModifySizeAndMovePrefabGridSize()
    {
        RectTransform gridPrefabRectTransform = gridSpacePrefab.GetComponent<RectTransform>();
        gridPrefabRectTransform.sizeDelta = new Vector2(gridSize, gridSize);
    }

    private void CalculateBoardSizes()
    {
        boardSizeWidth = columns * gridSize + 2 * columns * gridAssetFromDivisions + numberOfDivisionsInHeight * divisionLineWidth;
        boardSizeHeight = rows * gridSize + 2 * rows * gridAssetFromDivisions + numberOfDivisionsInWidth * divisionLineWidth;
    }

    private void ResizeBoard()
    {
        RectTransform boardRectTransform = boardPanel.GetComponent<RectTransform>();
        boardRectTransform.sizeDelta = new Vector2(boardSizeWidth, boardSizeHeight);
        Debug.Log("H: " + boardSizeHeight + ", W: " + boardSizeWidth);
    }


    private void CreateAndArrangeGrids()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject newSmoke = Instantiate(gridSpacePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                newSmoke.name = "Grid" + x + y;
                newSmoke.transform.SetParent(canvasObject.transform, false);
                newSmoke.transform.localScale = new Vector3(1, 1, 1);
                RectTransform gridPrefabRectTransform = newSmoke.GetComponent<RectTransform>();
                int gridsAsset = gridSize + 2 * gridAssetFromDivisions + divisionLineWidth;
                gridPrefabRectTransform.anchoredPosition = new Vector2((x * gridsAsset + gridsAsset/2) - (boardSizeWidth / 2), (y * gridsAsset + gridsAsset / 2) - (boardSizeHeight / 2));
                buttonTextArray[x, y] = newSmoke.GetComponentInChildren<Text>();
                imageArray[x, y] = newSmoke.GetComponentsInChildren<Image>()[1];
            }
        }
        AddNumbersToButtons();
        AddArrowsToButtons();

    }

    //void SetGameControllerReferenceOnButtons()
    //{
    //    for (int i = 0; i < buttonTextList.Length; i++)
    //    {
    //        buttonTextList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
    //        imageList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
    //    }
    //}

    //void Change1DTo2DArray()
    //{
    //    Match1DElementTo2D(0, 0, 2);
    //    Match1DElementTo2D(1, 1, 2);
    //    Match1DElementTo2D(2, 2, 2);
    //    Match1DElementTo2D(3, 0, 1);
    //    Match1DElementTo2D(4, 1, 1);
    //    Match1DElementTo2D(5, 2, 1);
    //    Match1DElementTo2D(6, 0, 0);
    //    Match1DElementTo2D(7, 1, 0);
    //    Match1DElementTo2D(8, 2, 0);
    //}

    //void Match1DElementTo2D(int index, int x2D, int y2D)
    //{
    //    buttonTextArray[x2D, y2D] = buttonTextList[index];
    //    imageArray[x2D, y2D] = imageList[index];
    //}

    void AddNumbersToButtons()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            { 
                int randomNumber = 0;
                do
                {
                    randomNumber = rand.Next(0, numbersLeft.Count);
                } while (y == 0 && randomNumber < columns);
                buttonTextArray[x, y].text = numbersLeft[randomNumber].ToString();
                numbersLeft.RemoveAt(randomNumber);
            }
        }
    }

    void AddArrowsToButtons()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
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
        int moveValue = 45;
        int[] arrowTransition = ChangeLocation(arrow.name, moveValue);
        image.rectTransform.anchoredPosition = new Vector2(arrowTransition[0], arrowTransition[1]);
    }

    public void MoveNumbersOnClick(Text buttonText, Button button)
    {
        string number = button.GetComponentInChildren<Text>().text;
        FindButtonIndexes(number);
        ChangeButtonsPlaces(number);
        CheckIfGameFinished(button);
        numberOfMoves--;
    }

    void FindButtonIndexes(string number)
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (buttonTextArray[i, j].text == number)
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
        buttonTextArray[newPositionX, newPositionY].text = clickedButtonNumber;
        buttonTextArray[clickedButtonX, clickedButtonY].text = newButtonNumber;
    }

    void CalculateNewLocation()
    {
        int moveValue = 1;
        int[] arrowTransition = ChangeLocation(clickedButtonArrowName, moveValue);
        newPositionX = (clickedButtonX + arrowTransition[0] + columns) % columns;
        newPositionY = (clickedButtonY + arrowTransition[1] + columns) % columns;
        newButtonNumber = buttonTextArray[newPositionX, newPositionY].text;
    }

    void CheckIfGameFinished(Button button)
    {
        if (numberOfMoves <= 0)
        {
            //LockButtons();
        }
        if (buttonTextArray[0, 0].text == "1" && buttonTextArray[1, 0].text == "2" && buttonTextArray[2, 0].text == "3")
        {
            //ChangeColorToGreen(buttonArray[0, 0]);
            //ChangeColorToGreen(buttonArray[1, 0]);
            //ChangeColorToGreen(buttonArray[2, 0]);
            RestartBoard();
        }
        else
        {
            Debug.Log("Moves left: " + numberOfMoves);
        }
    }

    void ChangeColorToGreen(Text text)
    {
        Button b = text.GetComponentInParent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = Color.green;
        cb.highlightedColor = Color.green;
        b.colors = cb;
    }

    void RestartBoard()
    {
        ResetVariables();
        PrepareArrowsToUse();
        AddNumbersToButtons();
        //AddArrowsToButtons();
        numberOfMoves += 5;
    }

    void ResetVariables()
    {
        arrowsToUse.Clear();
        arrowsListCopy.Clear();
        arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        numbersLeft.Clear();
        numbersLeft = new List<int> (new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

    }

    //void LockButtons()
    //{
    //    for (int i = 0; i < buttonTextList.Length; i++)
    //    {
    //        buttonTextList[i].GetComponentInParent<Button>().interactable = false;
    //    }
    //}

    int[] ChangeLocation(string arrowName, int valueMove)
    {
        int xChange = 0;
        int yChange = 0;
        if (arrowName == upArrowName)
        {
            yChange = valueMove;
        }
        else if (arrowName == upRightArrowName)
        {
            xChange = valueMove;
            yChange = valueMove;
        }
        else if (arrowName == upLeftArrowName)
        {
            xChange = -valueMove;
            yChange = valueMove;
        }
        else if (arrowName == rightArrowName)
        {
            xChange = valueMove;
        }
        else if (arrowName == leftArrowName)
        {
            xChange = -valueMove;
        }
        else if (arrowName == downArrowName)
        {
            yChange = -valueMove;
        }
        else if (arrowName == downLeftArrowName)
        {
            xChange = -valueMove;
            yChange = -valueMove;
        }
        else if (arrowName == downRightArrowName)
        {
            xChange = valueMove;
            yChange = -valueMove;
        }
        return new int[] { xChange, yChange };
    }
}
