﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

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
    private int nextLevelPointsLimit;
    private int board3x2limit = 200;
    private int board3x3limit = 600;
    private int board4x3limit = 1500;
    private int board4x4limit = 3000;
    private int board5x4limit = 6000;
    private int board5x5limit = 15000;
    private Text[,] buttonTextArray;
    private Image[,] imageArray;
    //private List<int> numbers;
    private List<int> numbersToAddToButtons;
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
    private int startingNumberOfMoves = 120;
    private int numberOfMovesLeft;
    public GameObject gridSpacePrefab;
    public GameObject canvasObject;
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
    private int totalPoints;
    private float targetTime;
    private List<GameObject> gridsList;
    private bool stillPlaying;
    private int pointsInThisRound;
    private int extraPointsForNumbers;
    private int minNumberOfGamesToNextLevel;
    private int maxNumberOfGamesToNextLevel;
    private int numOfGamesOnCurrentLevel;
    private bool trainingGame;
    private int howManyExtraNumbers;
    private int extraMovesForBigNumbers;
    private int numOfGamesToBlockOneButton;
    private int numberOfBlockedButtons;
    private int gameNumberFromWhichBlockingStarts = 4;
    private int pointsAtTheLevelBeginning;
    private int movesAtTheLevelBeginning;
    private int pointsToDecreaseForLevelRestart;
    //GAME ELEMENTS
    public GameObject gameElements;
    public Text timerValue;
    public Text numberOfMovesValue;
    public Text pointsValue;
    public Text pointsForRoundValue;
    public Text extraMovesForRoundValue;
    //MENU ELEMENTS
    public GameObject menuPanel;
    public Button startGameButton;
    public Button trainingButton;
    //AT BOARD END ELEMENTS
    public GameObject elementsAtBoardEnd;
    public Text pointsForRoundText;
    public Text restartLevelButtonText;
    //TRAINING ELEMENTS
    public GameObject trainingPanel;


    private void Awake()
    {
        InstantiateObjects();
        GoToMenu();
    }

    private void Update()
    {
        if (stillPlaying)
        {
            if (targetTime > 0)
            {
                targetTime -= Time.deltaTime;
                timerValue.text = Math.Round(targetTime, 0).ToString();
            }
        }
    }

    public void GoToMenu()
    {
        DestroyGrids();
        trainingGame = false;
        ShowOnlyMenu();
    }

    void ShowOnlyMenu()
    {
        MenuVisible(true);
        TrainingPanelVisible(false);
        GameElementsVisible(false);
        ElementsAtBoardEndVisible(false);
    }

    public void StartingGame()
    {
        StartGame(2, 2);
    }

    void StartGame(int columns, int rows)
    {
        BeforeWholeGame(columns, rows);
        BeforeNewLevel();
        BeforeNewBoard();
        ShowOnlyGameElements();
        ResetElementsAtBoardEnd();
    }

    void ShowOnlyGameElements()
    {
        MenuVisible(false);
        GameElementsVisible(true);
        ElementsAtBoardEndVisible(false);
        TrainingPanelVisible(false);
    }

    void BeforeWholeGame(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        numberOfMovesLeft = startingNumberOfMoves;
        SetArrowsNames();
        TimerActive(true);
    }

    void ResetElementsAtBoardEnd()
    {
        pointsValue.text = "0";
        totalPoints = 0;
        numberOfMovesValue.text = startingNumberOfMoves.ToString();
        timerValue.text = targetTime.ToString();
    }


    void BeforeNewLevel()
    {
        InstantiateVariables();
        ModifyVariablesValues();
        CalculateNumberOfDivisions();
        gridSize = CalculateGridSize();
        CalculateBoardSizes();
        SetNumberOfGamesToNextLevel();
        SaveValuesFromLevelBeginning();
        ModifySizeAndMovePrefabGridSize();
        CreateAndArrangeGrids();
        restartLevelButtonText.text = "Zacznij poziom " + columns + "x" + rows + " za " + pointsToDecreaseForLevelRestart + " punktów";
    }

    void SaveValuesFromLevelBeginning()
    {
        movesAtTheLevelBeginning = numberOfMovesLeft;
        pointsAtTheLevelBeginning = totalPoints;
    }

    void SetNumberOfGamesToNextLevel()
    {
        minNumberOfGamesToNextLevel = Math.Max(columns, rows);
        maxNumberOfGamesToNextLevel = columns + rows;
    }

    void BeforeNewBoard()
    {
        extraMovesForBigNumbers = 0;
        numOfGamesOnCurrentLevel++;
        InstantiateArrowsCopy();
        SetTimer();
        PrepareArrowsToUse();
        CopyNumbersToAddToButtons();
        AddNumbersToButtons();
        ColorCorrectNumbersOnButtons();
        AddArrowsToButtons();
        SetGameControllerReferenceOnButtons();
        AreButtonsInteractable(true);
        if (columns >= 4 && rows >= 3 && numOfGamesOnCurrentLevel >= gameNumberFromWhichBlockingStarts)
        {
            StartBlockingButtons();
        }
    }

    void StartBlockingButtons()
    {
        if (numOfGamesToBlockOneButton == 0)
        {
            CalculateNumOfGamesToBlockOneButton();
        }
        CalculateNumberOfBlockedButtons();
        BlockButtons();
    }

    void CalculateNumOfGamesToBlockOneButton()
    {
        if (columns == 4 && rows == 3)
        {
            numOfGamesToBlockOneButton = 6;
        }
        else if (columns == 4 && rows == 4)
        {
            numOfGamesToBlockOneButton = 4;
        }
        else if (columns == 5 && rows == 4)
        {
            numOfGamesToBlockOneButton = 2;
        }
        else if (columns == 5 && rows == 5)
        {
            numOfGamesToBlockOneButton = 1;
        }
    }

    void CalculateNumberOfBlockedButtons()
    {
        numberOfBlockedButtons = (numOfGamesOnCurrentLevel - gameNumberFromWhichBlockingStarts) / numOfGamesToBlockOneButton + 1;
    }

    void BlockButtons()
    {
        int rowsThatCanBeBlocked = rows - 2;
        int randomColumn;
        int randomRow;
        for (int i = 0; i < numberOfBlockedButtons; i++)
        {
            do
            {
                randomColumn = rand.Next(0, columns);
                randomRow = rand.Next(0, rowsThatCanBeBlocked);
            } while (buttonTextArray[randomColumn, randomRow].GetComponentInParent<Button>().IsInteractable() == false);
                buttonTextArray[randomColumn, randomRow].GetComponentInParent<Button>().interactable = false;
        }
    }

    void CopyNumbersToAddToButtons()
    {
        AddNumbersToList(numbersToAddToButtons);
    }

    private void InstantiateObjects()
    {
        if (rand == null)
        {
            rand = new System.Random();
            arrowsList = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
            arrowsUpDownLeftRight = new List<Sprite>(new Sprite[] { upArrow, leftArrow, rightArrow, downArrow });
            arrowsDiagonal = new List<Sprite>(new Sprite[] { upLeftArrow, upRightArrow, downLeftArrow, downRightArrow });
            arrowsToUse = new List<Sprite>();
            numbersToAddToButtons = new List<int>();
            gridsList = new List<GameObject>();
        }
    }

    void InstantiateArrowsCopy()
    {
        if (arrowsListCopy == null)
        {
            arrowsListCopy = new List<Sprite>(new Sprite[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
        } else
        {
            arrowsListCopy.Clear();
            for (int i = 0; i < arrowsList.Count; i++)
            {
                arrowsListCopy.Add(arrowsList[i]);
            }
        }
    }

    private void InstantiateVariables()
    {
        buttonTextArray = new Text[columns, rows];
        imageArray = new Image[columns, rows];
        numOfGamesOnCurrentLevel = 0;
        numberOfBlockedButtons = 0;
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

    void SetTimer()
    {
        targetTime = 20 + (rows + columns - 4) * 10;
    }
    
    void SetGameControllerReferenceOnButtons()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                buttonTextArray[x, y].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
                imageArray[x, y].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
                ColorBlock cb = buttonTextArray[x, y].GetComponentInParent<Button>().colors;
                cb.disabledColor = Color.grey;
                buttonTextArray[x, y].GetComponentInParent<Button>().colors = cb;
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
            pointsToDecreaseForLevelRestart = 50;
            nextLevelPointsLimit = board3x2limit;
        } else if (columns == 3)
        {
            gridAssetFromDivisions = 14;
            textSize = 70;
            arrowSize = 40;
            arrowMove = 45;
            if (rows == 2)
            {
                pointsToDecreaseForLevelRestart = 150;
                nextLevelPointsLimit = board3x3limit;
            }
            else if (rows == 3)
            {
                pointsToDecreaseForLevelRestart = 300;
                nextLevelPointsLimit = board4x3limit;
            }
        } else if (columns == 4)
        {
            gridAssetFromDivisions = 7;
            textSize = 60;
            arrowSize = 35;
            arrowMove = 40;
            if (rows == 3)
            {
                pointsToDecreaseForLevelRestart = 500;
                nextLevelPointsLimit = board4x4limit;
            }
            else if (rows == 4)
            {
                pointsToDecreaseForLevelRestart = 750;
                nextLevelPointsLimit = board5x4limit;
            }
        } else if (columns == 5)
        {
            gridAssetFromDivisions = 1;
            textSize = 40;
            arrowSize = 35;
            arrowMove = 30;
            if (rows == 4)
            {
                pointsToDecreaseForLevelRestart = 1050;
                nextLevelPointsLimit = board5x5limit;
            }
            else if (rows == 5)
            {
                pointsToDecreaseForLevelRestart = 1400;
            }
        }
    }

    void PrepareArrowsToUse()
    {
        arrowsToUse.Clear();
        int numOfButtons = columns * rows - 2;
        int allArrowsOccurrence = numOfButtons / arrowsList.Count;
        int extraArrows = numOfButtons % arrowsList.Count;
        arrowsToUse.Add(arrowsUpDownLeftRight[rand.Next(0, arrowsUpDownLeftRight.Count)]);
        arrowsToUse.Add(arrowsDiagonal[rand.Next(0, arrowsDiagonal.Count)]);
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
                    randomNumber = rand.Next(0, numbersToAddToButtons.Count);
                } while (y == (rows - 1) && randomNumber < columns);
                buttonTextArray[x, y].text = numbersToAddToButtons[randomNumber].ToString();
                numbersToAddToButtons.RemoveAt(randomNumber);
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

    public void MoveNumbersOnClick(Button button)
    {
        string number = button.GetComponentInChildren<Text>().text;
        Sprite arrow = button.GetComponentsInChildren<Image>()[1].sprite;
        FindButtonIndexes(number);
        ChangeButtonsPlaces(number);
        ChangeArrowsPlaces(arrow);
        ColorActiveButtons(clickedButtonX, clickedButtonY, buttonTextArray[clickedButtonX, clickedButtonY].text);
        ColorActiveButtons(newPositionX, newPositionY, buttonTextArray[newPositionX, newPositionY].text);
        CheckIfGameFinished(button);
    }

    void FindButtonIndexes(string number)
    {
        String buttonName = EventSystem.current.currentSelectedGameObject.name;
        clickedButtonX = Int32.Parse(buttonName.Substring(4, 1));
        clickedButtonY = Int32.Parse(buttonName.Substring(5, 1));
        clickedButtonArrowName = imageArray[clickedButtonX, clickedButtonY].sprite.name;
    }

    void ChangeButtonsPlaces(string clickedButtonNumber)
    {
        int moveValue = 1;
        int[] arrowTransition = ChangeLocation(clickedButtonArrowName, moveValue);
        newPositionX = (clickedButtonX + arrowTransition[0] + columns) % columns;
        newPositionY = (clickedButtonY + arrowTransition[1] + rows) % rows;
        newButtonNumber = buttonTextArray[newPositionX, newPositionY].text;
        newImageArrow = imageArray[newPositionX, newPositionY].sprite;
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

    void CheckIfGameFinished(Button button)
    {
        numberOfMovesLeft--;
        numberOfMovesValue.text = numberOfMovesLeft.ToString();
        if (numberOfMovesLeft <= 0)
        {
            EndOfGame();
        }
        else if (buttonTextArray[0, rows - 1].text == "1" && buttonTextArray[1, rows - 1].text == "2" && ((columns == 2 && buttonTextArray[0, 0].text == "3") || (columns > 2 && buttonTextArray[2, rows - 1].text == "3")))
        {
            EndOfBoard();
        }  
    }

    void EndOfBoard()
    {
        BlockBoard();
        UpdatePoints();
        CalculateNumberOfExtraMoves();
        pointsForRoundValue.text = pointsInThisRound.ToString();
        extraMovesForRoundValue.text = extraMovesForBigNumbers.ToString();
        ElementsAtBoardEndVisible(true);
    }

    void EndOfGame()
    {
        BlockBoard();
    }

    void BlockBoard()
    {
        AreButtonsInteractable(false);
        ColorCorrectNumbersDisabledColor();
        TimerActive(false);
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

    void ColorActiveButtons(int x, int y, string buttonNumber)
    {
        if (Int32.Parse(buttonNumber) == ((x + 1) + Math.Abs(y - (rows - 1)) * columns))
        {
            ChangeColor(buttonTextArray[x, y], Color.green);
        } else
        {
            ChangeColor(buttonTextArray[x, y], Color.white);
        }
    }

    void ColorCorrectNumbersDisabledColor()
    {
        extraPointsForNumbers = 0;
        howManyExtraNumbers = 0;
        Action work = delegate
        {
            for (int y = (rows - 1); y >= 0; y--)
            {
                for (int x = 0; x < columns; x++)
                {
                    int correctNumberOnField = (x + 1) + Math.Abs(y - (rows - 1)) * columns;
                    if (buttonTextArray[x, y].text == correctNumberOnField.ToString())
                    {
                        Button b = buttonTextArray[x, y].GetComponentInParent<Button>();
                        ColorBlock cb = b.colors;
                        cb.disabledColor = Color.green;
                        b.colors = cb;
                        int bigNumber = Int32.Parse(buttonTextArray[x, y].text);
                        if (bigNumber > 3 && columns >= 3)
                        {
                            howManyExtraNumbers++;
                            extraPointsForNumbers += (int)Math.Pow(bigNumber, 2);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        };
        work();
    }

    void UpdatePoints()
    {
        pointsInThisRound = (int)Math.Pow(rows * columns, 2) / 2 + (int)Math.Round(targetTime, 0) * Math.Min(rows, columns) + extraPointsForNumbers * Math.Min(rows, columns);
        totalPoints += pointsInThisRound;
        pointsValue.text = totalPoints.ToString();
    }

    void ChangeColor(Text text, Color color)
    {
        Button b = text.GetComponentInParent<Button>();
        ColorBlock cb = b.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        b.colors = cb;
    }

    public void AfterEachBoard()
    {
        numberOfMovesValue.text = numberOfMovesLeft.ToString();
        if (!trainingGame)
        {
            CheckIfNewLevel();
        }
        TimerActive(true);
        ElementsAtBoardEndVisible(false);
        AreButtonsInteractable(true);
        BeforeNewBoard();
    }

    void CalculateNumberOfExtraMoves()
    {
        extraMovesForBigNumbers += 5;
        for (int i = 0; i < howManyExtraNumbers; i++)
        {
            if (i == 0)
            {
                extraMovesForBigNumbers += Math.Min(columns, rows);
            } else
            {
                extraMovesForBigNumbers += Math.Max(columns, rows);
            }
        }
        numberOfMovesLeft = numberOfMovesLeft + extraMovesForBigNumbers;
    }

    void TimerActive(bool isActive)
    {
        stillPlaying = isActive;
    }

    void CheckIfNewLevel()
    {
        if (numOfGamesOnCurrentLevel >= minNumberOfGamesToNextLevel)
        {
            if (numOfGamesOnCurrentLevel >= maxNumberOfGamesToNextLevel || totalPoints > nextLevelPointsLimit)
            {
                if (columns == rows)
                {
                    SetColumnsAndRows((columns + 1), rows);
                } else
                {
                    SetColumnsAndRows(columns, (rows + 1));
                }
            }
        }
    }

    void SetColumnsAndRows(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        DestroyGrids();
        BeforeNewLevel();
    }

    void AreButtonsInteractable(bool areInterablable)
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                buttonTextArray[i, j].GetComponentInParent<Button>().interactable = areInterablable;
            }
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

    public void RestartGame()
    {
        DestroyGrids();
        if (trainingGame)
        {
            StartGame(columns, rows);
        } else
        {
            StartGame(2, 2);
        }
    }

    public void GoToTraining()
    {
        ShowOnlyTrainingPanel();
    }

    void ShowOnlyTrainingPanel()
    {
        MenuVisible(false);
        TrainingPanelVisible(true);
        ElementsAtBoardEndVisible(false);
        GameElementsVisible(false);
    }

    public void Training2x2()
    {
        StartTraining(2, 2);
    }

    public void Training3x2()
    {
        StartTraining(3, 2);
    }

    public void Training3x3()
    {
        StartTraining(3, 3);
    }

    public void Training4x3()
    {
        StartTraining(4, 3);
    }

    public void Training4x4()
    {
        StartTraining(4, 4);
    }

    public void Training5x4()
    {
        StartTraining(5, 4);
    }

    public void Training5x5()
    {
        StartTraining(5, 5);
    }

    void StartTraining(int boardColumns, int boardRows)
    {
        trainingGame = true;
        StartGame(boardColumns, boardRows);
    }

    public void RestartCurrentLevel()
    {
        totalPoints = pointsAtTheLevelBeginning - pointsToDecreaseForLevelRestart;
        pointsAtTheLevelBeginning = totalPoints;
        if (totalPoints <= 0)
        {
            totalPoints = 0;
        }
        numberOfMovesLeft = movesAtTheLevelBeginning;
        numOfGamesOnCurrentLevel = 0;
        pointsValue.text = totalPoints.ToString();
        numberOfMovesValue.text = numberOfMovesLeft.ToString();
        BeforeNewBoard();
        timerValue.text = targetTime.ToString();
        ElementsAtBoardEndVisible(false);
    }

    void DestroyGrids()
    {
        for (int i = 0; i < gridsList.Count; i++)
        {
            Destroy(gridsList[i]);
        }
    }

    void TrainingPanelVisible(bool isVisible)
    {
        trainingPanel.SetActive(isVisible);
    }

    void GameElementsVisible(bool isVisible)
    {
        gameElements.SetActive(isVisible);
    }

    void ElementsAtBoardEndVisible(bool isVisible)
    {
        elementsAtBoardEnd.SetActive(isVisible);
    }

    void MenuVisible(bool isVisible)
    {
        menuPanel.SetActive(isVisible);
    }

}