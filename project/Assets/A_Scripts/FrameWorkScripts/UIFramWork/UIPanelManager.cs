using System;
using System.Collections;
using System.Collections.Generic;
using EazyGF;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public enum UILevel
{
    WorldToScreen,//3D物体转为屏幕坐标，2D UI跟随3D物体
    Bottom,//底部
    View,//面板
    Dialog,//弹窗
    Front,//
    Guide,//新手引导
    Tips,//提示
}

/// <summary>
/// 外部调用
/// </summary>
public class UIMgr
{
    public static void Init()
    {
        UIPanelManager.Instance.Init();
    }

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="uiDataBase">数据</param>
    /// <param name="conditonPanelType">条件</param>
    public static void ShowPanel<T>(UIDataBase uiDataBase = null,float delayTime=0, params Type[] conditonPanelType) where T : UIBase
    {
        if (UIPanelManager.Instance.IsCanShowUI(typeof(T), uiDataBase, conditonPanelType))
        {
            UIPanelManager.Instance.ShowUI<T>(uiDataBase,delayTime);
        }
    }

    /// <summary>
    /// 展示一个UI并返回其引用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiDataBase"></param>
    /// <param name="delayTime"></param>
    /// <param name="conditonPanelType"></param>
    /// <returns></returns>
    public static T OpenUIAndReturnThis<T>(UIDataBase uiDataBase = null, float delayTime = 0, params Type[] conditonPanelType) where T : UIBase
    {

        UIMgr.ShowPanel<T>(uiDataBase);
        return UIMgr.GetUI<T>();

    }

    public static void HideUI<T>(Action action = null)
    {
        UIPanelManager.Instance.HideUI<T>(action);
    }

    public static void HideUI(string name, Action action = null)
    {
        UIPanelManager.Instance.HideUI(name, action);
    }

    public static T GetUI<T>() where T : UIBase
    {
       return UIPanelManager.Instance.GetUIByAllUIDic<T>();
    }

    public static void HideAllUI(bool hideAll)
    {
        UIPanelManager.Instance.HideAllUI(hideAll);
    }

    //public static void ClearWaitingList()
    //{
    //    UIPanelManager.Instance.ClearWaitList();
    //}
}

/// <summary>
/// 内部用
/// </summary>
public class UIPanelManager : Singleton<UIPanelManager>
{
    [Header("是否预加载UI面板")]
    public bool PreloadUIPanel;
    [SerializeField] public string[] PreLoadPanelName=new string[10];

#pragma warning disable 649
    [SerializeField] private Transform WorldToScreenTrans;//暂未用上
    [SerializeField] private Transform BottomTrans;//最底层
    [SerializeField] private Transform ViewTrans;//面板
    [SerializeField] private Transform DialogTrans;//弹窗
    [SerializeField] private Transform FrontTrans;//最前面
    [SerializeField] private Transform GuideTrans;//新手引导 
    [SerializeField] private Transform TipsTrans;//提示弹出窗口

    [SerializeField] private GameObject DesignObj;//设计
#pragma warning restore 649
    private readonly Dictionary<string, UIBase> dicAllUI = new Dictionary<string, UIBase>(); //所有打开过的UI,包括显示和不显示
    private readonly Dictionary<string, UIBase> dicShowingUI = new Dictionary<string, UIBase>(); //正在展示的UI

    public void Init()
    {
        //隐藏设计面板
        HideDesignObj();

        //预加载面板
        InitPreLoadPanel();

        //场景加载事件
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //每当场景加载后，清除UI等待列表
        ClearWaitList();
    }

    //隐藏设计面板
    private void HideDesignObj()
    {
        if (DesignObj != null)
        {
            DesignObj.SetActive(false);
        }
    }

