using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EazyGF;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DebugTools : Singleton<DebugTools>
{
    private Button []toolsBtnArray;
    private Text logText;
    public Text lEventText;
    private Transform debugPanelTrans;
    public InputField if1;

    //是否显示帧数
    private bool isShowFPS=false;
    //是否显示Debug面板
    private bool isShowDebugPanel = false;
    public void Init()
    { 
        Application.logMessageReceived += Application_logMessageReceived;
        debugPanelTrans = transform.Find("DebugPanel");
        toolsBtnArray = debugPanelTrans.Find("Btns").GetComponentsInChildren<Button>();
        logText = debugPanelTrans.Find("ScRect/LogText").GetComponent<Text>();
        
        //加金币
        toolsBtnArray[0].onClick.AddListener(() =>
        {
            // PlayerDataMgr.SetGold(PlayerDataMgr.GetGold() * 10);

            ItemPropsManager.Intance.AddItem(1, 100000);

            EventManager.Instance.TriggerEvent(EventKey.UdpatePlayData, null);
        });
        //加星级
        toolsBtnArray[1].onClick.AddListener(() =>
        {
            //PlayerDataMgr.SetScoolStar(PlayerDataMgr.GetSchoolStar() + 500);

            ItemPropsManager.Intance.AddItem(2, 100000);
            EventManager.Instance.TriggerEvent(EventKey.UdpatePlayData, null);
        });
        //加魔法石
        toolsBtnArray[2].onClick.AddListener(() =>
        {
            //PlayerDataMgr.SetMagicStone(PlayerDataMgr.GetMagicStone() + 100);
        });
        //加速
        toolsBtnArray[3].onClick.AddListener(() =>
        {
            Time.timeScale += 1;
        });
        //正常速度
        toolsBtnArray[4].onClick.AddListener(() =>
        {
            Time.timeScale = 1;
        }); 
        //一键满级
        //toolsBtnArray[5].onClick.AddListener(AllUnLock);
        //显示帧数
        toolsBtnArray[6].onClick.AddListener(ShowFPS);
        //加巫师经验
        toolsBtnArray[7].onClick.AddListener(() =>
        {
            ShowOrHidePanel();
        });

        //
        toolsBtnArray[8].onClick.AddListener(() =>
        {
            if (int.TryParse(if1.text, out int value))
            {
                CubeGameMgr.Instance.SetCubeLevel(if1.text.ToInt());
                CubeGameMgr.Instance.isPause = false;
                UIMgr.HideUI<PausePanel>();

                CubeMainPanel cubeMainPanel = UIMgr.GetUI<CubeMainPanel>();
                if (cubeMainPanel != null)
                {
                    cubeMainPanel.OnReset();
                    CubeGameMgr.Instance.ClearSelected();
                }

                ShowOrHidePanel();
            }

        });

        //事件开关
        //toolsBtnArray[8].onClick.AddListener(() =>
        //{
        //    if (!SpecialEventMgr.AllEventPause)
        //    {
        //        lEventText.text = "打开所有事件";
        //        SpecialEventMgr.AllEventPause = true;
        //        SpecialEventMgr.Instance.EventOver();
        //        TouchAdMgr.Instance.EventOver();
        //         SceneEventMgr.Instance.EventOver();
        //    }
        //    else
        //    {
        //        lEventText.text = "关闭所有事件";
        //        SpecialEventMgr.AllEventPause = false;
        //        SpecialEventMgr.Instance.Init();
        //        TouchAdMgr.Instance.Init();
        //        SceneEventMgr.Instance.Init();
        //    }
        //});
        ////刷新金币事件
        //toolsBtnArray[9].onClick.AddListener(() =>
        //{
        //    GlobeFunction.GameStoneChangeEvent.Invoke();
        //    GlobeFunction.GameStarChangeEvent.Invoke();
        //    GlobeFunction.GameGoldChangeEvent.Invoke("", 1000);
        //});
        HideDebugPanel();
        ShowFPS();
    }

    private void ShowFPS()
    {
#if MY_DEBUG
        FPSDisplay.Instance.gameObject.SetActive(!isShowFPS);
        isShowFPS = !isShowFPS;
#endif
    }

    private void ShowDebugPanel()
    {
        debugPanelTrans.gameObject.SetActive(true);
    }

    private void HideDebugPanel()
    {
        debugPanelTrans.gameObject.SetActive(false);
    }
    private const int maxLenth = 5000;
    private StringBuilder mLogSBuilder = new StringBuilder(maxLenth);
    private StringBuilder mLogShowBuilder = new StringBuilder(maxLenth);
    private const string endSimple = "&*#";//结尾后缀下标
    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        if (mLogSBuilder.Length >= maxLenth)
        {
            int endIndex = mLogSBuilder.ToString().IndexOf(endSimple, StringComparison.Ordinal);

            mLogSBuilder.Remove(0, endIndex+3);
            mLogShowBuilder.Remove(0, endIndex);
        }

        string logValue = string.Empty;
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
            case LogType.Assert:
                logValue = $"<color=red>{condition}</color>";
                AppendLogValue(logValue);
                break;
            case LogType.Warning:
                logValue = $"<color=yellow>{condition}</color>";
                AppendLogValue(logValue);
                break;
            case LogType.Log:
                logValue = $"<color=whrite>{condition}</color>";
                AppendLogValue(logValue);
                break;
        }
    }

    private void AppendLogValue(string value)
    {
        mLogSBuilder.AppendLine($"{value}{endSimple}");
        mLogShowBuilder.AppendLine(value);
    }
    
    private float time = 0;
    private void Update()
    {
        logText.text = mLogShowBuilder.ToString();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //显示调试窗口
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowOrHidePanel();
        }
        //调节游戏速度
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale += 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.timeScale <= 1)
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale - 0.25f, 0.01f, 10f);
            }
            else
            {
                Time.timeScale = Mathf.Clamp(Time.timeScale - 1, 0.01f, 10f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Time.timeScale = 1;
        }

#else
        if (Input.touchCount >3 && Time.time - time>3)
        {
            time=Time.time; 
            ShowOrHidePanel();
        }
#endif
    }

    public void ShowOrHidePanel()
    {
        if (!isShowDebugPanel)
        {
            ShowDebugPanel();
        }
        else
        {
            HideDebugPanel();
        }

        isShowDebugPanel = !isShowDebugPanel;
    }


}
