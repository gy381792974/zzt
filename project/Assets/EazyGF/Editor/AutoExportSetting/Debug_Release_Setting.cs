using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Debug_Release_Setting : Editor
{
    private static string debugSymbo = "MY_DEBUG";
    private static string releaseSymbo = "MY_RELEASE";
    public static void DebugSetting()
    {
        SwitchSymbol(true);
        //todo 可能需要对文件进行设置
    }

    public static void ReleaseSetting()
    {
        SwitchSymbol(false);
        //todo 可能需要对文件进行设置
    }

    private static void SwitchSymbol(bool isDebug)
    {
        string androidSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        string iosSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
        string pcSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);

        SetSymbol(androidSymbol, isDebug, BuildTargetGroup.Android);
        SetSymbol(iosSymbol, isDebug, BuildTargetGroup.iOS);
        SetSymbol(pcSymbol, isDebug, BuildTargetGroup.Standalone);
    }

    private static void SetSymbol(string symbol,bool isDebug, BuildTargetGroup buildTargetGroup)
    {
       string [] symbolArray= symbol.Split(';');
        //目标符号
        string tagetSymbo = isDebug ? debugSymbo : releaseSymbo;
        //相反符号
        string oppSymbo = isDebug ? releaseSymbo : debugSymbo;

        //当前的符号表中是否存在想要设置的符号
        bool isFindSymbolInCurSymbos = false;

        for (int i = 0; i < symbolArray.Length; i++)
        {
            if (symbolArray[i].Equals(tagetSymbo))
            {
                return;
            }
            if (symbolArray[i].Equals(oppSymbo))
            {
                symbolArray[i] = tagetSymbo;
                isFindSymbolInCurSymbos = true;
                break;
            }
        }

        string finalSymbo = string.Empty;
        //如果找到了符号
        if (isFindSymbolInCurSymbos)
        {
            for (int i = 0; i < symbolArray.Length; i++)
            {
                finalSymbo += symbolArray[i] + ";";
            }
        }//没找到就往后面加一个符号
        else
        {
            if (symbol.EndsWith(";"))
            {
                finalSymbo += tagetSymbo + ";";
            }
            else
            {
                finalSymbo +=";"+ tagetSymbo + ";";
            }
        }
        
        PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, finalSymbo);
    }
}