    //预加载面板
    private void InitPreLoadPanel()
    {
        if (PreloadUIPanel)
        {
            for (int i = 0; i < PreLoadPanelName.Length; i++)
            {
                if (!string.IsNullOrEmpty(PreLoadPanelName[i]))
                {
                    StartCoroutine(PreLoadUI(PreLoadPanelName[i]));
                }
            }
        }
    }

    
    /// <summary>
    /// 从容器中获取需要显示的UI
    /// </summary>
    /// <param name="panelName"></param>
    /// <returns></returns>
    private UIBase GetUIByAllUIDic(string panelName)
    {
        if (dicAllUI.TryGetValue(panelName, out var uiBase))
        {
            return uiBase;
        }

        return null;
    }

    public T GetUIByAllUIDic<T>() where T : UIBase
    {
        if (dicAllUI.TryGetValue(typeof(T).Name.ToLower(), out var uiBase))
        {
            return  uiBase as T;
        }
        //Debug.Log($"找不到面板:{typeof(T).Name}");
        return null;
    }

    /// <summary>
    /// 显示UI的方法 需要手动调用
    /// </summary>
    /// <param name="uiDataBase"></param>
    public void ShowUI<T>(UIDataBase uiDataBase = null,float delayTime=0) where T : UIBase//1.处理当前UI，2，处理即将显示的UI，3，回调了虚方法
    {
        string panelAb_Name = typeof(T).Name;
        ShowUI(panelAb_Name,uiDataBase);
    }

    private List<string> initPanelList=new List<string>(10);

    private void ShowUI(string panelAb_Name, UIDataBase uiDataBase = null)
    {
        string panelName = panelAb_Name;//实例化出来的面板名字
        panelAb_Name = panelAb_Name.ToLower();//面板AB包名字
        //1：加载当前UI
        if (dicShowingUI.TryGetValue(panelAb_Name, out var tempValue))//当前UI已经在显示列表中了，就直接返回
        {
            Debug.Log($"{panelAb_Name}:该面板已经显示出来了!");
            return;
        }

        UIBase ui = GetUIByAllUIDic(panelAb_Name);//通过ID获取需要显示的UI，从dicAllUI容器中获取
        if (ui == null)//如果在容器中没有此UI(说明从来就没有打开过该面板)，就从资源中读取ui预制体
        {
            if (!LoadUiPrefab(panelAb_Name, panelName, out ui))
            {
                Debug.LogError($"加载{panelName}失败！");
                return;
            }
        }
        //2:更新显示其它的UI
        UpdateOtherUIState(ui);//更新其他UI状态
        //3:显示当前UI
        dicAllUI[panelAb_Name] = ui;//所有显示和不显示的UI都添加进来
        dicShowingUI[panelAb_Name] = ui;//所有显示的UI都添加进来
        if (!initPanelList.Contains(panelAb_Name))
        {
            ui.Init();
            initPanelList.Add(panelAb_Name);
        }
        ui.Show(uiDataBase);
    }

    private IEnumerator PreLoadUI(string panelAb_Name)
    {
        string panelName = panelAb_Name;//实例化出来的面板名字
        panelAb_Name = panelAb_Name.ToLower();//面板AB包名字
        //1：加载当前UI
        if (dicAllUI.TryGetValue(panelAb_Name, out var tempValue))//当前UI已经在显示列表中了，就直接返回
        {
            Debug.Log($"{panelAb_Name}:该面板已经预加载过!");
            yield break;
        }
        
        if (!LoadUiPrefab(panelAb_Name, panelName, out var ui))
        {
            Debug.LogError($"加载{panelName}失败！");
            yield break;
        }

        dicAllUI[panelAb_Name] = ui;//所有显示和不显示的UI都添加进来
        ui.PreLoad();
        Debug.Log($"预加载面板:{panelName}");
        yield return null;
    }

