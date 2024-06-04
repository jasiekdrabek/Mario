using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPanels : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject liveLostScreen = Resources.Load<GameObject>("LifeLostScreen");
        GameObject nextLevelScreen = Resources.Load<GameObject>("NextLevelScreen");
        Instantiate(liveLostScreen, this.transform);
        Instantiate(nextLevelScreen, this.transform);
        liveLostScreen.SetActive(false);
        nextLevelScreen.SetActive(false);
    }
}
