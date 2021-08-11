
using UnityEngine;

namespace EazyGF
{
    public class BattleLogic
    {
        public static BattleLogic battleLogic;

        public bool isOpenSound;
        public bool isOpenMusic;

        public static BattleLogic Intance
        {
            get
            {
                if (battleLogic == null)
                {
                    battleLogic = new BattleLogic();
                    battleLogic.Init();
                }
                 return battleLogic;
            }
        }

        public void Init()
        {

        }
        public void IsCloseBGM(bool isPause)
        {
            MusicMgr.Instance.IsCloseBG = isPause;
            //if (isPause)
            //{
            //    MusicMgr.Instance.PauseBG();
            //}
            //else
            //{
            //    MusicMgr.Instance.ResumBG();
            //}
        }

        public void IsCloseEFF(bool isPause)
        {
            MusicMgr.Instance.IsCloseEff = isPause;
        }

    }
}