    private bool LoadUiPrefab(string panelAb_Name,string panelName,out UIBase ui)
    {
        //加载UI面板Prefab
        GameObject prefab = AssetMgr.Instance.LoadAsset<GameObject>(panelAb_Name, panelAb_Name);
        if (prefab != null)//资源加载成功
        {
            Transform goWillShowUI = Instantiate(prefab).transform;//克隆游戏对象到层次面板上
            ui = goWillShowUI.GetComponent<UIBase>();//获取此对象上的UI
            if (ui == null)
            {
                Debug.LogError($"请在{panelAb_Name}上挂载脚本！");
                return false;
            }
            goWillShowUI.SetParent(GetPanelParen(ui.uiLevel));
            RectTransform uiGoRectTrans = goWillShowUI as RectTransform;
            if (uiGoRectTrans == null)
            {
                Debug.LogError($"加载{panelAb_Name}失败！");
                return false;
            }
            uiGoRectTrans.offsetMin = Vector2.zero;
            uiGoRectTrans.offsetMax = Vector2.zero;
            uiGoRectTrans.anchoredPosition3D = Vector3.zero;
            uiGoRectTrans.anchorMin = Vector2.zero;
            uiGoRectTrans.anchorMax = Vector2.one;
            uiGoRectTrans.localScale = Vector3.one;
            uiGoRectTrans.rotation = Quaternion.identity;
            uiGoRectTrans.gameObject.name = panelName;
            prefab = null; 
            return true;
        }
        else
        {
            Debug.LogError($"加载:{panelAb_Name}失败!");
            ui = null;
            return false;
        }
    }

    private void UpdateOtherUIState(UIBase ui)//更新其它UI的状态（显示或者隐藏）
    {
        if (ui.showMode == UIShowMode.HideAll)
        {
            HideAllUI(true);//隐藏所有的UI
        }
        else if (ui.showMode == UIShowMode.HideAllExceptTop)//剔除最前面UI
        {
            HideAllUI(false);//隐藏所有的UI
        }
    }

    public void HideUI<T>( Action action = null) //隐藏UI，传入ID和需要做的事情
    {
        string panelName = typeof(T).Name.ToLower();
        //正在显示的容器中没有此ID
        if (!dicShowingUI.TryGetValue(panelName, out var uiBase))
        {
            return;
        }
        if (action == null)//隐藏UI的时候不需要做别的事情
        {
            dicShowingUI[panelName].Hide();//直接隐藏
            dicShowingUI.Remove(panelName);//从显示列表中删除 
        }
        else//隐藏窗体之后需要做的事情
        {
            action += delegate { dicShowingUI.Remove(panelName); };
            dicShowingUI[panelName].Hide(action);
            
        }
        //检查等待显示的UI列表中是否有满足显示条件的UI面板
        CheckIsNeedShowWaitedUI();
    }

    public void HideUI(string name, Action action = null) //隐藏UI，传入ID和需要做的事情
    {
        string panelName = name.ToLower();
        //正在显示的容器中没有此ID
        if (!dicShowingUI.TryGetValue(panelName, out var uiBase))
        {
            return;
        }
        if (action == null)//隐藏UI的时候不需要做别的事情
        {
            dicShowingUI[panelName].Hide();//直接隐藏
            dicShowingUI.Remove(panelName);//从显示列表中删除 
        }
        else//隐藏窗体之后需要做的事情
        {
            action += delegate { dicShowingUI.Remove(panelName); };
            dicShowingUI[panelName].Hide(action);

        }
        //检查等待显示的UI列表中是否有满足显示条件的UI面板
        CheckIsNeedShowWaitedUI();
    }

    public void HideAllUI(bool isHideAbove) //隐藏所有的UI，参数表示是否隐藏Bottom层级的UI
    {
        if (isHideAbove)//隐藏最上面的UI
        {
            foreach (var item in dicShowingUI)//遍历所有正在显示的UI
            {
                item.Value.Hide();
            }
            dicShowingUI.Clear();
        }
        else
        {//不隐藏最上面的主UI，其他的全部隐藏
            List<string> listRemove = new List<string>();
            foreach (var item in dicShowingUI)
            {
                if (item.Value.uiLevel == UILevel.Bottom)
                {
                    continue;
                }

                item.Value.Hide();
                listRemove.Add(item.Key);
            }
            for (int i = 0; i < listRemove.Count; i++)
            {
                dicShowingUI.Remove(listRemove[i]);
                //循环结束后，只剩下了主UI
            }
            listRemove.Clear();
        }
    }

