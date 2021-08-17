using UnityEngine;
using UnityEditor;

namespace EazyGF
{
    public class LocalCommonUtil
    {
        public int type; //1建筑金币的收集 2飘字 3取餐等待 4露台给小费
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


        public static int TipMultipleNor(int tipMultiple)
        {
            if (tipMultiple <= -1)
            {
                tipMultiple = 1;
            }

            return tipMultiple;
        }

        public static int GetBuildEquipCollectId(int[] id)
        {
            return id[0] * 100 + id[1];
        }

        public static int[] GetBuildEquipCollectIds(int id)
        {
            return new int[] {id/100, id % 100};
        }

        //得到有多个孩子的trasform
        public static Transform GetTfTyByChildNum(Transform grid, int childNum = 3)
        {
            Transform tf = grid;
            while (tf.childCount < childNum)
            {
                tf = tf.GetChild(0);

                if (tf == null)
                {
                    Debug.LogError("获得子物体错误");
                    return null;
                }
            }

            return tf;
        }
    }
}