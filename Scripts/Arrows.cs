using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrows : MonoBehaviour {

    public Sprite upArrow;
    public Image upLeftArrow;
    public Image upRightArrow;
    public Image leftArrow;
    public Image rightArrow;
    public Image downArrow;
    public Image downLeftArrow;
    public Image downRightArrow;

    private List<Image> arrowsList;

    private void Awake()
    {
        arrowsList = new List<Image>(new Image[] { upArrow, upLeftArrow, upRightArrow, leftArrow, rightArrow, downArrow, downLeftArrow, downRightArrow });
    }

    public List<Image> GetArrowsList()
    {
        return arrowsList;
    }

}
