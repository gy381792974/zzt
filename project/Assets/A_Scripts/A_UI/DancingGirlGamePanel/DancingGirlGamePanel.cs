using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class DancingGirlGamePanelData : UIDataBase
    {
        public int mHashCode;

        public DancingGirlGamePanelData(int mHashCode)
        {
            this.mHashCode = mHashCode;
        }
    }

    public partial class DancingGirlGamePanel : UIBase
    {
        List<int> meUserGridList = new List<int>();
        List<int> otherUserGridList = new List<int>();
        RoleDialoguePanel dialoguePanel;
        private int[,] state = new int[3, 3];
        bool isDebug = false;

        public Transform grid;
        [ContextMenu("InitComponent")]
        private void InitComponent()
        {
            dGItemList.Clear();

            for (int i = 0; i < grid.childCount; i++)
            {
                dGItemList.Add(grid.GetChild(i).GetComponent<DancingGirlGameItem>());
            }
        }

        protected override void OnInit()
        {
            dialoguePanel = UIMgr.GetUI<RoleDialoguePanel>();
            closeBtn.onClick.AddListener(() =>
            {
                UIMgr.HideUI<DancingGirlGamePanel>();
            });
        }

        protected override void OnShow(UIDataBase dancinggirlgamepanelData = null)
        {
            if (dancinggirlgamepanelData != null)
            {
                mPanelData = dancinggirlgamepanelData as DancingGirlGamePanelData;
            }

            meUserGridList.Clear();
            otherUserGridList.Clear();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    state[i, j] = 0;
                }
            }

            for (int i = 0; i < dGItemList.Count; i++)
            {
                dGItemList[i].BindData(0, i);
                dGItemList[i].SetOnClickHandle(OnClickGridHandle);
            }
        }

        protected override void OnHide()
        {

        }


        private void OnClickGridHandle(int index)
        {
            AddMeUseGrid(index);

            if (IsFillCondition(1))
            {
                Victory();
            }
            else
            {
                index = GetOtherIndex();

                if (index != -1)
                {
                    dGItemList[index].BindData(2);

                    AddOtherUseGrid(index);

                    if (IsFillCondition(2))
                    {
                        Fail();
                    }
                }
                else
                {
                    Fail();
                }
            }
        }

        private int GetOtherIndex()
        {
            List<int> data = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            for (int i = 0; i < meUserGridList.Count; i++)
            {
                if (data.Contains(meUserGridList[i]))
                {
                    data.Remove(meUserGridList[i]);
                }
            }

            for (int i = 0; i < otherUserGridList.Count; i++)
            {
                if (data.Contains(otherUserGridList[i]))
                {
                    data.Remove(otherUserGridList[i]);
                }
            }

            if (data.Count > 1)
            {
                int index = data[UnityEngine.Random.Range(0, data.Count)];
                if (isDebug)
                {
                    string log = "";
                    for (int i = 0; i < data.Count; i++)
                    {
                        log += data[i] + " ";
                    }

                    Debug.LogError("  index: " + index + "  data " + log);
                }
                return index;
            }

            return -1;
        }

        private void AddMeUseGrid(int index)
        {
            state[index / 3, index % 3] = 1;

            if (!meUserGridList.Contains(index))
            {
                meUserGridList.Add(index);
            }
            else
            {
                Debug.LogError("string str" + index);
            }
        }

        public void AddOtherUseGrid(int index)
        {
            state[index / 3, index % 3] = 2;

            if (!otherUserGridList.Contains(index))
            {
                otherUserGridList.Add(index);
            }
            else
            {
                Debug.LogError("AddOtherUseGrid " + index);
            }
        }

        private bool IsFillCondition(int type)
        {
            return type == CheckGrid();
        }

        private bool SortData(int x, int y)
        {
            if (x <= y)
            {
                return true;
            }

            return false;
        }

        private void Victory()
        {
            MusicMgr.Instance.PlayMusicEff("d_guests_d_win");
            UIMgr.ShowPanel<RoleDialoguePanel>();
            dialoguePanel.ShowVictoryDialogue();
            UIMgr.HideUI<DancingGirlGamePanel>();
            EventManager.Instance.TriggerEvent(EventKey.DancingGameVictory, 3);//触发游戏胜利事件
            EventManager.Instance.TriggerEvent(EventKey.DancingPlayAction, mPanelData.mHashCode);
        }

        private void Fail()
        {
            MusicMgr.Instance.PlayMusicEff("d_guests_d_lose");
            UIMgr.ShowPanel<RoleDialoguePanel>();
            dialoguePanel.ShowFailDialogue();
            UIMgr.HideUI<DancingGirlGamePanel>();
            EventManager.Instance.TriggerEvent(EventKey.SpeRoleLeave, mPanelData.mHashCode);
        }
        int CheckGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
                    return state[i, 0];
            }
            for (int i = 0; i < 3; i++)
            {
                if (state[0, i] != 0 && state[0, i] == state[1, i] && state[1, i] == state[2, i])
                    return state[0, i];
            }
            int flag = state[0, 0];
            int flag2 = state[0, 2];
            for (int i = 0; i < 3; i++)
            {
                if (state[i, i] != flag && state[i, 2 - i] != flag2)
                    return 0;
            }
            if (flag == state[2, 2])
                return flag;
            else
                return flag2;
        }
    }
}
