using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EazyGF
{

    public class GameMgr : Singleton<GameMgr>
    {
        public AssetLoadMode LoadMode;

        long currentDateTime;
        public long CurrentDateTime { get => currentDateTime; set => currentDateTime = value; }

        private void Awake()
        {
            // SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Init();
            StartGame();
        }

        //private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        //{
        //    if (scene.buildIndex != 0)
        //    {
        //        StartGame();
        //    }
        //}

        public void Init()
        {
            DontDestroyOnLoad(gameObject);
            Vector3 vector3 = new Vector3();
            //环境
            InitEnvironment();

            //初始化第三方SDK-有部分SDK要求必须同意某些协议后才能继续游戏，因此，后面的两个函数可以放在同意第三方协议后再调用
            ThirdPartySdkMgr.Instance.InitSDK();

            //Mgr组件
            InitGame();

            InitData();
        }


        //初始化游戏环境
        private void InitEnvironment()
        {
            //设置游戏目标帧率
            Application.targetFrameRate
#if UNITY_EDITOR
                = 0;
#else
                = 45;//根据需求动态调节 45上下
#endif


#if MY_DEBUG || UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
            LoadDebugPanel();
#else
            Debug.unityLogger.logEnabled = false;
#endif


#if !UNITY_EDITOR
            LoadMode = AssetLoadMode.AssetBundleMode; 
#endif
        }

        private void LoadDebugPanel()
        {
            GameObject debugPanelPrefab = Resources.Load<GameObject>("DeBugCanvas");
            if (debugPanelPrefab != null)
            {
                Transform debugPanelIns = Instantiate(debugPanelPrefab).transform;
                //debugPanelIns.DontDestroyOnLoad();
                debugPanelIns.transform.SetAsLastSibling();
                debugPanelIns.GetComponent<DebugTools>().Init();
                DontDestroyOnLoad(debugPanelIns.gameObject);
            }
        }

        private void InitGame()
        {
            //资源加载器初始化
            AssetMgr.Instance.Init(LoadMode);
            //加载josn数据
            JsonDataLoaderMgr.Instance.LoadGameData();
            //加载和初始化玩家数据
            new PlayerDataMgr().InitPlayerData();
            //初始化语言环境
            LanguageMgr.InitLanguage();

            //初始化UIMgr
            UIMgr.Init();
            AutoMachCanvasScaler.Instance.Init();
            //初始化播放器
            MusicMgr.Instance.Init();
            //初始化数据保存
            SaveMgr.Instance.Init();

        }

        private void InitData()
        {
            //CustomerLogic.InitData();
            BuildMgr.InitData();
        }

        #region 主要API

        //主要API：
        //1.资源加载
        //同步加载

        //AssetMgr.Instance.LoadGameobjFromPool("");
        // AssetMgr.Instance.LoadTexture(abName,assetName)
        //异步加载
        //
        //AssetMgr.Instance.LoadTextureAsync("guide_npc_02", "guide_npc_02", delegate (Sprite loadSprite)
        //{
        //    //loadSprite是返回加载后的图片
        //});

        //2.UI框架

        // UIMgr.ShowPanel<Panel1>(null, typeof(Panel2)); //第二个参数的意思是当前显示出来的面板只有Panel2时，才会显示Panel1

        //List<int> tem = PlayerDataMgr.GetCashCheckDataList(1, 1, CashCheckTarget.Promote);
        //Debug.Log(tem[0]);

        #endregion

        //开始游戏
        public void StartGame()
        {
            // PoolMgr.Instance.

            //UIMgr.ShowPanel<MainPanel>();
            //Debug.Log(LanguageMgr.GetTranstion(1, 1, 10, 999));

            //创建预制体

            // CoroutineManager.Instance.DoCoroutine(MainSceneMGR.Instance.CreateAiObj());

            //用这个回收
            //PoolMgr.Instance.DespawnOne();
            //Test
            //AssetMgr.Instance.LoadGameobj("Cube_Test");
            //Transform ts1 = AssetMgr.Instance.LoadGameobjFromPool("Cube_Test");//从对象池里获取一个Cube预制体
            //PoolMgr.Instance.DespawnOne(ts1);//对象池回收Cube预制体

            //AssetMgr.Instance.LoadTexture("BuildTex", "Test1");

            //AssetMgr.Instance.LoadGameobjFromPool("TTModel");
            //LanguageMgr.GetTranstion(1, 1,2,5);
            // LanguageMgr.GetTranstion(new int[] { 1, 1 },2,5);
            //AssetMgr.Instance.LoadTexture("TestTexture", "Test1");
            //LanguageMgr.GetTranstion(data.BuildName);
            UIMgr.ShowPanel<MainPanel>();
            UIMgr.ShowPanel<BubblePanel>();
        }

        private void Update()
        {
            currentDateTime = Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
        }
    }
}

