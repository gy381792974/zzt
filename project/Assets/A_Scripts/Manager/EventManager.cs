using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EazyGF
{
    public enum EventKey
    {
        CubeGameEvent,        //方块游戏
        UnLockBuildEvent,        //解锁
        GetNewBuild,        //得到新的
        SelectBuildEvent,   //选择使用的建筑
        UdpatePlayData, // 更新玩家数据
        SpeRoleLeave, // 特殊顾客离开事件
        SpeRoleTriggerEvent, // 特殊顾客事件触发
        NCDiningEnd, //顾客用餐结束

        DancingGameVictory,  //下棋胜利触发事件
        DancingPlayAction,   //芭蕾舞者触发事件
        UpdateCubeEvent,
        KitChenLevelChange,   //厨房等级改变事件
        CheckoutCounterLevelUpdate, // 收银台等级改变

        SuccClearCEvent, //成功消除一对方块的事件
        ItemNumUpdate,

        EnterGetFoodPos,        //到摊位的取餐点事件

        LeaveGetGoodPos,        //离开摊位的取餐点事件

        MoveCamerToTargetPos,   //移动摄像机到目标位置

        MoveCamerToTargetPos2,   //移动摄像机到目标位置2

        StaffDataUpdate,   //职员数据更新

        SendBubbleData, //发送气泡数据

        FastCreateCusBuff,  //快速创建顾客

        CanceLoopBubble,  //取消循环泡泡

        SendCBBData, //发送通用泡泡数据
        RecycleCBBData, //回收通用泡泡数据

        BuildComDataUdpate, //组合类型的建筑升级

        BuildAreaDataUpdate, //建筑数据更新

        TestEvent,
        Null,
    }

    public class EventManager
    {
        private class EventNode : UnityEvent<object>
        {

        }

        private Dictionary<EventKey, EventNode> eventDictionary = new Dictionary<EventKey, EventNode>();
        private static EventManager eventManager = new EventManager();

        public static EventManager Instance
        {
            get
            {
                return eventManager;
            }
        }

        public void RegisterEvent(EventKey eventKey, UnityAction<object> listener)
        {
            EventNode thisEvent = null;
            if (eventManager.eventDictionary.TryGetValue(eventKey, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new EventNode();
                thisEvent.AddListener(listener);
                eventManager.eventDictionary.Add(eventKey, thisEvent);
            }
        }

        public void RemoveListening(EventKey eventKey, UnityAction<object> listener)
        {
            if (eventManager == null) return;
            EventNode thisEvent = null;
            if (eventManager.eventDictionary.TryGetValue(eventKey, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public void TriggerEvent(EventKey eventKey, object mess)
        {
            EventNode thisEvent = null;
            if (eventManager.eventDictionary.TryGetValue(eventKey, out thisEvent))
            {
                thisEvent.Invoke(mess);
            }
        }

        internal void RegisterEvent(EventKey aCTION_OUT_END, object onOut)
        {
            throw new NotImplementedException();
        }

        internal void RegisterEvent(object mAP_BG_SHOW, Action<object> bgNodeShow)
        {
            throw new NotImplementedException();
        }
    }
}