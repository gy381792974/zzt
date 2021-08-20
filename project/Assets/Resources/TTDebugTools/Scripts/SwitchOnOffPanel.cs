using EazyGF;
using HunliGame;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SwitchOnoffKey
{
    TEST,
    TestValue1,
    TestValue2,
    AddItemNum,
    AddCoinStar,
    timeScale,
    ChangeLanguage,
    KeepNum,
    TargetCus,
    DebugPanel,
    UnlockAllBuild,

    DISPLAYLOG,
    DeleteAllPlayerPrefs,
    NUM,

}

enum SwitchFlag
{
    SWITCH_OFF = 0,
    SWITCH_ON = 1,
}


class SwitchOnOffElements
{
    static SwitchOnOffElements instance_;
    public static SwitchOnOffElements Instance
    {
        get
        {
            if (instance_ == null)
                instance_ = new SwitchOnOffElements();
            return instance_;
        }
    }

    int[] onoff_dict_ = new int[(int)SwitchOnoffKey.NUM];

    SwitchOnOffElements()
    {
        Register(SwitchOnoffKey.TEST, () => 1);
        Register(SwitchOnoffKey.TestValue1, () => (int)SwitchOnOffPanel.testValue1);
        Register(SwitchOnoffKey.TestValue2, () => (int)SwitchOnOffPanel.testValue2);
        Register(SwitchOnoffKey.DISPLAYLOG, () => 5);
        Register(SwitchOnoffKey.AddCoinStar, () => 100000);
        Register(SwitchOnoffKey.AddItemNum, () => 1000);
        Register(SwitchOnoffKey.KeepNum, () => 50);
        Register(SwitchOnoffKey.timeScale, () => 10);
        Register(SwitchOnoffKey.UnlockAllBuild, () => 1);
        Register(SwitchOnoffKey.TargetCus, () => 1);
    }

