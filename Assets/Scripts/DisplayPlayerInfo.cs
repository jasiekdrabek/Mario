using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayerInfo : MonoBehaviour
{
    void Awake()
    {
        GameObject info = Resources.Load<GameObject>("Info");
        Instantiate(info, this.transform);
    }
}
