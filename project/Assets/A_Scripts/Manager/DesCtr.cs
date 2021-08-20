using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesCtr : MonoBehaviour
{
    public static bool isInit = false;

    private void Awake()
    {
        if (!isInit)
        {
            isInit = true;
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}
