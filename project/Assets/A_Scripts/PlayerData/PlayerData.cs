using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;

namespace EazyGF
{

    /// <summary>
    /// 玩家数据类
    /// 因为版本覆盖导致的兼容问题，该类的字段在上线后不允许改名字和删除字段，只允许添加字段，
    /// 如果更改了数据类型，比如Money，原本不是数组类型，
    /// 现在需要更改为数组类型，那么需要这么处理：1，添加一个Money的数组，假设名字为Moneys
    /// 2，在版本号不一致时将原本的Money赋值给数组里的某个元素
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        // 必要的字段，用来判断两次保存的值，作为极限存储的依据--也是用来作为离线的时间判断
        public DateTime playerOfflineTime = DateTime.Now; //玩家离线时间
        public string gameVersion; //版本号
        public long goldNum = 100;//金币数量
        public long starNum = 1;//星星
        internal int curChapterID;
        public Test test;
        public PlayerData()
        {
           
            test = new Test();
            test.a = 0;

        }
    }
    [Serializable]
    public class Test
    {
       public int a;
       public int b;

    }
}



