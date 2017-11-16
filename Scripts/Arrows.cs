using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrows : MonoBehaviour {

    private List<Sprite> arrowsList;
    private GameController gameController;

    private void Awake()
    {
    }

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }

}
