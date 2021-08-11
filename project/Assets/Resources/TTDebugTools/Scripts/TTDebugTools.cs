using UnityEngine;
using UnityEngine.Profiling;
using System;
using UnityEngine.UI;

namespace HunliGame
{
    public class TTDebugTools : MonoBehaviour
    {

        private bool isRunInBackground = true;
        public bool isShowLog = true;
        public Button panelBtn;
        public int timeScale;

        private void Awake()
        {
            Debug.unityLogger.logEnabled = isShowLog;
            Application.runInBackground = isRunInBackground;
            //Application.targetFrameRate = 60;

            panelBtn.onClick.AddListener(() => {
                SwitchOnOffPanel.ShowPanel();
                Debug.Log("DebugSwicthPanel");
            });

            Time.timeScale = timeScale;
        }

        private void Start()
        {
            //gameObject.SetActive(App.GameConfig.IsDebug);
        }

#if !IFGAME_RELEASE

        void Update()
        {
            UpdateTick();
            //Application.targetFrameRate = 60;

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (SwitchOnOffPanel.Instance == null)
                {
                    SwitchOnOffPanel.ShowPanel();
                }
                else
                {
                    SwitchOnOffPanel.Instance.gameObject.SetActive(!SwitchOnOffPanel.Instance.gameObject.activeSelf);
                }

                //SwitchOnOffPanel.Instance.gameObject.SetActive(!SwitchOnOffPanel.Instance.gameObject.activeSelf);
            }
        }

        //void OnGUI()
        //{
        //    if (true)
        //    {
        //        DrawFps();
        //    }
        //}

        static string usedMEM = "-1";
        private void DrawFps()
        {
            GUIStyle style = new GUIStyle();
            if (mLastFps > 25)
            {
                style.normal.textColor = new Color(0, 1, 0);
            }
            else if (mLastFps > 18)
            {
                style.normal.textColor = new Color(1, 1, 0);
            }
            else
            {
                style.normal.textColor = new Color(1.0f, 0, 0);
            }

            style.fontSize = 50;
            style.alignment = TextAnchor.UpperRight;
            GUI.Label(new Rect(Screen.width - 160, 16, 160, 32), "fps: " + mLastFps, style);
            style.normal.textColor = new Color(0, 1, 0);

            if (GUI.Button(new Rect(Screen.width - 80, Screen.height / 2 - 80, 140, 50), "Debug"))
            {
                SwitchOnOffPanel.ShowPanel();
                Debug.Log("DebugSwicthPanel");
            }
        }

        private int mFrameCount = 0;
        private long mLastFrameTime = 0;
        static int mLastFps = 0;
        static int TotalFps = 0;
        static int RecordCount = 0;
        static int MinFps = 0;
        static int MaxFps = 0;
        static bool isRecordingFps = false;

        public static void StartRecordFPS()
        {
            if (!isRecordingFps)
            {
                TotalFps = 0;
                MinFps = int.MaxValue;
                MaxFps = 0;
                RecordCount = 0;
                isRecordingFps = true;
            }
        }

        public static void StopRecordFPS()
        {
            isRecordingFps = false;
        }

        public static void StopRecordFPS(out int average, out int min, out int max)
        {
            average = RecordCount > 0 ? TotalFps / RecordCount : 0;
            min = MinFps;
            max = MaxFps;
            isRecordingFps = false;
        }


        //根据千分之一秒计算fps
        private void UpdateTick()
        {
            if (true)
            {
                mFrameCount++;

                long nCurTime = TickToMilliSec(GameClock.now.Ticks);

                if (mLastFrameTime == 0)
                {
                    mLastFrameTime = TickToMilliSec(GameClock.now.Ticks);
                }

                if ((nCurTime - mLastFrameTime) >= 1000)
                {
                    int fps = (int)(mFrameCount * 1.0f / ((nCurTime - mLastFrameTime) / 1000.0f));

                    mLastFps = fps;

                    mFrameCount = 0;

                    mLastFrameTime = nCurTime;

                    showFpsTxt.text = "Fps:" + mLastFps.ToString();
                }
            }
        }

        public Text showFpsTxt;

        //得到千分之一秒
        public static long TickToMilliSec(long tick)
        {
            if (tick % 10 == 0)
            {
                //Debug.LogError("currentDateTime " + Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds) + "-- tick  ---" + tick
                // + " ----- " + GameClock.now.Ticks + "  - " + DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).Ticks);

            }
            return tick / (10 * 1000);
        }
#endif
    }
}