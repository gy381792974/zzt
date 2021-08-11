using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StallToggleControl : MonoBehaviour
{
 
    Toggle StallBtntoggle;
    public GameObject Stallpanel;
    public GameObject Decorapanel;
    public GameObject Facilitypanel;
   
    private void Start()
    {
        StallBtntoggle = GetComponent<Toggle>();
        StallBtntoggle.onValueChanged.AddListener((bool value) => OnToggleClick());
      
    }

 

    public void OnToggleClick()
    {
        Stallpanel.SetActive(true);
        Decorapanel.SetActive(false);
        Facilitypanel.SetActive(false);

        if (StallBtntoggle.isOn == true)
        {

            GlobeFunction.isOpenStar = false;
        }
       
    }
}
