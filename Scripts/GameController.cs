using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    private Board boardObject;
    private Arrows arrows;
    public Text[] buttonTextList;
    public Image[] imageList;
    public Sprite upArrow;
    public Sprite upLeftArrow;
    public Sprite upRightArrow;
    public Sprite rightArrow;
    public Sprite leftArrow;
    public Sprite downArrow;
    public Sprite downRightArrow;
    public Sprite downLeftArrow;
    private int gridsInRow = 3;
    private int gridsInColumn = 3;
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
    public GameObject board;
    public GameObject divisionLine;
    public GameObject textPrefab;
    public GameObject image;
    private int maxBoardSize = 512;
    private int boardSizeHeight;
    private int boardSizeWidth;
    private int gridSize;
    private int divisionLineWidth = 5;
    private int numberOfDivisionsInWidth;
    private int numberOfDivisionsInHeight;
    private int gridAssetFromDivisions = 20;
    

    //          BOARD
    //  \x   0   1   2
    //  y    _________
    //  2   |7   8   9|
    //  1   |6   5   4|
    //  0   |1   2   3|
    //      |_________|

    private void Awake()
    {
        InstantiateObjects();
        InstantiateVariables();
        SetArrowsNames();
        //SetGameControllerReferenceOnButtons();
        //Change1DTo2DArray();
        PrepareArrowsToUse();
        //AddNumbersToButtons();
        //AddArrowsToButtons();
    }

    private void InstantiateObjects()
    {
        rand = new System.Random();
        arrows = new Arrows();
    }

    void Start()
    {
        CalculateNumberOfDivisions();
        gridSize = CalculateGridSize();
        ModifySizeAndMovePrefabGridSize();
        CalculateBoardSizes();
        ResizeBoard();
        CreateAndArrangeGrids();
    }

    private void CalculateNumberOfDivisions()
    {
        numberOfDivisionsInWidth = gridsInColumn - 1;
        numberOfDivisionsInHeight = gridsInRow - 1;
    }

    private int CalculateGridSize()
    {
        int totalWidthForGrids = maxBoardSize - (numberOfDivisionsInWidth * divisionLineWidth) - (gridsInColumn * 2 * gridAssetFromDivisions);
        int gridWidth = totalWidthForGrids / gridsInColumn;
        int totalHeightForGrids = maxBoardSize - (numberOfDivisionsInHeight * divisionLineWidth) - (gridsInRow * 2 * gridAssetFromDivisions);
        int gridHeigth = totalHeightForGrids / gridsInRow;
        return Math.Min(gridHeigth, gridWidth);
    }

    private void ModifySizeAndMovePrefabGridSize()
    {
        RectTransform gridPrefabRectTransform = gridSpacePrefab.GetComponent<RectTransform>();
        gridPrefabRectTransform.sizeDelta = new Vector2(gridSize, gridSize);
    }

    private void CalculateBoardSizes()
    {
        boardSizeHeight = gridsInRow * gridSize + 2 * gridsInRow * gridAssetFromDivisions + numberOfDivisionsInHeight * divisionLineWidth;
        boardSizeWidth = gridsInColumn * gridSize + 2 * gridsInColumn * gridAssetFromDivisions + numberOfDivisionsInWidth * divisionLineWidth;
    }

    private void ResizeBoard()
    {
        RectTransform boardRectTransform = board.GetComponent<RectTransform>();
        boardRectTransform.sizeDelta = new Vector2(boardSizeWidth, boardSizeHeight);
        Debug.Log("H: " + boardSizeHeight + ", W: " + boardSizeWidth);
    }


    private void CreateAndArrangeGrids()
    {
        rand = new System.Random();
        for (int y = 0; y < gridsInRow; y++)
        {
            for (int x = 0; x < gridsInColumn; x++)
            {
                GameObject newSmoke = Instantiate(gridSpacePrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
                newSmoke.name = "Grid" + x + y;
                newSmoke.transform.SetParent(canvasObject.transform, false);
                newSmoke.transform.localScale = new Vector3(1, 1, 1);
                RectTransform gridPrefabRectTransform = newSmoke.GetComponent<RectTransform>();
                int gridsAsset = gridSize + 2 * gridAssetFromDivisions + divisionLineWidth;
                gridPrefabRectTransform.anchoredPosition = new Vector2((x * gridsAsset + gridsAsset/2) - (boardSizeWidth / 2), (y * gridsAsset + gridsAsset / 2) - (boardSizeHeight / 2));
                buttonTextArray[x, y] = newSmoke.GetComponentInChildren<Text>();
                imageArray[x, y] = newSmoke.GetComponentInChildren<Image>();
            }
        }
        AddNumbersToButtons();
        AddArrowsToButtons();

    }

    private void InstantiateVariables()
    {
        buttonTextArray = new Text[gridsInRow, gridsInColumn];
        imageArray = new Image[gridsInRow, gridsInColumn];
        numbers = new List<int>();
        numbersLeft = new List<int>();
        AddNumbersToList(numbers);
        AddNumbersToList(numbersLeft);
        //numbers = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        //numbersLeft = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        arrowsList = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsToUse = new List<Sprite>();
    }

    private void AddNumbersToList(List<int> list)
    {
        for (int i = 1; i <= gridsInRow * gridsInColumn; i++)
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

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonTextList.Length; i++)
        {
            buttonTextList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
            imageList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    void Change1DTo2DArray()
    {
        Match1DElementTo2D(0, 0, 2);
        Match1DElementTo2D(1, 1, 2);
        Match1DElementTo2D(2, 2, 2);
        Match1DElementTo2D(3, 0, 1);
        Match1DElementTo2D(4, 1, 1);
        Match1DElementTo2D(5, 2, 1);
        Match1DElementTo2D(6, 0, 0);
        Match1DElementTo2D(7, 1, 0);
        Match1DElementTo2D(8, 2, 0);
    }

    void Match1DElementTo2D(int index, int x2D, int y2D)
    {
        buttonTextArray[x2D, y2D] = buttonTextList[index];
        imageArray[x2D, y2D] = imageList[index];
    }

    void PrepareArrowsToUse()
    {
        int numOfButtons = gridsInRow * gridsInColumn;
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

    void AddNumbersToButtons()
    {
        for (int j = 0; j < gridsInRow; j++)
        {
            for (int i = 0; i < gridsInColumn; i++)
            { 
                int randomNumber = 0;
                do
                {
                    randomNumber = rand.Next(0, numbersLeft.Count);
                } while (j == 0 && randomNumber < gridsInRow);
                buttonTextArray[i, j].text = numbersLeft[randomNumber].ToString();
                numbersLeft.RemoveAt(randomNumber);
            }
        }
    }

    void AddArrowsToButtons()
    {
        for (int i = 0; i < gridsInRow; i++)
        {
            for (int j = 0; j < gridsInColumn; j++)
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
        for (int i = 0; i < gridsInRow; i++)
        {
            for (int j = 0; j < gridsInColumn; j++)
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
        newPositionX = (clickedButtonX + arrowTransition[0] + gridsInRow) % gridsInRow;
        newPositionY = (clickedButtonY + arrowTransition[1] + gridsInRow) % gridsInRow;
        newButtonNumber = buttonTextArray[newPositionX, newPositionY].text;
    }

    void CheckIfGameFinished(Button button)
    {
        if (numberOfMoves <= 0)
        {
            LockButtons();
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
        AddArrowsToButtons();
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

    void LockButtons()
    {
        for (int i = 0; i < buttonTextList.Length; i++)
        {
            buttonTextList[i].GetComponentInParent<Button>().interactable = false;
        }
    }

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
