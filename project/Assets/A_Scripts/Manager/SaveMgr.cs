using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EazyGF
{
    /// <summary>
    /// 处理玩家前后台切换以及数据保存和离线时间相关事宜
    /// </summary>
    public class SaveMgr : Singleton<SaveMgr>
    {
        //当前场景是否为加载场景，加载场景不能显示出离线收益界面
        private bool m_isLoadingScene = true;
        public void Init()
        {
            //SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        }
        private void Awake()
        {
            ChangeSceneEvent();
        }
        public void ChangeSceneEvent()
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        public void RemoveSceneEven()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            m_isLoadingScene = scene.buildIndex == 0;
            if (m_isLoadingScene)
            {
#if UNITY_EDITOR && MY_DEBUG 
                ShowOfflinePanel();
#else
 
#endif
            }
        }

        private void Start()
        {
            InvokeRepeating("AutoSaveGameData", 60, 60);
        }
        private void AutoSaveGameData()
        {
            SaveGameData();
        }


        //获取和失去焦点，进入游戏系统会自动调用一次
        private void OnApplicationFocus(bool focus)
        {
            if (focus) //获取焦点
            {
                ShowOfflinePanel();
            }
            else //失去焦点
            {
                PlayerDataMgr.SaveData(true);
            }
        }

        /// <summary>
        /// 处理关于离线时间的相关事宜
        /// </summary>
        public void OnHandleOffLineTime()
        {
            int offLineTime = GetPlayerOfflineSceonds();

            if (offLineTime > 180) //todo
            {

            }

            ShowOfflinePanel();
        }
        /// <summary>
        /// 处理离奖励面板
        /// </summary>
        public void ShowOfflinePanel()
        {
            int offline = GetPlayerOfflineSceonds();
            //大于10分钟才显示离线面板和离线收益
            if (offline >= 600)
            {
                //离线时间大于5小时算作5小时收益
                if (offline > 18000) offline = 18000;
                UIMgr.ShowPanel<OffLinePanel>(new OffLinePanelData(offline));
            }
        }
        /// <summary>
        /// 获取玩家从离线到上线的时间，单位 秒
        /// </summary>
        /// <returns></returns>
        private int GetPlayerOfflineSceonds()
        {
            DateTime focusDateTime = DateTime.Now;
            //玩家如果往前调时间-则认为离线时间为0
            if (focusDateTime < PlayerDataMgr.g_playerData.playerOfflineTime)
            {
                PlayerDataMgr.g_playerData.playerOfflineTime = focusDateTime;
            }
            int offlineTimer = TimeHelp.DiffSecondByTwoDateTime(focusDateTime, PlayerDataMgr.g_playerData.playerOfflineTime);
            //PlayerDataMgr.g_playerData.playerOfflineTime = focusDateTime;
            Debug.Log("离线时间:" + offlineTimer);
            return offlineTimer;
        }

        private void SaveGameData()
        {
            PlayerDataMgr.SaveData();
        }
    }
}

