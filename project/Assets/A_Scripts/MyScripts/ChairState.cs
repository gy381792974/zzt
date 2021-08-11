using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairState : MonoBehaviour
{
    [Header("是否坐下=是否空位")]
    [SerializeField]
    public bool isSit = false;

    public bool IsSit
    {
        get{ return isSit; }
        set{ isSit =value; }  
     }
}
