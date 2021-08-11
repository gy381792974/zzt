using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BtnClickAnim : MonoBehaviour
{
   private EventTrigger eventTrigger;
    private RectTransform rect;
    private Button btn;//对应按钮
    private bool stay = false;//是否停留在按钮上动
    private bool tweenPlay = false;//是否播放按钮缩小动画
    private bool tweenPlayBack = false;//是否播放按钮放大动画
    private float subT;
    private float iniSubT = 0.2f;
    private float minSubT = 0.075f;

    public static bool ClickOver = false;
    public float btnSamllRate = 0.85f;
    public float btnBigRate = 1f;

    private Vector3 btnSmallVec3;
    private Vector3 btnBigVec3;
    void Awake()
    {
        rect = this.GetComponent<RectTransform>();
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        btn = gameObject.GetComponent<Button>();
        EventTrigger.Entry entry0 = new EventTrigger.Entry();// 按下
        entry0.eventID = EventTriggerType.PointerDown;
        entry0.callback = new EventTrigger.TriggerEvent();
        entry0.callback.AddListener(PointerDown);
        EventTrigger.Entry entry1 = new EventTrigger.Entry();// 离开
        entry1.eventID = EventTriggerType.PointerExit;
        entry1.callback = new EventTrigger.TriggerEvent();
        entry1.callback.AddListener(PointerExit);
        EventTrigger.Entry entry2 = new EventTrigger.Entry();// 按住
        entry2.eventID = EventTriggerType.Drag;
        entry2.callback = new EventTrigger.TriggerEvent();
        entry2.callback.AddListener(OnDrag);
        EventTrigger.Entry entry3 = new EventTrigger.Entry();// 按住
        entry3.eventID = EventTriggerType.PointerUp;
        entry3.callback = new EventTrigger.TriggerEvent();
        entry3.callback.AddListener(OnDrag);
        eventTrigger.triggers.Add(entry0);
        eventTrigger.triggers.Add(entry1);
        eventTrigger.triggers.Add(entry2);
        eventTrigger.triggers.Add(entry3);

        btnSmallVec3=new Vector3(btnSamllRate,btnSamllRate,btnSamllRate);
        btnBigVec3=new Vector3(btnBigRate, btnBigRate, btnBigRate);
    }

    public void SetClickOver()
    {
        ClickOver = true;
    }


    public void OnDrag(BaseEventData pointData)
    {
        stay = false;
    }
    

    public void PointerDown(BaseEventData pointData)
    {
        tweenPlayBack = true;
        BtnDoSamllAnimation();

        stay = true;
        subT = iniSubT;
        ClickOver = false;
        this.StartCoroutine(EnterStayButton(pointData));
    }
    
    public void PointerExit(BaseEventData pointData)
    {
        BtnDoBigAnimation();
        stay = false;
        ClickOver = true;
    }

    IEnumerator EnterStayButton(BaseEventData pointData)
    {
        yield return new WaitForSeconds(subT);
        while (stay&&!ClickOver)
        {
            if (ClickOver)
            {
                yield break;
            }
            btn.OnSubmit(pointData);

            yield return new WaitForSeconds(subT);
            if (subT>minSubT)
            {
                subT -= 0.015f;
            }

        }
        yield return null;
    }


    void BtnDoSamllAnimation()
    {
        rect.DOScale(btnSmallVec3, 0.2f);
    }

    public void BtnDoBigAnimation()
    {
        rect.DOScale(btnBigVec3, 0.2f);
    }
}
