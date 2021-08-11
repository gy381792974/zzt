using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace EazyGF
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public class TimeHelp
    {
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNowTime()
        {
#if MY_DEBUG
            return DateTime.Now;
#else
            return UnbiasedTime.Instance.Now();
#endif
        }

        /// <summary>
        /// 获取两个时间间隔的秒数
        /// </summary>
        /// <param name="dateTime1"></param>
        /// <param name="dateTime2"></param>
        /// <param name="MaxSecond">允许最大的间隔秒数</param>
        /// <returns></returns>
        public static int DiffSecondByTwoDateTime(DateTime dateTime1, DateTime dateTime2, int MaxSecond = int.MaxValue)
        {
            //间隔的时间
            TimeSpan ts = dateTime1.Subtract(dateTime2).Duration();
            //间隔的秒数
            int difSceonds = CoverDateToSecond(ts);
            difSceonds = difSceonds < MaxSecond ? difSceonds : MaxSecond;
            return difSceonds;
        }
        /// <summary>
        /// 是否有网络
        /// </summary>
        /// <returns></returns>
        public static bool IsHaveNet()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        /// <summary>
        /// 将时间转换为秒数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int CoverDateToSecond(DateTime dateTime)
        {
            TimeSpan ts=new TimeSpan(dateTime.Ticks);
            return CoverDateToSecond(ts);
        }

        public static int CoverDateToSecond(TimeSpan timeSpan)
        {
            return (int)timeSpan.TotalSeconds;
        }

        private static string FinalHour;
        private static string FinalMinute;
        private static string FinalSecon;
        private static TimeSpan timeSpan = new TimeSpan();
        /// <summary>
        /// 将一个数字转换成11：23：33这种形式
        /// </summary>
        /// <returns>/是否有小时</returns>
        public static string CoverNumberToTimer(float Number, bool IsHaveHour)
        {
            timeSpan = TimeSpan.FromSeconds(Number);
            if (IsHaveHour)
            {
                FinalHour = timeSpan.Hours < 10 ? $"0{timeSpan.Hours}" : timeSpan.Hours.ToString();
            }

            FinalMinute = timeSpan.Minutes < 10 ? $"0{timeSpan.Minutes}" : timeSpan.Minutes.ToString();
            FinalSecon = timeSpan.Seconds < 10 ? $"0{timeSpan.Seconds}" : timeSpan.Seconds.ToString();
            if (IsHaveHour)
            {
                return $"{FinalHour}:{FinalMinute}:{FinalSecon}";
            }
            return $"{FinalMinute}:{FinalSecon}";
        }

        /// <summary>
        /// 获取当前时间戳,秒单位
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
    
}
