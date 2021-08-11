using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{

    public partial class CubeGrids : MonoBehaviour
    {
        [SerializeField] private Transform selectFrame1;
        [SerializeField] private Transform selectFrame2;
        [SerializeField] private List<CubeLayout> cubeLayouts;

        [SerializeField] private Transform leftFlyTf;
        [SerializeField] private Transform rightFlyTf;
        [SerializeField] private Transform flyTfEffect;

        public Transform grid;

        [ContextMenu("InitComponent")]
        private void InitComponent()
        {
            cubeLayouts.Clear();

            for (int i = 0; i < grid.childCount; i++)
            {
                cubeLayouts.Add(grid.GetChild(i).GetComponent<CubeLayout>());
            }
        }

        Vector3[] layoutOgrPos;

        public void OnInit()
        {
            CubeGameMgr.Instance.InitComponent(transform, selectFrame1, selectFrame2, leftFlyTf, rightFlyTf, flyTfEffect);

            layoutOgrPos = new Vector3[1];

            layoutOgrPos[0] = cubeLayouts[0].transform.position;

            //for (int i = 0; i < cubeLayouts.Count; i++)
            //{
            //    layoutOgrPos[i] = cubeLayouts[i].transform.position;
            //}
        }

       
        public void OnShow()
        {
            EventManager.Instance.RegisterEvent(EventKey.UpdateCubeEvent, UpdateCubeEvent);

            List<Cube_Property> levelData = CubeGameMgr.Instance.GetCurLevelData();
            MusicMgr.Instance.PlayMusicEff("c_map_load");
            int[][] ids = CubeGameMgr.Instance.GetCubeItemIds();
            
            for (int i = cubeLayouts.Count - 1; i >= 0; i--)
            {
                if (i < levelData.Count)
                {
                    cubeLayouts[i].gameObject.SetActive(false);

                    cubeLayouts[i].BindData(levelData[i], i, ids[i]);

                    cubeLayouts[i].transform.localPosition = layoutOgrPos[0] +  new Vector3(levelData[i].PosX, levelData[i].PosY , 0);
                }
                else
                {
                    cubeLayouts[i].gameObject.SetActive(false);
                }
            }

            dataNum = levelData.Count;
            UIMgr.ShowPanel<MaskPanel>();
            FlyGridLayout(() => {
                UIMgr.HideUI<MaskPanel>();
            });
        }

        int dataNum = 0;

        Action flyEndHand;

        public void FlyGridLayout(Action flyEndHand = null)
        {
            this.flyEndHand = flyEndHand;

            for (int i = 0; i < dataNum; i++)
            {
                cubeLayouts[i].gameObject.SetActive(false);
            }

            StartCoroutine(StartFly(dataNum));
        }

        IEnumerator StartFly(int count)
        {
            //yield return new WaitForSeconds(2);

            int dis = 10;

            for (int i = 0; i < count; i++)
            {
                Transform tf = cubeLayouts[i].transform;
                
                tf.gameObject.SetActive(true);

                Vector3 orgPos = tf.position;
                int index = i;
                if (index % 4 == 0)
                {
                    tf.position = orgPos + Vector3.up * dis;
                }
                if (index % 4 == 1)
                {
                    tf.position = orgPos + Vector3.left * dis;
                }
                if (index % 4 == 2)
                {
                    tf.position = orgPos + -Vector3.up * dis;
                }
                if (index % 4 == 3)
                {
                    tf.position = orgPos + Vector3.right * dis;
                }

                tf.DOMove(orgPos, 0.3f).SetEase(Ease.Linear);

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.2f * count);

            for (int i = 0; i < cubeLayouts.Count; i++)
            {
                cubeLayouts[i].SetCubeLayoutLockInfo();
            }

            if (flyEndHand != null)
            {
                flyEndHand();
                flyEndHand = null;
            }
        }

        public void OnHide()
        {
            EventManager.Instance.RemoveListening(EventKey.UpdateCubeEvent, UpdateCubeEvent);

            selectFrame1.SetParent(transform);
            selectFrame2.SetParent(transform);

            selectFrame1.gameObject.SetActive(false);
            selectFrame2.gameObject.SetActive(false);

            for (int i = 0; i < cubeLayouts.Count; i++)
            {
                cubeLayouts[i].RemoveAllItem();
            }
        }

        private void UpdateCubeEvent(object arg0)
        {
            CubeItem cubeItem = (CubeItem)arg0;

            cubeLayouts[cubeItem.MLayout].RemoveItem(cubeItem);

            CubeGameMgr.Instance.IsVictory();

            //if (cubeItem.canLockIndexs == null)
            //{
            //    return;
            //}

            if (cubeItem.MLayout > 0)
            {
                //cubeLayouts[cubeItem.MLayout - 1].UnLockItem(CubeGameMgr.Instance.GetOffData(cubeItem.PosIndex, cubeItem.MLayout, cubeItem.MLayout - 1));

                for (int i = cubeItem.MLayout - 1; i >= 0; i--)
                {
                    cubeLayouts[i].UnLockItem(CubeGameMgr.Instance.GetOffData(cubeItem.PosIndex, cubeItem.MLayout, i));
                }
            }
        }
    }
}
