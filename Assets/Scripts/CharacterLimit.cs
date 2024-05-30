using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField mainInputField;
    public int length = 10;

    void Start()
    {
        //Changes the character limit in the main input field.
        mainInputField.characterLimit =length;
    }
}
