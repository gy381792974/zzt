//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class IOS_GameCenter : MonoBehaviour
//{
//    //等级
//    static string rankName = "driverank";

//    private static IOS_GameCenter Instance = null;

//    public static void Init()
//    {
//        if (Instance == null)
//        {
//            GameObject go=new GameObject("IOS_GameCenter");
//            go.AddComponent<IOS_GameCenter>();
//        }
//    }

//    private void Awake()
//    {
//#if UNITY_IOS && !UNITY_EDITOR
//        Instance = this;
//        DontDestroyOnLoad(gameObject);
//        Social.localUser.Authenticate(HandleAuthenticated);
//#endif
//    }

//    /// <summary>
//    /// 授权
//    /// </summary>
//    /// <param name="success"></param>
//    /// <param name="message"></param>
//    void HandleAuthenticated(bool success, string message)
//    {
//#if UNITY_EDITOR
//        if (success)
//        {
//            Debug.Log("初始化成功 用户名:" + Social.localUser.userName + " Id " + Social.localUser.id);
//            ReportLevel(1);
//        }
//        else
//        {
//            Debug.Log("初始化失败,错误信息 " + message);
//        }
//#endif
//    }


//    /// <summary>
//    /// 上传等级
//    /// </summary>
//    /// <param name="score"></param>
//    public static void ReportLevel(int officeLevel)
//    {
//#if UNITY_IOS
//        if(Social.localUser.authenticated)
//            Social.ReportScore(officeLevel, rankName, HandleScoreReported);
//#endif
//    }

//    static void HandleScoreReported(bool success)
//    {
//#if UNITY_EDITOR
//        Debug.Log("上传状态: " + success);
//#endif
//    }
//    //打开排行榜
//    public static void OpenRankBoard()
//    {
//#if UNITY_IOS && !UNITY_EDITOR
//        if (Social.localUser.authenticated)
//            Social.ShowLeaderboardUI();
//#endif
//    }
//}