    public void RemoveDic(string panelName)
    {
        if (dicShowingUI.ContainsKey(panelName))
        {
            dicShowingUI.Remove(panelName);
        }

        if (dicAllUI.ContainsKey(panelName))
        {
            dicAllUI.Remove(panelName);
        }
    }
    private Transform GetPanelParen(UILevel uiLevel)
    {
        switch (uiLevel)
        {
            case UILevel.WorldToScreen:
                return WorldToScreenTrans;
            case UILevel.Bottom:
                return BottomTrans;
            case UILevel.View:
                return ViewTrans;
            case UILevel.Dialog:
                return DialogTrans;
            case UILevel.Front:
                return FrontTrans;
            case UILevel.Guide:
                return GuideTrans;
            case UILevel.Tips:
                return TipsTrans;
        }
        return null;
    }
    

    //当前等待显示的UI列表
    private static readonly List<WaitParam> WaitingShowList = new List<WaitParam>();
    public class WaitParam
    {
        public UIDataBase uiDataBase;//面板数据
        public Type[] conditionPanelArray;//显示面板条件
        public Type panelType;//需要显示的面板类型
    }
    public WaitParam GetWaitParam(Type panelType,UIDataBase _uiDataBase, Type[] _conditionPanelArray) 
    {
        WaitParam waitParam = new WaitParam();

        waitParam.uiDataBase = _uiDataBase;
        waitParam.conditionPanelArray = _conditionPanelArray;
        waitParam.panelType = panelType;
        return waitParam;
    }


    /// <summary>
    /// 是否可以显示UI
    /// </summary>
    /// <param name="panelType"></param>
    /// <param name="uiDataBase"></param>
    /// <param name="conditonPanelType"></param>
    /// <returns></returns>
    public bool IsCanShowUI(Type panelType,UIDataBase uiDataBase,Type[] conditonPanelType)
    {
        int showDicCount = dicShowingUI.Count;
        if (conditonPanelType == null || conditonPanelType.Length <= 0)
        {
            return true;
        }
        if (showDicCount <= 0)
        {
            return false;
        }
        int conditonLenth = conditonPanelType.Length;
        int count = 0;
        foreach (var uiBase in dicShowingUI)
        {
            for (int i = 0; i < conditonLenth; i++)
            {
                if (conditonPanelType[i].Name.Equals(uiBase.Value.name))
                {
                    count++;
                    break;
                }
            }
        }
        bool canShow = count == showDicCount;
        if (!canShow)
        {
            WaitParam waitParam = WaitingShowList.Find(x => x.panelType == panelType);
            if (waitParam != null)//有该面板，需要对显示参数重新赋值
            {
                waitParam.uiDataBase = uiDataBase;
            }
            else//如果没有
            {
                waitParam = GetWaitParam(panelType, uiDataBase, conditonPanelType);
                WaitingShowList.Add(waitParam);
            }
        }
        return canShow;
    }
    /// <summary>
    /// 每次关闭界面时检查等待显示的UI中是否有满足条件的
    /// </summary>
    public void CheckIsNeedShowWaitedUI()
    {
        for (int i = 0; i < WaitingShowList.Count; i++)
        {
            WaitParam waitParam = WaitingShowList[i];
            if (IsCanShowUI(waitParam.panelType, waitParam.uiDataBase, waitParam.conditionPanelArray))
            {
                ShowUI(waitParam.panelType.Name, waitParam.uiDataBase);
                WaitingShowList.Remove(waitParam);
            }
        }
    }

    public void ClearWaitList()
    {
        WaitingShowList.Clear();
    }

    public int GetDicShowingUI()
    {
        return dicShowingUI.Count;
    }

}

