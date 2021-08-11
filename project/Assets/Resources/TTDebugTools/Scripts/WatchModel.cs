using EazyGF;
using HunliGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchModel : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;

    void Awake()
    {
        EventManager.Instance.RegisterEvent(EventKey.Null, CheckModelNumEvent);

        CheckModelNumEvent(null);
    }

    //检车模型更新事件
    private void CheckModelNumEvent(object arg0)
    {
        //text.text = "Model Num: " + BattleManager.Instance.GetModelCount().ToString();
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListening(EventKey.Null, CheckModelNumEvent);
    }

}
