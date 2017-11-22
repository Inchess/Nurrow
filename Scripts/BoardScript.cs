using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour {

    private List<Sprite> arrowsList;
    private GameController gameController;

    private void Awake()
    {
    }

    public void Abc(GameController controller)
    {
        gameController = controller;
    }

}
