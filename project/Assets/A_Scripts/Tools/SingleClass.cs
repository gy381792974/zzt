using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EazyGF
{
    public class SingleClass<T> where T : SingleClass<T>, new()
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

        }

    }
}
