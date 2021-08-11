using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace EazyGF
{
    public class MainSpace : MonoBehaviour
    {
        public Transform stallGrid;

        public Transform equipGrid;

        public Transform adornGrid;

        public List<BuildStall> stallList;
        
        public List<BuildEquip> equipList;

        public List<BuildAdorn> adornList;

        public static MainSpace Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        { 
            Dictionary<int, BuildDataModel> userBuildDir = BuildMgr.userBuildDir;

            foreach (var item in userBuildDir)
            {
                UpdateBuildModelShow(item.Value);
            }

            EventManager.Instance.RegisterEvent(EventKey.SelectBuildEvent, UpdateUIInfo);

            InitBubbleCollectBB();
        }

        public void InitBubbleCollectBB()
        {
            Dictionary<int, int> bcDir = BuildCollectMgr.Instance.bCollctCoinDic;

            foreach (var item in bcDir)
            {
                if (item.Value > BuildCollectMgr.cCoinMinShowNum)
                {
                    CBBData cBBData = new CBBData();

                    int posIndex = BuildMgr.GetStallPosById(item.Key);

                    if (posIndex == -1)
                    {
                        continue;
                    }

                    cBBData.type = 1;
                    cBBData.tf = MainSpace.Instance.stallList[posIndex - 1].GetShowBuildBoxTf();

                    cBBData.id = item.Key;
                    cBBData.num = item.Value;

                    EventManager.Instance.TriggerEvent(EventKey.SendCBBData, cBBData);
                }
            }
        }

        public void UnLockALLBuildFun()
        {
            Dictionary<int, BuildDataModel> userBuildDir = BuildMgr.userBuildDir;

            foreach (var item in userBuildDir)
            {
                UpdateBuildModelShow(item.Value);
            }

            EventManager.Instance.RegisterEvent(EventKey.SelectBuildEvent, UpdateUIInfo);
        }

        private void OnEnable()
        {
            EventManager.Instance.RegisterEvent(EventKey.EnterGetFoodPos, EnterStallEvent);
            EventManager.Instance.RegisterEvent(EventKey.LeaveGetGoodPos, LevelStallEvent);
            EventManager.Instance.RegisterEvent(EventKey.BuildComDataUdpate, BuildComDataUdpate);

            EventManager.Instance.RegisterEvent(EventKey.BuildAreaDataUpdate, BuildAreaDataUpdate);
        }

        private void OnDisable()
        {
            EventManager.Instance.RemoveListening(EventKey.EnterGetFoodPos, EnterStallEvent);
            EventManager.Instance.RemoveListening(EventKey.LeaveGetGoodPos, LevelStallEvent);
            EventManager.Instance.RemoveListening(EventKey.BuildComDataUdpate, BuildComDataUdpate);

            EventManager.Instance.RemoveListening(EventKey.BuildAreaDataUpdate, BuildAreaDataUpdate);
        }

        private void EnterStallEvent(object arg0)
        {
            int stallID = (int)arg0;

            PlayStallAni(stallID, true);
        }

        private void LevelStallEvent(object arg0)
        {
            int stallID = (int)arg0;

            PlayStallAni(stallID, false);
        }

        private void BuildComDataUdpate(object arg0)
        {
            BuildDataModel buildDataModel = (BuildDataModel)arg0;

            if (buildDataModel.Type == 2)
            {
                for (int i = 0; i < equipList.Count; i++)
                {
                    if (equipList[i].MBuildDataModel.Id == buildDataModel.Id)
                    {
                        equipList[i].ShowComBuild(buildDataModel.Pos, buildDataModel.Level);

                        equipList[i].PlayComEffect(buildDataModel.Pos, buildDataModel.Level);
                    }
                }
            }
        }

        private void BuildAreaDataUpdate(object arg0)
        {
            BuildAreadInfo buildAreadInfo = (BuildAreadInfo)arg0;

            if (buildAreadInfo.maskAreaId != -1)
            {
                List<BuildDataModel> list = BuildMgr.GetBuildDatasByArea(buildAreadInfo.id);

                for (int i = 0; i < list.Count; i++)
                {
                    UpdateBuildModelShow(list[i]);
                }
            }
        }

        private void PlayStallAni(int stallID, bool isEnter)
        {
            for (int i = 0; i < stallList.Count; i++)
            {
                if (stallList[i].id == stallID)
                {
                    if (isEnter)
                    {
                        stallList[i].MOnTriggerEnter(null);
                    }
                    else
                    {
                        stallList[i].MOnTriggerExit(null);
                    }

                    break;
                }
            }
        }

        private void UpdateUIInfo(object arg0)
        {
            BuildDataModel data = arg0 as BuildDataModel;

            UpdateBuildModelShow(data, true);
        }

        public void UpdateBuildModelShow(BuildDataModel unLockBuild, bool isPlayEffect = false)
        {
            int pos = unLockBuild.Pos - 1;

            if (unLockBuild.Type == 1)
            {
                //Stall_PropertyBase table = BuildMgr.GetStall_Property(unLockBuild.Id, unLockBuild.Level);
               
                //stallList[pos].BuildData(unLockBuild);

                //bool isUnlockArea = BuildAreaMgr.Instance.IsUnLockById(unLockBuild.AreaIndex);
                //stallList[pos].gameObject.SetActive(false);

                //if (unLockBuild.Level > 0)
                //{
                //    stallList[pos].ShowBuild();
                    
                //    stallList[pos].SetSkeletonDataAsset(table, unLockBuild.Level);

                //    stallList[pos].id = table.ID;
                //}

                //if (isPlayEffect)
                //{
                //    PlayBuildChangeEffect(stallList[pos], table.ID);
                //}

                SetBuildData(stallList[pos], unLockBuild, isPlayEffect);
            };

            if (unLockBuild.Type == 2)
            {
                //Equip_Property table = BuildMgr.GetEquip_Property(unLockBuild.Id, unLockBuild.Level);

                //equipList[pos].BuildData(unLockBuild);
                //bool isUnlockArea = BuildAreaMgr.Instance.IsUnLockById(unLockBuild.AreaIndex);
                //stallList[pos].gameObject.SetActive(false);

                //if (unLockBuild.Level > 0)
                //{
                //    equipList[pos].ShowBuild();
                //    equipList[pos].id = table.ID;
                //}

                //if (isPlayEffect)
                //{
                //    PlayBuildChangeEffect(equipList[pos], table.ID);
                //}

                SetBuildData(equipList[pos], unLockBuild, isPlayEffect);
            };

            if (unLockBuild.Type == 3)
            {
                //Adorn_Property table = BuildMgr.GetAdorn_Property(unLockBuild.Id, unLockBuild.Level);

                //adornList[pos].BuildData(unLockBuild);
                //bool isUnlockArea = BuildAreaMgr.Instance.IsUnLockById(unLockBuild.AreaIndex);
                //stallList[pos].gameObject.SetActive(false);

                //if (unLockBuild.Level > 0)
                //{
                //    adornList[pos].ShowBuild();

                //    adornList[pos].id = table.ID;
                //}

                //if (isPlayEffect)
                //{
                //    PlayBuildChangeEffect(adornList[pos], table.ID);
                //}

                SetBuildData(adornList[pos], unLockBuild, isPlayEffect);
            }
        }

        public void SetBuildData(BuildItem buildItem, BuildDataModel unLockBuild, bool isPlayEffect)
        {
            buildItem.BuildData(unLockBuild);
            bool isUnlockArea = unLockBuild.Type == 1|| BuildAreaMgr.Instance.GetIsUnLockAreaById(unLockBuild.AreaIndex);
            buildItem.gameObject.SetActive(isUnlockArea);

            if (!isUnlockArea)
            {
                return;
            }

            if (unLockBuild.Level > 0)
            {
                buildItem.ShowBuild();

                buildItem.id = unLockBuild.Id;
            }

            if (isPlayEffect)
            {
                PlayBuildChangeEffect(buildItem, unLockBuild.Id);
            }
        }

        private void PlayBuildChangeEffect(BuildItem buildItem, int id)
        {
            buildItem.PlayerEffect();
            MusicMgr.Instance.PlayMusicEff("d_building_fix");

            Vector3 pos = buildItem.GetShowBuildPos();

            if (buildItem.id == 22001)
            {
                pos = new Vector3(-21.21f, 11, 12.5f);
            }

            EazyGF.EventManager.Instance.TriggerEvent(EazyGF.EventKey.MoveCamerToTargetPos, pos);

            StartCoroutine(UICommonUtil.Instance.DelayedHandle((obj) => {

                buildItem.HidePlayEffect();
                MusicMgr.Instance.PlayMusicEff("d_building_fix_finish");

            }, 2f));
        }
    
    }
}
