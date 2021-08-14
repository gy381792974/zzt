using UnityEngine;
using UnityEditor;

namespace EazyGF
{
    public class LocalCommonUtil
    {
       public static void ShowBB(int type, Transform tf, int id, int num = 0, string txt = "")
        {
            CBBData cBBData = new CBBData();

            cBBData.type = type;
            cBBData.tf = tf;

            cBBData.id = id;
            cBBData.num = num;
            cBBData.txt = txt;

            EventManager.Instance.TriggerEvent(EventKey.SendCBBData, cBBData);
        }
    }
}