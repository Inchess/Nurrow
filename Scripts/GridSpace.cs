using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

    public Button button;
    public Text buttonText;

    private int[,] numbers;
    private int[,] numbersLeft;

    private GameController gameController;

    private void Awake()
    {
        numbers = new int[3, 3];
        numbersLeft = numbers;
    }

    public void SetSpace()
    {

    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

}
