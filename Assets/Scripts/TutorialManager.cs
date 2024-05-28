using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int popUpIndex = 0;
    public GameObject tutorialImg;
    public Canvas mainCanvas;
    TextMeshProUGUI popUp;
    bool isDisplayed = false;
    string[] texts = {"To move a player press <- or ->",
        "To jump press space or up",
        "Now go and kill enemy by falling on its head",
        "Aviod spikes they can kill you",
        "Wait for platform to advance, don't fall or you die",
        "First powerUp give you more lives, second makes you bigger and last one make you invincible against enemies for 10 seconds",
        "Now grab a flag pole and finish level. Have fun!"};
    // Start is called before the first frame update
    void Start()
    {
        Transform tutorialImage = Instantiate(tutorialImg.transform, mainCanvas.transform);
        popUp = tutorialImage.Find("TutorialText").GetComponent<TextMeshProUGUI>(); ;
        popUpIndex = 0;
        //GameObject.Find("Tutorial 1").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameObject.Find("Mario")) return;
        for(int i = 0; i< texts.Length; i++)
        {
            if(i == popUpIndex && !isDisplayed)
            {
                popUp.text = texts[popUpIndex];
                isDisplayed = true;
            }
        }
        if (popUpIndex == 6) return;
        if (popUpIndex == 0)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                popUpIndex = 1;
                isDisplayed = false;
            }
        }
        if(popUpIndex == 1)
        {
            if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space))
            {
                popUpIndex = 2;
                isDisplayed = false;
            }
        }
        if (popUpIndex == 2)
        {
            if (!GameObject.Find("Goomba") || GameObject.Find("Mario").transform.position.x > 5.5f)
            {
                popUpIndex = 3;
                isDisplayed = false;
            }
        }
        if (popUpIndex == 3)
        {
            if (GameObject.Find("Mario").transform.position.x > 19f)
            {
                popUpIndex = 4;
                isDisplayed = false;
            }
        }
        if (popUpIndex == 4)
        {
            if (GameObject.Find("Mario").transform.position.x > 44f)
            {
                popUpIndex = 5;
                isDisplayed = false;
            }
        }
        if (popUpIndex == 5)
        {
            if (GameObject.Find("Mario").transform.position.x > 64f)
            {
                popUpIndex = 6;
                isDisplayed = false;
            }
        }
    }
}
