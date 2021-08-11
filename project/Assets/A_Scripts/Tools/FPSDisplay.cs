using UnityEngine;

namespace EazyGF
{
#if MY_DEBUG
    public class FPSDisplay : Singleton<FPSDisplay>
    {
        //刷新频率
        public float fpsMeasuringDelta = 0.5f;

        private float timePassed;
        private int m_FrameCount = 0;
        private float m_FPS = 0.0f;
        private GUIStyle guiStyle;
        private void Start()
        {
            timePassed = 0.0f;
            guiStyle = new GUIStyle();
            guiStyle.normal.background = null;    //设置背景填充的
            guiStyle.normal.textColor = new Color(0f, 1f, 0.3f);   //设置字体颜色
            guiStyle.fontSize = 20;       //字体大小
        }

        private void Update()
        {
            m_FrameCount = m_FrameCount + 1;
            timePassed = timePassed + Time.deltaTime;

            if (timePassed > fpsMeasuringDelta)
            {
                m_FPS = Mathf.RoundToInt(m_FrameCount / timePassed);

                timePassed = 0.0f;
                m_FrameCount = 0;
            }
        }

        private void OnGUI()
        {
            //左下角显示FPS
            GUI.Label(new Rect(Screen.width-150, Screen.height-30, 100, 50), "FPS: " + m_FPS, guiStyle);
        }
    }
#endif
}
