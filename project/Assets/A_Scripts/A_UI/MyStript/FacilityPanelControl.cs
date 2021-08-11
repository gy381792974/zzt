using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityPanelControl : MonoBehaviour
{
    Toggle FacilityToggle;
    public GameObject Stallpanel;
    public GameObject Decorapanel;
    public GameObject Facilitypanel;

    private void Start()
    {
        FacilityToggle = GetComponent<Toggle>();
        FacilityToggle.onValueChanged.AddListener((bool value) => OnToggleClick());
    }



    public void OnToggleClick()
    {
        Facilitypanel.SetActive(true);
        Stallpanel.SetActive(false);
        Decorapanel.SetActive(false);       

        //if (FacilityToggle.isOn == true)
        //{
        //    GlobeFunction.isOpenStar = true;
        //}
    }
}