    public void Register(SwitchOnoffKey key, Func<int> func)
    {
        try
        {
            int val = func();
            onoff_dict_[(int)key] = val;
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("Register<{0}> Error", key));
            Debug.LogException(e);
        }
    }

    public void TestCode(int i)
    {
        switch (i)
        {
            case 1:

                //UIMgr.ShowPanel<StaffPanel>();

                //CBBData cBBData = new CBBData();

                //cBBData.type = 2;
                //cBBData.tf = MainSpace.Instance.stallList[0].GetShowBuildBoxTf();

                //cBBData.id = 10;
                //cBBData.num = 100;

                //EventManager.Instance.TriggerEvent(EventKey.SendCBBData, cBBData);

                //BuildCollcetSerData serData = SerializHelp.DeserializeFileToObj<BuildCollcetSerData>(UICommonUtil.GetSerSavePath("buildCollect.data"), out bool loadSuccess);

                //if (loadSuccess)
                //{
                //    Debug.LogError(serData.SBCollctCoinDic.Count);
                //    return;
                //}

                //BuildMgr.UpgradeComBuild(22001, (int)SwitchOnOffPanel.testValue1);

                //CusFashionMgr.Instance.AddGTStallNum(1, 1);

                //CusFashionMgr.Instance.AddGTStallNum(1, 1);

                //CusFashionMgr.Instance.AddGTStallNum(1, 1);

                //Debug.LogError(CusFashionMgr.Instance.GetGTStallNum(1, 1));


                //int[] ids = new int[2];
                //List<int> areaIds = new List<int>();
                //ids[0] = -1;
                //ids[1] = -1;


                //if (areaIds.Count == 0 || areaIds.Count == 1 || index == -1)
                //{
                //    ids[0] = -1;
                //    ids[1] = -1;
                //}
                //else
                //{
                //    if (index - 1 >= 0)
                //    {
                //        ids[0] = index - 1;
                //    }

                //    if (index + 1 < areaIds.Count && index + 1 >= 1)
                //    {
                //        ids[1] = index - 1;
                //    }
                //}

                //Debug.LogWarning($"left{ids[0]} right{ids[1]}");

                //BuildAreaMgr.Instance.GetBoundAreas(1);
                //BuildAreaMgr.Instance.GetBoundAreas(2);
                //BuildAreaMgr.Instance.GetBoundAreas(3);

                //int pos = (int)SwitchOnOffPanel.testValue2;
                //int level = 1;
                //Vector3 vp = MainSpace.Instance.equipList[0].GetShowBuildPos(pos, level);
                //ColorGradientUtil.Instance.PlayerCGradientEff(MainSpace.Instance.equipList[0].GetShowBuildMR(pos, level));

                //EventManager.Instance.TriggerEvent(EventKey.MoveCamerToTargetPos2, vp);


                //ColorGradientUtil.Instance.PlayerCGradientEff(MainSpace.Instance.adornList[10].GetShowBuildMR(0,1));

                 //LanguageMgr.LoadLanguage(SystemLanguage.ChineseSimplified);

                break;

            case 2:

                //Debug.LogError(LanguageMgr.GetTranstion(new int[] { 1, 1 }, 2, 5));

                break;

            case 3:
                break;

            case 4:
                break;

            default:
                break;
        }

        SwitchOnOffPanel.Instance.OnClosePanle();
    }

    public int GetValue(SwitchOnoffKey key)
    {
        return onoff_dict_[(int)key];
    }

    public void SetValue(SwitchOnoffKey key, int value, Action<object> action = null, object param = null)
    {
        onoff_dict_[(int)key] = value;

        Debug.LogWarning(key + "  SetValue  " + value);

        switch (key)
        {
            case SwitchOnoffKey.TEST:

                //EazyGF.EventManager.Instance.TriggerEvent(EazyGF.EventKey.MoveCamerToTargetPos, GuideMgr.Instance.targetTf.position);
                //GuideMgr.Instance.SetHighlight(GuideMgr.Instance.targetTf.position, 1);

                //SwitchOnOffPanel.Instance.OnClosePanle();

                //EventManager.Instance.TriggerEvent(EventKey.TestEvent, value);

                //SwitchOnOffPanel.Instance.OnClosePanle();

                //GuideMgr.Instance.isGuide = true;

                //GuideMgr.Instance.isFirstEnterGame = true;

                //GuideMgr.Instance.TargetGuide(0);

                //SwitchOnOffPanel.Instance.OnClosePanle();

                TestCode(value);

                break;

            case SwitchOnoffKey.DISPLAYLOG:


                //TipCommonTool.Intance.ShowTip(LanguageMgr.GetTranstion(new int[2] { 2001, 1 }));

                EazyGF.EventManager.Instance.TriggerEvent(EazyGF.EventKey.MoveCamerToTargetPos, GuideMgr.Instance.targetTf.position);
                GuideMgr.Instance.SetHighlight(GuideMgr.Instance.targetTf, 1);

                SwitchOnOffPanel.Instance.OnClosePanle();

                break;
            case SwitchOnoffKey.TestValue1:

                SwitchOnOffPanel.testValue1 = value;

                break;
            case SwitchOnoffKey.TargetCus:               

                CustomerMgr.targetIndex = value;

                break;
            case SwitchOnoffKey.TestValue2:

                SwitchOnOffPanel.testValue2 = value;

                break;

            case SwitchOnoffKey.AddItemNum:

                ItemPropsManager.Intance.AddAllItem(value);

                break;



            case SwitchOnoffKey.DeleteAllPlayerPrefs:



                CubeGameMgr.Instance.GetLinkCubeIndexs(value, -10, -10);
                //CubeGameMgr.Instance.GetLinkCube(0, 0, -100);
                //CubeGameMgr.Instance.GetLinkCube(0, 135, -100);
                //CubeGameMgr.Instance.GetLinkCube(0, 100, 0);
                //CubeGameMgr.Instance.GetLinkCube(5, 100, 0);

                //CubeGameMgr.Instance.GetLinkCube(5, 0, 100);
                //CubeGameMgr.Instance.GetLinkCube(5, 0, -100);
                //CubeGameMgr.Instance.GetLinkCube(5, 0, -100);
                //CubeGameMgr.Instance.GetLinkCube(5, 135, -100);



                //CubeGameMgr.Instance.GetLinkCube(18, 0, 100);
                //CubeGameMgr.Instance.GetLinkCube(18, 0, -100);
                //CubeGameMgr.Instance.GetLinkCube(18, 135, -100);

                break;

            case SwitchOnoffKey.AddCoinStar:

                
                ItemPropsManager.Intance.AddItem(1, value);
                ItemPropsManager.Intance.AddItem(2, value);

                EventManager.Instance.TriggerEvent(EventKey.UdpatePlayData, value);

                break;

            case SwitchOnoffKey.timeScale:
                Time.timeScale = value;

                break;
            case SwitchOnoffKey.DebugPanel:

                DebugTools.Instance.ShowOrHidePanel();

                break;
            case SwitchOnoffKey.ChangeLanguage:

                SystemLanguage lIndex = (SystemLanguage)GameComSetting.Instance.languageIndex;

                // if (lIndex == SystemLanguage.English)
                //{
                //    GameComSetting.Instance.SetLanguageMgr(SystemLanguage.ChineseSimplified);
                //}
                //else if (lIndex == SystemLanguage.ChineseSimplified)
                //{
                //    GameComSetting.Instance.SetLanguageMgr(SystemLanguage.English);
                //}
                //else
                //{
                //    GameComSetting.Instance.SetLanguageMgr(SystemLanguage.ChineseSimplified);
                //}

                if (value == 1)
                {
                    GameComSetting.Instance.SetLanguageMgr(SystemLanguage.ChineseSimplified);
                }
                else if (value == 0)
                {
                    GameComSetting.Instance.SetLanguageMgr(SystemLanguage.English);
                }


                break;

            case SwitchOnoffKey.KeepNum:
                
                CustomerMgr.faseCreateNum = value;

                break;
         
            case SwitchOnoffKey.UnlockAllBuild:

                int level = value;

                if (level >= 1 && level <= 4)
                {
                    BuildMgr.UnLockAllBuild(level);

                    MainSpace mainSpace = GameObject.Find("Battle/BuildSpace").GetComponent<MainSpace>();
                    mainSpace.UnLockALLBuildFun();
                }

                break;
        }

        //SwitchOnOffPanel.Instance.gameObject.SetActive(false);

        //GameObject.

       

    }
}

