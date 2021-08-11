using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AutoHide : MonoBehaviour
{
    [SerializeField]private float hideTime = 1;
    private float curTime = 0;
    public void OnShow()
    {
        if (curTime <= 0)
        {
            gameObject.SetActive(true);
        }
        curTime = hideTime; 
    }
    private void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
