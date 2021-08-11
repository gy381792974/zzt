using UnityEngine;

namespace EazyGF
{

    public class CubeMainPanelData : UIDataBase
    {
        public CubeMainPanelData()
        {

        }
    }

    public partial class CubeMainPanel : UIBase
    {
        int[] ids = new int[3] { 10, 11, 12 };

        float levelTimer = 0;

        int totalTimer = 0;

        public CubeGrids cubeGrids;
        MainPanel mainPanel;

        bool isPlayStarLightSound = false;

        protected override void OnInit()
        {
            mainPanel = UIMgr.GetUI<MainPanel>();
            HelpBtn_btn.onClick.AddListener(() =>
            {
                UIMgr.ShowPanel<HelpPanel>();
            });

            PauseBtn_btn.onClick.AddListener(() =>
            {
                UIMgr.ShowPanel<PausePanel>();
            });

            cubeGrids.OnInit();
        }

        public void AddTime(int time)
        {
            levelTimer += time;
            if (levelTimer > totalTimer)
            {
                levelTimer = totalTimer;
            }
        }

        protected override void OnShow(UIDataBase cubemainpanelData = null)
        {
            if (cubemainpanelData != null)
            {
                mPanelData = cubemainpanelData as CubeMainPanelData;
            }

            UpdateData();

            UpdateItemEvent(null);
            EventManager.Instance.RegisterEvent(EventKey.ItemNumUpdate, UpdateItemEvent);
            EventManager.Instance.RegisterEvent(EventKey.SuccClearCEvent, SuccClearCEvent);
            cubeGrids.OnShow();
            EventManager.Instance.TriggerEvent(EventKey.CubeGameEvent, 1);

            isPlayStarLightSound = false;
        }

        private void UpdateData()
        {
            int level = CubeGameMgr.Instance.curLevel;
            LevelText_text.text = $"level.{level}";

            Revival();
        }
        public void Revival()
        {
            isStart = true;//游戏开始
            CubeGameMgr.Instance.isPause = false;//游戏变为非暂停状态
            levelTimer = CubeGameMgr.Instance.GetCurLevelTimer();//获取进度条的计时器
            totalTimer = (int)levelTimer;
            LightAllStars();
        }

        protected override void OnHide()
        {
            EventManager.Instance.RemoveListening(EventKey.ItemNumUpdate, UpdateItemEvent);
            EventManager.Instance.RemoveListening(EventKey.SuccClearCEvent, SuccClearCEvent);
            mainPanel.ShowCurLevel();
            cubeGrids.OnHide();

            EventManager.Instance.TriggerEvent(EventKey.CubeGameEvent, 0);
        }

        public void SuccClearCEvent(object obj)
        {
            AddTime(AppConst_Data.DataArray.RemoveCubeAddTime);
        }

        public void OnReset()
        {
            CubeGameMgr.Instance.ClearRFlayCI();
            OnHide();
            OnShow();
        }

        bool isStart;

        float timer = 0;
        float timeInterval = 1;
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= timeInterval)
            {
                timer = 0;
            }

            UdpateLevelTimer();
        }

        int lastStarNum = 0;

        public void UdpateLevelTimer()
        {
            if (isStart && !CubeGameMgr.Instance.isPause)
            {
                levelTimer -= Time.deltaTime;

                if (levelTimer > 0)
                {
                    FillSlide_img.fillAmount = levelTimer / totalTimer;

                    UpdateStar();
                }
                else
                {
                    UIMgr.ShowPanel<CubeFailPanel>();
                    levelTimer = 0;
                    isStart = false;
                    FillSlide_img.fillAmount = 0;

                }
            }
        }
        bool isTimeOut = true;

        private void LightAllStars()
        {
            //点亮所有的星星
            //lastStarNum = sgs.Count;
            for (int i = 0; i < sgs.Count; i++)
            {
                sgs[i].gameObject.SetActive(true);
                sgs[i].AnimationState.SetAnimation(0, "animation", false);
                lockStartsObj[i].SetActive(true);
            }
        }

        private void UpdateStar()
        {
            int curStrNum = 0;

            if (FillSlide_img.fillAmount > 0.6f)
            {
                curStrNum = 3;
            }
            else if (FillSlide_img.fillAmount > 0.3f)
            {
                curStrNum = 2;
            }
            else if (FillSlide_img.fillAmount > 0.02f)
            {
                curStrNum = 1;
            }
            if (FillSlide_img.fillAmount > 0.05f && !isTimeOut)
            {
                isTimeOut = true;
            }
            if (FillSlide_img.fillAmount <= 0.05f && isTimeOut)
            {
                isTimeOut = false;
                MusicMgr.Instance.PlayMusicEff("c_time_out");
            }
            if (lastStarNum != curStrNum)
            {

                CubeGameMgr.Instance.startNum = curStrNum;

                //for (int i = 0; i < 3; i++)
                //{
                //    bool isShow = curStrNum >= i + 1;

                //    if (lockStartsObj[i].gameObject.activeSelf && isShow)
                //    {
                //        if (!sgs[i].gameObject.activeSelf)
                //        {
                //            MusicMgr.Instance.PlayMusicEff("c_star_light");
                //        }
                //        sgs[i].gameObject.SetActive(true);
                //        sgs[i].timeScale = 0.6f;
                //        sgs[i].AnimationState.SetAnimation(0, "animation", false);
                //    }
                //    else
                //    {
                //        sgs[i].gameObject.SetActive(false);
                //        MusicMgr.Instance.PlayMusicEff("c_star_fail");
                //    }

                //    lockStartsObj[i].SetActive(!isShow);
                //}
                if (curStrNum > lastStarNum)
                {
                    sgs[lastStarNum].gameObject.SetActive(true);
                    sgs[lastStarNum].timeScale = 0.6f;
                    sgs[lastStarNum].AnimationState.SetAnimation(0, "animation", false);

                    if (isPlayStarLightSound)
                    {
                        MusicMgr.Instance.PlayMusicEff("c_star_light");
                    }

                    isPlayStarLightSound = true;

                }
                else
                {
                    sgs[curStrNum].gameObject.SetActive(false);
                    MusicMgr.Instance.PlayMusicEff("c_star_fail");
                }
                lastStarNum = curStrNum;
            }
        }

        private void UpdateItemEvent(object obj)
        {
            for (int i = 0; i < cubeProps.Count; i++)
            {
                cubeProps[i].BindData(ids[i]);
            }

            GoldTxt_text.text = ItemPropsManager.Intance.GetItemNum(1).ToString();
            StarTxt_text.text = ItemPropsManager.Intance.GetItemNum(2).ToString();
        }
    }
}
