using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class BuildStall : BuildItem
    {
        public List<SkeletonAnimation> sas;

        private bool isPlayAni = false;

        public void SetSkeletonDataAsset(Stall_PropertyBase table, int level = 0)
        {
            if (!isPlayAni)
            {
                for (int i = 0; i < sas.Count; i++)
                {
                    sas[i].gameObject.SetActive(false);
                }

                return;
            }

            Vector3 pos = Vector3.zero;

            id = table.ID;

            float[][] aniPos = new float[3][];
            aniPos[0] = table.SPPos1;
            aniPos[1] = table.SPPos2;
            aniPos[2] = table.SPPos3;

            int[][] aniRota = new int[3][];
            aniRota[0] = table.SPRota1;
            aniRota[1] = table.SPRota2;
            aniRota[2] = table.SPRota3;

            for (int i = 0; i < sas.Count; i++)
            {
                if (i == 0)
                {
                    pos = buildPos[table.Level - 1].GetChild(0).GetComponentInChildren<MeshRenderer>().bounds.center;
                }

                SkeletonAnimation sa = sas[i];
                
                if (table.SPAssetName[i] != "0")
                {
                    sa.gameObject.SetActive(true);

                    sa.skeletonDataAsset = CustomerLogic.LoadSkeletonDataAsset(table.SPBundleName, table.SPAssetName[i]);
                    sa.Initialize(true);

                    Vector3 mPos = new Vector3(aniPos[i][0], aniPos[i][1], aniPos[i][2]);
                    Vector3 mRota = new Vector3(aniRota[i][0], aniRota[i][1], aniRota[i][2]);

                    if (Vector3.Distance(mPos, Vector3.zero) <= 0.01f)
                    {
                        sa.transform.position = pos;
                    }
                    else
                    {
                        sa.transform.localPosition = mPos;
                        sa.transform.localScale = Vector3.one * table.SPScale[i];
                        sa.transform.localEulerAngles = mRota;
                    }
                   
                    PlayAni(sa, aniName, enterNum > 0);
                }
                else
                {
                    sa.gameObject.SetActive(false);
                }   
            }
        }

        private int enterNum;

        private void PlayAni(SkeletonAnimation sa, string ani, bool isLoop)
        {
            return;

            if (sa != null)
            {
                sa.AnimationState.SetAnimation(0, ani, isLoop);
            }
            else
            {
                for (int i = 0; i < sas.Count; i++)
                {
                    if (sas[i].gameObject.activeSelf)
                    {
                        //if (sas[i].loop != isLoop && sas[i].loop == false)
                        //{
                        //}
                        //sas[i].Initialize(true);

                        sas[i].AnimationState.SetAnimation(0, ani, isLoop);
                    }
                }
            }
        }

        private string aniName = "animation";
        private bool isLock = false;

        public void MOnTriggerEnter(Collider other)
        {
            if (!isUnLock || isLock)
            {
                return;
            }

            if (enterNum % 10 == 0)
            {
                PlayAni(null, aniName, true);
            }

            enterNum++;
        }

        public void MOnTriggerExit(Collider other)
        {
            if (!isUnLock || isLock)
            {
                return;
            }

            enterNum--;
            if (enterNum <= 0)
            {
                PlayAni(null, aniName, false);
            }
        }
    }
}
