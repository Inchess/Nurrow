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
    private int columns = 2;
    private int rows = 2;
    private int board3x2limit = 200;
    private int board3x3limit = 600;
    private int board4x3limit = 1200;
    private int board4x4limit = 2000;
    private int board5x4limit = 3200;
    private int board5x5limit = 5000;
    private Text[,] buttonTextArray;
    private Image[,] imageArray;
    private List<int> numbers;
    private List<int> numbersLeft;
    private System.Random rand;
    private List<Sprite> arrowsList;
    private List<Sprite> arrowsListCopy;
    private List<Sprite> arrowsToUse;
    private List<Sprite> arrowsUpDownLeftRight;
    private List<Sprite> arrowsDiagonal;
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
    private Sprite newImageArrow;
    private int numberOfMoves = 120;
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
    private int textSize;
    private int arrowSize;
    private int arrowMove;
    public Text timerText;
    public Text numberOfMovesText;
    public Text pointsText;
    private int points;
    private float targetTime;
    private List<GameObject> gridsList;
    

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
    }

    private void InstantiateObjects()
    {
        rand = new System.Random();
        arrowsList = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        arrowsUpDownLeftRight = new List<Sprite>(new Sprite[] { upArrow, leftArrow, rightArrow, downArrow });
        arrowsDiagonal = new List<Sprite>(new Sprite[] { upLeftArrow, upRightArrow, downLeftArrow, downRightArrow });
        numbersLeft = new List<int>();
        numbers = new List<int>();
        arrowsToUse = new List<Sprite>();
        gridsList = new List<GameObject>();
    }

    private void InstantiateVariables()
    {
        buttonTextArray = new Text[columns, rows];
        imageArray = new Image[columns, rows];
        AddNumbersToList(numbers);
        AddNumbersToList(numbersLeft);
    }

    private void AddNumbersToList(List<int> list)
    {
        list.Clear();
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

        SetTime();
        numberOfMovesText.text = numberOfMoves.ToString();

        CheckCorrectRowsAndColumns();
        ModifyVariablesValues();
        PrepareArrowsToUse();
        CalculateNumberOfDivisions();
        gridSize = CalculateGridSize();
        ModifySizeAndMovePrefabGridSize();
        CalculateBoardSizes();
        ResizeBoard();
        CreateAndArrangeGrids();
        AddNumbersToButtons();
        ColorCorrectNumbersOnButtons();
        AddArrowsToButtons();
        SetGameControllerReferenceOnButtons();
    }

    void SetTime()
    {
        targetTime = 20 + (rows + columns - 4) * 10;
    }

    private void Update()
    {
        targetTime -= Time.deltaTime;
        timerText.text = Math.Round(targetTime, 0).ToString();
    }

    private void CheckCorrectRowsAndColumns()
    {
        if (1 >= columns || columns >= 6)
        {
            throw new ArgumentException("Incorrect number of grids! Grids in row: " + columns + ", grids in column: " + rows);
        }
        else if (Math.Abs(rows - columns) > 1)
        {
            throw new ArgumentException("Too big difference between columns and rows! Grids in row: " + columns + ", grids in column: " + rows);
        }
        else if (rows > columns)
        {
            throw new ArgumentException("There are more grids in column than grids in row! Grids in row: " + columns + ", grids in column: " + rows);
        }
    }   
    
    void SetGameControllerReferenceOnButtons()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                buttonTextArray[x, y].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
                imageArray[x, y].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
            }
        }
    }

    private void ModifyVariablesValues()
    {
        if (columns == 2)
        {
            gridAssetFromDivisions = 20;
            textSize = 90;
            arrowSize = 80;
            arrowMove = 70;
        } else if (columns == 3)
        {
            gridAssetFromDivisions = 14;
            textSize = 70;
            arrowSize = 40;
            arrowMove = 45;
        } else if (columns == 4)
        {
            gridAssetFromDivisions = 7;
            textSize = 60;
            arrowSize = 35;
            arrowMove = 40;
        } else if (columns == 5)
        {
            gridAssetFromDivisions = 1;
            textSize = 40;
            arrowSize = 35;
            arrowMove = 30;
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
            if (columns == 2 && i == 2)
            {
                int randomNum = rand.Next(0, arrowsUpDownLeftRight.Count);
                arrowsToUse.Add(arrowsUpDownLeftRight[randomNum]);
            }
            else if (columns == 2 && i == 3)
            {
                int randomNum = rand.Next(0, arrowsDiagonal.Count);
                arrowsToUse.Add(arrowsDiagonal[randomNum]);
            }
            else
            {
                int randomNumber = rand.Next(0, arrowsListCopy.Count);
                arrowsToUse.Add(arrowsListCopy[randomNumber]);
                arrowsListCopy.RemoveAt(randomNumber);
            }
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
                buttonTextArray[x, y].fontSize = textSize;
                imageArray[x, y] = newSmoke.GetComponentsInChildren<Image>()[1];
                gridsList.Add(newSmoke);
            }
        }
    }

    void AddNumbersToButtons()
    {
        for (int y = (rows - 1); y >= 0; y--)
        {
            for (int x = 0; x < columns; x++)
            { 
                int randomNumber = 0;
                do
                {
                    randomNumber = rand.Next(0, numbersLeft.Count);
                } while (y == (rows - 1) && randomNumber < columns);
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
                imageArray[i, j].rectTransform.sizeDelta = new Vector2(arrowSize, arrowSize);
                SetArrowLocation(arrowsToUse[index], imageArray[i, j]);
                arrowsToUse.RemoveAt(index);
            }
        }
    }

    void SetArrowLocation(Sprite arrow, Image image)
    {
        int[] arrowTransition = ChangeLocation(arrow.name, arrowMove);
        image.rectTransform.anchoredPosition = new Vector2(arrowTransition[0], arrowTransition[1]);
    }

    public void MoveNumbersOnClick(Text buttonText, Button button)
    {
        string number = button.GetComponentInChildren<Text>().text;
        Sprite arrow = button.GetComponentsInChildren<Image>()[1].sprite;
        FindButtonIndexes(number);
        ChangeButtonsPlaces(number);
        ChangeArrowsPlaces(arrow);
        CheckIfGameFinished(button);
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

    void ChangeArrowsPlaces(Sprite arrow)
    {
        imageArray[newPositionX, newPositionY].sprite = arrow;
        imageArray[clickedButtonX, clickedButtonY].sprite = newImageArrow;
        SetArrowLocation(arrow, imageArray[newPositionX, newPositionY]);
        SetArrowLocation(newImageArrow, imageArray[clickedButtonX, clickedButtonY]);
    }

    void CalculateNewLocation()
    {
        int moveValue = 1;
        int[] arrowTransition = ChangeLocation(clickedButtonArrowName, moveValue);
        newPositionX = (clickedButtonX + arrowTransition[0] + columns) % columns;
        newPositionY = (clickedButtonY + arrowTransition[1] + rows) % rows;
        newButtonNumber = buttonTextArray[newPositionX, newPositionY].text;
        newImageArrow = imageArray[newPositionX, newPositionY].sprite;
    }

    void CheckIfGameFinished(Button button)
    {
        ColorCorrectNumbersOnButtons();
        if (numberOfMoves <= 0)
        {
            //LockButtons();
        }
        else if (columns == 2 && buttonTextArray[0, 1].text == "1" && buttonTextArray[1, 1].text == "2" && buttonTextArray[0, 0].text == "3")
        {
            points += 300;
            UpdatePoints();
            RestartBoard();
        }

        else if (columns > 2 && buttonTextArray[0, rows-1].text == "1" && buttonTextArray[1, rows - 1].text == "2" && buttonTextArray[2, rows - 1].text == "3")
        {
            UpdatePoints();
            RestartBoard();
        }
        else
        {
            numberOfMoves--;
            numberOfMovesText.text = numberOfMoves.ToString();
        }
    }

    void ColorCorrectNumbersOnButtons()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                int correctNumberOnField = (x + 1) + Math.Abs(y - (rows - 1)) * columns;
                if (buttonTextArray[x, y].text == correctNumberOnField.ToString())
                {
                    ChangeColor(buttonTextArray[x, y], Color.green);
                } else
                {
                    ChangeColor(buttonTextArray[x, y], Color.white);
                }
            }
        }
    }

    void UpdatePoints()
    {
        int pointsInThisRound = rows * columns * (int)targetTime;
        Debug.Log("Time left: "+ targetTime + ", Points scored: " + pointsInThisRound);
        points += pointsInThisRound;
        pointsText.text = points.ToString();
    }

    void ChangeColor(Text text, Color color)
    {
        Button b = text.GetComponentInParent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        b.colors = cb;
    }
    
    void RestartBoard()
    {
        ResetVariables();
        PrepareArrowsToUse();
        AddNumbersToButtons();
        AddArrowsToButtons();
        ColorCorrectNumbersOnButtons();
        numberOfMoves += 5;
        SetTime();
        DestroyGrids();
        ChangeBoardSize();
    }

    void ResetVariables()
    {
        arrowsToUse.Clear();
        arrowsListCopy.Clear();
        arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        numbersLeft.Clear();
        numbersLeft = new List<int>();
        AddNumbersToList(numbers);
        AddNumbersToList(numbersLeft);

    }

    void ChangeBoardSize()
    {
        if (points <= board3x2limit)
        {
            SetColumnsAndRows(2, 2);
        } else if (board3x2limit < points && points <= board3x3limit)
        {
            SetColumnsAndRows(3, 2);
        }
        else if (board3x3limit < points && points <= board4x3limit)
        {
            SetColumnsAndRows(3, 3);
        }
        else if (board4x3limit < points && points <= board4x4limit)
        {
            SetColumnsAndRows(4, 3);
        }
        else if (board4x4limit < points && points <= board5x4limit)
        {
            SetColumnsAndRows(4, 4);
        }
        else if (board5x4limit < points && points <= board5x5limit)
        {
            SetColumnsAndRows(5, 4);
        }
        else if (board5x5limit < points)
        {
            SetColumnsAndRows(5, 5);
        }
        Start();
    }

    void DestroyGrids()
    {
        for (int i = 0; i < gridsList.Count; i++)
        {
            Destroy(gridsList[i]);
        }
    }

    void SetColumnsAndRows(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        InstantiateVariables();
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
