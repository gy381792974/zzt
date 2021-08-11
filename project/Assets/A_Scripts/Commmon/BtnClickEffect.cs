using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button", menuName = "New Button", order = 1)]
public class BtnClickEffect : ScriptableObject
{
    public void Btn_ClickAudioEffect()
    {
        MusicMgr.Instance.PlayMusicEff("g_btn_gen");
    }
    public void Btn_CloseWin()
    {
        MusicMgr.Instance.PlayMusicEff("g_win_close");
    }
}
