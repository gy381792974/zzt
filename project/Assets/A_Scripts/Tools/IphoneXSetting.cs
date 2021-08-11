using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IphoneXSetting : MonoBehaviour
{
    public int Hight = 45;
    void Awake()
    {
#if UNITY_EDITOR || UNITY_IOS
        if (Screen.height / (float)Screen.width > 2.1f)//IponeX
        {
            transform.localPosition-=new Vector3(0, Hight, 0);
        }
#endif
    }
	
}
