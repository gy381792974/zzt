using UnityEngine;
using System;

namespace HunliGame
{
    /**
     * 1. use server timezone
     * 2. ignore local clock, because it's affected by local device clock settings
     * 3. evaluate network lag continuously
     */
    public class GameClock : MonoBehaviour
    {
        public static void Init()
        {
            var go = new GameObject("GameClock", typeof(GameClock));
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            DontDestroyOnLoad(go);
        }

        #region time
        public static DateTime utcStart { get { return new DateTime(1970, 1, 1); } }

        /// <summary>
        /// synchronized clock time
        /// </summary>
        public static DateTime now { get { return utcStart.AddSeconds(nowTS + timezone); } }
        public static long ticks { get { return now.Ticks; } }
        public static double nowTS
        {
            get
            {
                //if (null != instance && instance.mServerUtc > 0)
                //    return instance.mServerUtc + (Time.realtimeSinceStartup - instance.mLastUpdate);
                //else
                //    return (DateTime.UtcNow - utcStart).TotalSeconds;

                return (DateTime.UtcNow - utcStart).TotalSeconds;
            }
        }

        public static double ClientTS { get { return (DateTime.UtcNow - utcStart).TotalSeconds; } }

        /// <summary>
        /// connection lag
        /// </summary>
        public static float lag
        {
            get
            {
                if (null != instance)
                {
                    int i = 0;
                    float t = 0f;
                    float[] lags = instance.mLags;
                    for (; i < lags.Length && lags[i] > 0f; ++i)
                        t += lags[i];
                    return i > 0 ? t / i : 0f;
                }
                else
                {
                    return 0f;
                }
            }
        }

        public static double timezone
        {
            get { return null != instance ? instance.mTimezone : defaultTimezone; }
            set
            {
                if (null != instance)
                {
                    instance.mTimezone = value;
                    Debug.Log(string.Format(
                        "<color=green>GameClock> time zone updated: {0:f2}</color>",
                        instance.mTimezone));
                }
            }
        }

        public static DateTime ToDateTime(double t)
        {
            return utcStart.AddSeconds(t + timezone);
        }

        public static double ToTimestamp(DateTime t)
        {
            return (t.AddSeconds(-timezone) - utcStart).TotalSeconds;
        }

        public static string ToUrl(string url)
        {
            return string.Format("{0}?ts={1}", url, now.Ticks);
        }

        /// <summary>
        /// 将表格配置中的时间串转换为DateTime类型时间
        /// 表格中的时间是北京时间, 所以最后返回的时候转换成了本地时间
        /// </summary>
        /// <param name="timestr">string 表格配置中的时间串(2015年4月21日19点28分30秒：20150421-192830)</param>
        /// <returns>DateTime</returns>
        public static DateTime ParseTableTime(string timestr)
        {
            if (timestr.Length != 15)
                throw new Exception("invalid table time format: " + timestr);

            int year = int.Parse(timestr.Substring(0, 4));
            int month = int.Parse(timestr.Substring(4, 2));
            int day = int.Parse(timestr.Substring(6, 2));
            int hour = int.Parse(timestr.Substring(9, 2));
            int minute = int.Parse(timestr.Substring(11, 2));
            int second = int.Parse(timestr.Substring(13, 2));

            var time = new DateTime(year, month, day, hour, minute, second);
            return time.AddSeconds(timezone - 28800);
        }

        public static double ParseTableTimeToTS(string timestr)
        {
            if (timestr.Length != 15)
                return -1;

            int year = int.Parse(timestr.Substring(0, 4));
            int month = int.Parse(timestr.Substring(4, 2));
            int day = int.Parse(timestr.Substring(6, 2));
            int hour = int.Parse(timestr.Substring(9, 2));
            int minute = int.Parse(timestr.Substring(11, 2));
            int second = int.Parse(timestr.Substring(13, 2));

            var time = new DateTime(year, month, day, hour, minute, second);
            var currentTime = time.AddSeconds(timezone - 28800);
            return (currentTime.AddSeconds(-timezone) - utcStart).TotalSeconds;
        }

        #endregion time

        #region internal
        const int lagEvalCount = 100;

        double mServerUtc;
        double mTimezone;
        float mLastUpdate;

        int mFillIndex;
        float[] mLags = new float[100];

        static GameClock instance;

        static double defaultTimezone { get { return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds; } }

        void Awake()
        {
            instance = this;
            mTimezone = defaultTimezone;
            Debug.Log(string.Format(
                "<color=green>GameClock> default time zone: {0:f2}</color>",
                mTimezone));
        }
        #endregion internal
    }
}
