using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
using EazyGF;
using Spine.Unity;
using Spine;
using UnityEngine.UI;

public interface UIDataBase
{

}

public enum UIShowMode
{
    DoNothing,
    HideAll,
    HideAllExceptTop
}

/// <summary>
/// UI动画类型
/// </summary>
public enum UIAnimType
{
    DoNothing,
    DoScale,
    DoHoriz,
    DoVertl,
    DoFade,
    DoScaleAndFade,
    Animation
}


public class UIBase : MonoBehaviour
{
    public UIShowMode showMode = UIShowMode.DoNothing;//当前UI显示出来后，是否隐藏其他UI
    public UILevel uiLevel = UILevel.View;//当前UI是否保持存在.
    /// <summary>
    /// 动画类型
    /// </summary>
    public UIAnimType animType = UIAnimType.DoNothing;//  预加载时的面板 动画类型有时会被改动 ，暂时未知原因

    /// <summary>
    /// 执行动画的区域
    /// </summary>
    public Transform DoAnimContent = null;

    /// <summary>
    /// UI背景图
    /// </summary>
    public Transform UIBackGround = null;

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public Button CloseBtn = null;

    float scaleSize = 1.1f;
    float animationLength = 0.3f;
    /// <summary>
    /// UI展示时动画队列
    /// </summary>
    private Sequence showSeq;
    /// <summary>
    /// UI匿名时动画队列
    /// </summary>
    private Sequence hideSeq;

    private Sequence otherSeq;



    /// <summary>
    /// 平行增量
    /// </summary>
    int IncreX = Screen.width;
    /// <summary>
    /// 垂直增量
    /// </summary>
    int IncreY = Screen.height;

    /// <summary>
    /// 平行起始点坐标
    /// </summary>
    Vector3 FormHorPos;
    Vector3 ToHorPos;

    /// <summary>
    /// 垂直起始点坐标
    /// </summary>
    Vector3 FromVerPos;
    Vector3 ToVerPos;

    /// <summary>
    /// 动画执行时长
    /// </summary>
    private float AnimPlayTime = 0.2f;

    /// <summary>
    /// UI完全关闭后的回调
    /// </summary>
    public Action HideCompleteCallBack = null;

    private void OnShowScaleTween()
    {

        DoAnimContent.localScale = Vector3.zero;

        showSeq.Join(DoAnimContent.DOScale(Vector3.one, AnimPlayTime));

    }
    private void OnHideScaleTween()
    {

        hideSeq.Join(DoAnimContent.DOScale(Vector3.zero, AnimPlayTime));

    }

    private void OnShowHorizTween()
    {
        DoAnimContent.localPosition = FormHorPos;
        showSeq.Join(DoAnimContent.DOLocalMove(Vector3.zero, AnimPlayTime));

    }

    private void OnHideHorizTween()
    {

        hideSeq.Join(DoAnimContent.DOLocalMove(ToHorPos, AnimPlayTime)); ;

    }


    private void OnShowVertlTween()
    {
        DoAnimContent.localPosition = FromVerPos;
        showSeq.Join(DoAnimContent.DOLocalMove(Vector3.zero, AnimPlayTime));

    }

    private void OnHideVertlTween()
    {

        hideSeq.Join(DoAnimContent.DOLocalMove(ToVerPos, AnimPlayTime)); ;

    }

    private void OnShowFadeTween()
    {
        if (DoAnimContent.GetComponent<CanvasGroup>() == null)
        {
            DoAnimContent.gameObject.AddComponent<CanvasGroup>();
        }

        DoAnimContent.GetComponent<CanvasGroup>().alpha = 0;
        showSeq.Join(DoAnimContent.GetComponent<CanvasGroup>().DOFade(1, AnimPlayTime));

    }

    private void OnHideFadeTween()
    {
        hideSeq.Join(DoAnimContent.GetComponent<CanvasGroup>().DOFade(0, AnimPlayTime));

    }

