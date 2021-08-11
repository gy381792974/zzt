using EazyGF;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoraPanelControl : MonoBehaviour
{
    Toggle DecoraToggle;
    public GameObject Stallpanel;
    public GameObject Decorapanel;
    public GameObject Facilitypanel;
    private void Start()
    {
        DecoraToggle = GetComponent<Toggle>();
        DecoraToggle.onValueChanged.AddListener((bool value) => OnToggleClick());
    }

 

    public void OnToggleClick()
    {
        Decorapanel.SetActive(true);
        Stallpanel.SetActive(false);   
        Facilitypanel.SetActive(false);
 
        if (DecoraToggle.isOn == true)
        {
            GlobeFunction.isOpenStar = true;
        }
    }

}
