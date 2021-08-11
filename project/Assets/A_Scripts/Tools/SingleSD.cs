using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace EazyGF
{
    public class SingleSD<T> where T : SingleSD<T>, new()
    {
        protected static T t = null;

        public static T Instance
        {
            get
            {
                if (t == null)
                {
                    t = new T();
                    t.Init();
                }
                return t;
            }
        }

        public virtual void Init()
        {
            if (!ReadData())
            {
                MInitData();

                SaveData();
            }
        }

        protected virtual void MInitData()
        {

        }

        protected virtual string GetSavaDataName()
        {
            return "";
        }

        protected virtual void SaveData()
        {

        }

        protected virtual bool ReadData()
        {
            return true;
        }


        protected string savePath;
        protected string SavePath
        {
            get
            {
                if (string.IsNullOrEmpty(savePath))
                {
                    savePath = UICommonUtil.GetSerSavePath(GetSavaDataName());
                }

                return savePath;
            }
        }
    }
}