    private void OnPlayStartAnimation()
    {
        Transform trans = DoAnimContent;
        trans.localScale = Vector3.zero;
        showSeq.Append(trans.DOScale(Vector3.one * scaleSize, animationLength).SetEase(Ease.InOutBack));
        showSeq.Join(trans.GetChild(0).DOMove(new Vector3(-3, 3, 0) / 320, animationLength));
        showSeq.Append(trans.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack));
        showSeq.Join(trans.GetChild(0).DOMove(new Vector3(0, 0, 0) / 320, 0.2f));
    }

    private void OnPlayBackAnimation()
    {
        Transform trans = DoAnimContent;
        trans.localScale = Vector3.one;
        hideSeq.Append(trans.DOScale(Vector3.one * scaleSize, 0.15f).SetEase(Ease.InOutBounce));
        hideSeq.Join(trans.GetChild(0).DOMove(new Vector3(0, 0, 0) / 320, 0.15f));
        hideSeq.Append(trans.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
        hideSeq.Join(trans.GetChild(0).DOMove(new Vector3(3, -3, 0) / 320, 0.25f));
    }
    /// <summary>
    /// 按钮点击时的UI动画
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="scaleSize"></param>
    /// <param name="time"></param>
    protected void BtnClickAnimation(Transform trans, TweenCallback complete = null, float scaleSize = 0.8f, float time = 0.2f)
    {
        //MusicMgr.Instance.PlayMusicEff("g_btn_gen");
        otherSeq = DOTween.Sequence();
        otherSeq.Join(trans.DOScale(Vector3.one * scaleSize, time).SetEase(Ease.InOutBack));
        otherSeq.Append(trans.DOScale(Vector3.one, time).SetEase(Ease.OutBack));
        if (animType != UIAnimType.Animation)
        {
            otherSeq.onComplete = complete;
        }
    }
    protected virtual void Awake()
    {
        //animType = UIAnimType.DoNothing;
        //Debug.Log($"{gameObject}=>{animType}");

        FormHorPos = new Vector3(-IncreX, 0, 0);
        FromVerPos = new Vector3(0, -IncreY, 0);

        ToHorPos = new Vector3(IncreX, 0, 0);
        ToVerPos = new Vector3(0, IncreY, 0);

        DoAnimContent = DoAnimContent == null ? transform : DoAnimContent;


        //如果该UI使用背景动画，需要在背景UI加上canvasGrou组件
        if (UIBackGround != null)
        {
            if (UIBackGround.GetComponent<CanvasGroup>() == null)
            {

                UIBackGround.gameObject.AddComponent<CanvasGroup>();
            }
        }

        if (CloseBtn != null)
        {
            CloseBtn.onClick.AddListener(() => {

                //UIMgr.HideUI<this.GetType()>();

                UIMgr.HideUI(this.GetType().Name);

            });
        } 
    }

    //虚函数，子类来实现该方法
    protected virtual void OnInit()//1：用来初始化一些数据和UI组件
    {

    }
    protected virtual void OnShow(UIDataBase uiDataBase = null)//2：显示UI时候调用的方法，如果你打开该界面需要操作，就写在这里面
    {

    }
    protected virtual void OnHide()//3：隐藏UI时候调用的方法，隐藏该界面需要操作什么，就写里面
    {

    }

    protected virtual void OnPreLoad()//4：预加载后处理的事情
    {

    }

    /// <summary>
    /// 该方法只会执行一次，在Show之前执行
    /// </summary>
    public void Init()
    {
        OnInit();//虚方法，子类实现
    }

    /// <summary>
    /// 显示UI的方法
    /// </summary>
    public void Show(UIDataBase uiDataBase = null)
    {

        showSeq = DOTween.Sequence();

        if (UIBackGround != null)
        {

            showSeq.Join(UIBackGround.GetComponent<CanvasGroup>().DOFade(1, AnimPlayTime));

        }


        switch (animType)
        {
            case UIAnimType.DoNothing:
                break;
            case UIAnimType.DoScale:
                OnShowScaleTween();
                break;
            case UIAnimType.DoHoriz:
                OnShowHorizTween();
                break;
            case UIAnimType.DoVertl:
                OnShowVertlTween();
                break;
            case UIAnimType.DoFade:
                OnShowFadeTween();
                break;
            case UIAnimType.DoScaleAndFade:
                break;
            case UIAnimType.Animation:
                OnPlayStartAnimation();
                break;
            default:
                break;
        }
        gameObject.SetActive(true);//即将显示的UI设置为显示     
        showSeq.Play().OnComplete(() =>
        {
            //Debug.Log("展示动画播放完成");
        });

        OnShow(uiDataBase);//虚方法，子类实现
    }

    /// <summary>
    /// 预加载后的方法
    /// </summary>
    public void PreLoad()
    {
        gameObject.SetActive(false);
        OnPreLoad();
    }

    /// <summary>
    /// 隐藏UI的方法
    /// </summary>
    /// <param name="action">隐藏UI之后需要做的事情</param>
    public void Hide(Action action = null)
    {
        hideSeq = DOTween.Sequence();
        if (UIBackGround != null)
        {

            hideSeq.Join(UIBackGround.GetComponent<CanvasGroup>().DOFade(0, AnimPlayTime));

        }
        switch (animType)
        {
            case UIAnimType.DoNothing:
                break;
            case UIAnimType.DoScale:
                OnHideScaleTween();
                break;
            case UIAnimType.DoHoriz:
                OnHideHorizTween();
                break;
            case UIAnimType.DoVertl:
                OnHideVertlTween();
                break;
            case UIAnimType.DoFade:
                OnHideFadeTween();
                break;
            case UIAnimType.DoScaleAndFade:
                break;
            case UIAnimType.Animation:
                OnPlayBackAnimation();
                break;
            default:
                break;
        }

        //MusicMgr.Instance.PlayMusicEff("g_win_close");

        OnHide();
        hideSeq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);//直接隐藏
                                        // Debug.Log("隐藏动画播放完成");            
            action?.Invoke();

            HideCompleteCallBack?.Invoke();
            HideCompleteCallBack = null;
        });

    }

    //UI动画相关




}
