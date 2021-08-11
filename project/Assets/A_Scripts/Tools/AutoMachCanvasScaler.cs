using UnityEngine;
using UnityEngine.UI;

namespace EazyGF
{
    public class AutoMachCanvasScaler : Singleton<AutoMachCanvasScaler>
    {
        private readonly Vector2 BaseResolution = new Vector2(720f, 1280f);
        public float MatchWidth = 0;
        public float MatchHeight = 1;

#if UNITY_EDITOR
       [SerializeField] private float _screeWidth;
       [SerializeField] private float _screenHeight;
#endif

        public void Init()
        {
#if UNITY_EDITOR

            _screenHeight = Screen.height;
            _screeWidth = Screen.width;
#endif

            SetScaler();
        }

        private void SetScaler()
        {
            CanvasScaler scaler = GetComponent<CanvasScaler>();
            if (null == scaler)
            {
                return;
            }

            float scale = (float)Screen.height / Screen.width;

            if (scale < BaseResolution.y / BaseResolution.x)
            {
                scaler.matchWidthOrHeight = MatchHeight;
            }
            else
            {
                scaler.matchWidthOrHeight = MatchWidth;
            }
        }

#if UNITY_EDITOR
        //private void Update()
        //{
        //    if (Screen.height != _screenHeight || Screen.width != _screeWidth)
        //    {
        //        SetScaler();
        //        _screeWidth = Screen.width;
        //        _screenHeight = Screen.height;
        //    }
        //}

#endif
    }
}
