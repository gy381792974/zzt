using System.Collections;
using System.Collections.Generic;
using EazyGF;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneLoad
{
    InitGame,
    StartGame,
    InitAndStartGame
}

public class SceneLoadingMgr : Singleton<SceneLoadingMgr>
{
    [SerializeField] private float curLoadedValue; //当前加载的进度
    [SerializeField] private SceneLoadingParm sceneLoadingParm;
    [SerializeField] private bool m_isGameInit = false;
    [SerializeField] private bool m_startLoad = false;
    [SerializeField] private int m_targetLoadIndex = -1;
    [SerializeField] private bool m_isHighScreen = false;

    private void Awake()
    {
        curLoadedValue = 0;
        m_isGameInit = false;
        LoadScene();
    }

    public void LoadScene()
    {
        ShowCanvans();

        if (SceneLoadingFlag.Instance.LoadedUIRoot)
        {
            SceneLoadingFlag.Instance.LoadedUIRoot = false;
            LoadUIRoot();
        }

        if (SceneLoadingFlag.Instance.LoadedGameMgr)
        {
            SceneLoadingFlag.Instance.LoadedGameMgr = false;
            LoadGameMgr();
        }
        
        m_targetLoadIndex = PlayerDataMgr.g_playerData.curChapterID;
        m_isGameInit = true;
        StartLoad(m_targetLoadIndex);
    }


    private GameObject iponeXCanvanObj;
    private GameObject normalCanvanObj;
    private float distance;
    private void ShowCanvans()
    {
        iponeXCanvanObj = transform.Find("LoadingCanvas_HighScreen").gameObject;
        normalCanvanObj = transform.Find("LoadingCanvas_Normal").gameObject;

        m_isHighScreen = Screen.height / (float) Screen.width > 2.1f;
        if (m_isHighScreen)//IponeX或者超长屏幕
        {
            sceneLoadingParm = iponeXCanvanObj.GetComponent<SceneLoadingParm>();
            iponeXCanvanObj.SetActive(true);
            normalCanvanObj.SetActive(false);
            distance = 925.082f;
        }
        else
        {
            sceneLoadingParm = normalCanvanObj.GetComponent<SceneLoadingParm>();
            iponeXCanvanObj.SetActive(false);
            normalCanvanObj.SetActive(true);
            distance = 633f;
        }

#if UNITY_ANDROID
        sceneLoadingParm.AndriodNameObj.SetActive(true);
        sceneLoadingParm.IosNameObj.SetActive(false);
#else
        sceneLoadingParm.AndriodNameObj.SetActive(false);
        sceneLoadingParm.IosNameObj.SetActive(true);
#endif

        sceneLoadingParm.LoadFillImage.fillAmount = 0;
        if (sceneLoadingParm.autoMachCanvasScaler != null)
        {
            sceneLoadingParm.autoMachCanvasScaler.Init();
        }
    }


    private void LoadGameMgr()
    {
        GameObject managerObj = Instantiate(Resources.Load<GameObject>("Manager"));
       
        DontDestroyOnLoad(managerObj);
        
        managerObj.GetComponent<GameMgr>().Init();
    }

    private void LoadUIRoot()
    {
        GameObject uiRootObj = Instantiate(Resources.Load<GameObject>("UIRoot"));
        uiRootObj.GetComponent<AutoMachCanvasScaler>().Init();
        DontDestroyOnLoad(uiRootObj);
    }

    private void StartLoad(int targetIndex)
    {
        curLoadedValue = 0;
        m_targetLoadIndex = targetIndex;
        m_startLoad = true;
    }

    private float rate = 0;

    void Update()
    {
        if (!m_startLoad)
        {
            return;
        }
//#if UNITY_EDITOR && MY_DEBUG
//        curLoadedValue = 100;
//        SceneManager.LoadScene(m_targetLoadIndex);
//        m_startLoad = false;
//        return;
//#endif


        if (curLoadedValue < 30)
        {
            rate = 0.5f;
        }
        else if (curLoadedValue >= 30 && curLoadedValue < 90)
        {
            rate = 1f;
        }
        else
        {
            rate = 2f;
        }
        curLoadedValue = Mathf.Lerp(curLoadedValue, 100, Time.fixedDeltaTime * rate);

        if (curLoadedValue >= 99.99f && m_isGameInit)
        {
            curLoadedValue = 100;
            SceneManager.LoadScene(m_targetLoadIndex);
            m_startLoad = false;
        }
        sceneLoadingParm.LoadFillImage.fillAmount = curLoadedValue / 100f; //把百分比赋值给图片
        SetParticPos(curLoadedValue / 100f);
    }

    private Vector3 particPos=new Vector3();
    private void SetParticPos(float rate)//17.918
    {
       // float distance = 633f;
        particPos.x = rate * distance;
        sceneLoadingParm.ParticTrans.transform.localPosition = particPos;
    }
    
}
