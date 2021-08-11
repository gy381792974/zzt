using EazyGF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 全局域
/// </summary>
/// 
public class GlobeFunction : Singleton<GlobeFunction>
{
    //public delegate void VoidDelegate();
    //public static event VoidDelegate AddGold;
    public static event Action AddGold;
    static public bool isOpenStar;
    static public bool isBuyGoods;
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //  ADDGold();
    //        //AddGold();    
    //    }
    //}


  
    //void ADDGold()
    //{
    //    long g= PlayerDataMgr.g_playerData.Gold += 10;
    //    UIMgr.GetUI<SalaPanel>().UpdataGoldNum(g);       
    //}
}
