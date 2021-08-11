using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace IFGame
{
    class WatchDog : MonoBehaviour
    {
        public static WatchDog Instance { get; private set; }

        public enum LogLevel
        {
            Log,
            Warning,
            Error,
            Assert,
            Exception,
            None,
        }

        public int totalLines = 20;
        public LogLevel logLevel = LogLevel.Error;
        private int fontSize = 15;

        private LogLevel dynLogLevel
        {
            get
            {
                SwitchOnOffPanel panel = SwitchOnOffPanel.Instance;
                if (panel == null)
                {
                    //#if IFGAME_RELEASE
                    //                    return LogLevel.None;
                    //#elif IFGAME_DEBUG
                    //                    return LogLevel.Log;
                    //#else
                    //                    return logLevel;
                    //#endif

                    return logLevel;
                }
                return (LogLevel)panel.getSwitchOnoffValue(SwitchOnoffKey.DISPLAYLOG);
            }
        }

        class LogLine
        {
            public string lMsg;
            public LogType lType;
            public Exception e;

            public LogLine(string l, LogType lt)
            {
                lMsg = l;
                lType = lt;
            }

            public LogLine(Exception e)
            {
                this.lType = LogType.Exception;
                this.e = e;
            }
        }

        List<LogLine> m_logs = new List<LogLine>();
        List<string> m_filelogs = new List<string>();

        List<LogLine> m_thread_log_cache_ = new List<LogLine>();
        object thread_log_lock_ = new object();

        public void ThreadLog(LogType lt, string message)
        {
            lock (thread_log_lock_)
            {
                m_thread_log_cache_.Add(new LogLine(message, lt));
            }
        }

        public void ThreadLog(Exception e)
        {
            lock (thread_log_lock_)
            {
                m_thread_log_cache_.Add(new LogLine(e));
            }
        }
        GUIStyle fontStylel;

        void Awake()
        {
            WatchDog.Instance = this;

            Application.logMessageReceived += LogCallback;

            fontStylel = new GUIStyle();

            fontStylel.fontSize = fontSize;
        }
  

        void LogCallback(string log, string stack, LogType type)
        {
            if (m_logs.Count >= totalLines)
            {
                m_logs.RemoveAt(0);
                m_filelogs.RemoveAt(0);
            }


            if (type == LogType.Warning)
            {
                log = string.Format("<color=yellow>{0}</color>", log);
            }
            else if (type == LogType.Error || type == LogType.Assert || type == LogType.Exception)
            {
                string msg = log;
                string l = "";

                while (msg.Length > 50)
                {
                    l += msg.Substring(0, 50) + "\n";

                    msg = msg.Remove(0,50);
                }

                l += msg + "\n";

                log = string.Format("<color=red>{0}</color>", l);
            }

            m_logs.Add(new LogLine(log, type));
            m_filelogs.Add(type + ": " + log);
            if (type != LogType.Log && type != LogType.Warning)
            {
                m_logs.Add(new LogLine(string.Format("<color=red>{0}</color>", stack), type));
                m_filelogs.Add(stack);
            }
        }

        //#if !IFGAME_RELEASE
        //#endif

        void OnGUI()
        {
            if (dynLogLevel != LogLevel.None)
            {
                for (int i = m_logs.Count - 1; i >= 0; --i)
                {
                    if (GetLogLevel(m_logs[i].lType) >= dynLogLevel)
                    {
                        GUILayout.Label(m_logs[i].lMsg, fontStylel);
                    }
                }
            }
        }

        LogLevel GetLogLevel(LogType lt)
        {
            switch (lt)
            {
                case LogType.Error:
                    return LogLevel.Error;
                case LogType.Assert:
                    return LogLevel.Assert;
                case LogType.Warning:
                    return LogLevel.Warning;
                case LogType.Log:
                    return LogLevel.Log;
                case LogType.Exception:
                    return LogLevel.Exception;
            }
            return LogLevel.None;
        }
    }

}