public class SwitchOnOffPanel : MonoBehaviour
{

    public ScrollRect ItemRoot;
    public SwitchOnOffItem ItemPrefab;
    public InputField gmFileInput;

    public static SwitchOnOffPanel Instance;

    public static float testValue1 = 1;
    public static float testValue2 = 1;

    private void Awake()
    {
        Instance = this;
    }


    // Use this for initialization
    void Start()
    {
        UpdateInfo();

    }

    public void UpdateInfo()
    {
        int height = 0;

        for (int i = 0; i < (int)SwitchOnoffKey.NUM; ++i)
        {
            try
            {
                string key_str = getSwitchOnoffName((SwitchOnoffKey)i);
                int value = SwitchOnOffElements.Instance.GetValue((SwitchOnoffKey)i);
                GameObject go = Instantiate(ItemPrefab.gameObject) as GameObject;
                go.transform.SetParent(ItemRoot.content);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.Euler(Vector3.zero);
                height += 100;
                go.SetActive(true);
                SwitchOnOffItem item = go.GetComponent<SwitchOnOffItem>();
                item.Refresh(key_str, value, (SwitchOnoffKey)i);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("UpdateInfo<{0}> Error", (SwitchOnoffKey)i));
                Debug.LogException(e);
            }
        }
        height += ((int)SwitchOnoffKey.NUM - 1) * 10;
        ItemRoot.content.sizeDelta = new Vector2(ItemRoot.content.sizeDelta.x, height);

    }

    public string getSwitchOnoffName(SwitchOnoffKey key)
    {
        return key.ToString();
    }

    public int getSwitchOnoffValue(SwitchOnoffKey key)
    {
        return SwitchOnOffElements.Instance.GetValue(key);
    }

    int index = 1;

    public void setSwitchOnoffValue(SwitchOnoffKey key, int value, Action<object> action = null, object param = null)
    {
        SwitchOnOffElements.Instance.SetValue(key, value, action, param);
    }

    public static void ShowPanel()
    {
        if (Instance == null)
        {
            UnityEngine.Object obj = Resources.Load("TTDebugTools/swicth_onoff_panel");
            if (obj != null)
            {
                GameObject ob = Instantiate(obj) as GameObject;
                ob.transform.SetParent(GameObject.Find("Debug").transform);
                ob.transform.localScale = Vector3.one;
                ob.transform.localPosition = Vector3.zero;
                ob.transform.localRotation = Quaternion.Euler(Vector3.zero);
                ((RectTransform)ob.transform).sizeDelta = Vector2.zero;
            }
        }
        else
        {
            Instance.gameObject.SetActive(true);
        }
    }



    public void OnClosePanle()
    {
        //Instance = null;

        //GameObject.Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void OnGMFileLoad(InputField fileInput)
    {
        if (!string.IsNullOrEmpty(fileInput.text))
        {
            string fileName = fileInput.text;
            string filePath = Application.persistentDataPath + "/GM/" + fileName;

            string gmCommand = null;
            if (System.IO.File.Exists(filePath))
            {
                gmCommand = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(filePath));
                //LuaManager.Instance.CallLuaFunction("ChatManager.OnCSharpGMSend", gmCommand);
            }
            else
            {
                //if (KEngine.IFGame.UpdateRes.build_config != null && KEngine.IFGame.UpdateRes.build_config.res_servers.ContainsKey(KEngine.IFGame.UpdateRes.build_config.res_server_id))
                //{
                //    string resServerUrl = KEngine.IFGame.UpdateRes.build_config.res_servers[KEngine.IFGame.UpdateRes.build_config.res_server_id].res_root;
                //    string fileUrl = resServerUrl + "/GM/" + fileName;
                //    Instance.StartCoroutine(
                //        WWWHelper.Download(fileUrl,
                //        delegate (WWWHelperState retd, byte[] data)
                //        {
                //            if (ret == WWWHelperState.Finish)
                //            {
                //                gmCommand = System.Text.Encoding.UTF8.GetString(data);
                //                LuaManager.Instance.CallLuaFunction("ChatManager.OnCSharpGMSend", gmCommand);
                //            }
                //        }
                //        ));
                //}
            }
        }
    }

    //public void OnGMCommandExe(InputField commandInput)
    //{
    //    if (!string.IsNullOrEmpty(commandInput.text))
    //    {
    //        LuaManager.Instance.CallLuaFunction("ChatManager.OnCSharpGMSend", commandInput.text);
    //    }
    //}
}

