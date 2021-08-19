using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class DoorToiletAniCtr : MonoBehaviour
    {
        public enum DorState
        {
            Open = 0,
            Close,
        }

        BuildItem buildItem;

        private void Start()
        {
            buildItem = GetComponent<BuildItem>();
            EventManager.Instance.RegisterEvent(EventKey.PlayDoorAni, PlayDoorAni);
        }

        private void PlayDoorAni(object obj)
        {
            int[] datas = (int[])obj;

            Transform doorTf = buildItem.GetShowBuildTf().GetChild((int)datas[1]).GetChild(0);

            doorTf.DOPause();

            if (datas[0] == (int)DorState.Open)
            {
                doorTf.DOLocalMoveY(-0.04f, 0.4f).SetEase(Ease.Linear);
            }
            else if (datas[0] == (int)DorState.Close)
            {
                doorTf.DOLocalMoveY(0f, 0.4f).SetEase(Ease.Linear);
            }
        }
    }
}