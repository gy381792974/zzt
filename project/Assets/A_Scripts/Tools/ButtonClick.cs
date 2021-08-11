using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 按钮点击扩展
/// </summary>
public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 是否进入长按
    /// </summary>
    [SerializeField]private bool isPressed=false;

    /// <summary>
    /// 时间间隔
    /// </summary>
    [SerializeField] private float TimeInterval = 0.1f;

    /// <summary>
    /// 时间增量
    /// </summary>
     private float TimeScale = 0;

    /// <summary>
    /// 原始缩放比例
    /// </summary>
    [SerializeField]private Vector3 TranScalingOriginal = new Vector3(1,1,1);
    /// <summary>
    /// 期望缩放比例
    /// </summary>
    [SerializeField]private Vector3 TranScalingExpect = new Vector3(0.85f, 0.85f, 1);

    /// <summary>
    /// 按钮绑定的点击事件
    /// </summary>
    private Action onBtnClick;

    /// <summary>
    /// 宿主身上的Button组件
    /// </summary>
    private Button button;

    private void Start()
    {
        button = transform.GetComponent<Button>();
        onBtnClick =delegate {
            if (button.interactable) { 
                button.onClick.Invoke();
            }         
        } ;

    }

    private void OnEnable()
    {
        isPressed = false;
        TimeScale = 0;
        transform.localScale = TranScalingOriginal;
    }

    /// <summary>
    /// 按下
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) { return; }
        isPressed = true;
        //第一次点击给个0.5秒的延迟
        TimeScale = -0.3f;
        transform.localScale = TranScalingExpect;
    }

    /// <summary>
    /// 抬起
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        TimeScale = 0;
        transform.localScale = TranScalingOriginal;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isPressed) {
            if (!button.interactable) {
                isPressed = false;
                TimeScale = 0;
                transform.localScale = TranScalingOriginal;
                return;
            }
            TimeScale += Time.deltaTime;
            if (TimeScale>=TimeInterval) {
                onBtnClick.Invoke();
                TimeScale = 0;
            }
        
        }
    }

    /// <summary>
    /// 点击动画
    /// </summary>
    private void PlayClickAnim() { 
    
    
    }
}

