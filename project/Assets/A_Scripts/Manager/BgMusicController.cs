using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EazyGF
{
    public class BgMusicController : MonoBehaviour
    {

        private string[] musicNum = new string[] { "d_bg_1"};
        private string[] musicCubeNum = new string[] { "c_bg_1", "c_bg_2" };

        private int currentPlayMusicIndex = 0;
        private int currentPlayCubeMusicIndex = 0;

        private int currentPlayType = 0;

        private bool isLoopMusic = true;
        private bool isResetPlay = true;

        private void Start()
        {
            EventManager.Instance.RegisterEvent(EventKey.CubeGameEvent, CubeGameEvent);
        }

        int type = 1;
        private void CubeGameEvent(object arg0)
        {
            currentPlayMusicIndex = 0;
            currentPlayCubeMusicIndex = 0;

            currentPlayType = (int)arg0;
            MusicMgr.Instance.ResumBG();
            if (MusicMgr.Instance.IsCloseBG)
            {
                MusicMgr.Instance.PauseBG();
            }
            isResetPlay = true;
        }

        void Update()
        {
            if (isLoopMusic)
            {
                if ((!MusicMgr.Instance.IsPlayingMusic() || isResetPlay)&&!MusicMgr.Instance.IsCloseBG)
                {
                    StartCoroutine(PlayerBattleMusic());
                }
            }
        }

        private IEnumerator PlayerBattleMusic()
        {
            isLoopMusic = false;

            if (isResetPlay)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }

            isResetPlay = false;
            MusicMgr.Instance.ResumBG();
           
            if (currentPlayType == 0)
            {
                MusicMgr.Instance.PlayMusicBG(musicNum[currentPlayMusicIndex % musicNum.Length], false);
                currentPlayMusicIndex++;
            }
            else
            {
                MusicMgr.Instance.PlayMusicBG(musicCubeNum[currentPlayCubeMusicIndex % musicCubeNum.Length], false);
                currentPlayCubeMusicIndex++;
            }

            isLoopMusic = true;
        }
    }
}