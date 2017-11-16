using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

    public Button button;
    public Text buttonText;

    private GameController gameController;

    public void UpdateGridSpace()
    {
        gameController.MoveNumbersOnClick(buttonText);
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

}
