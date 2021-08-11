using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//顾客气泡
public class Bubble : MonoBehaviour
{
    [SerializeField] Toggle enjoyImg;
    [SerializeField] Text content;
    [SerializeField] float textMaxWidth = 70;
    [SerializeField] float offsetY = 1.5f;
    public bool isUse = true;
    GameObject imgParent;
    GameObject textParent;
    Transform trans;    //使用该气泡顾客的 transform 
    ContentSizeFitter fitter;
    public Transform Trans { get => trans; }

    public void Init(Transform trans)
    {
        this.trans = trans;
        fitter = content.GetComponent<ContentSizeFitter>();
        imgParent = enjoyImg.transform.parent.gameObject;
        textParent = content.transform.parent.gameObject;
        textParent.SetActive(true);
        imgParent.SetActive(false);
        ScreenToWorldMgr.Instance.AddChild(transform);
        transform.localPosition = ScreenToWorldMgr.Instance.GetUILocalPostion(trans.position + Vector3.up * offsetY);
    }

    private void Update()
    {
        ScreenToWorldMgr.Instance.AddChild(transform);
        transform.localPosition = ScreenToWorldMgr.Instance.GetUILocalPostion(trans.position + Vector3.up * offsetY);
    }


    /// <summary>
    /// 气泡跟随顾客
    /// </summary>
    /// <param name="cusTrans"></param>
    public void BubbleFollowCustomer(Camera uiCamera, Transform cusTrans, float offsetY)
    {
        //世界坐标转化为屏幕坐标
        Vector3 worldPos = new Vector3(cusTrans.position.x, cusTrans.position.y + offsetY, cusTrans.position.z);
        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
        position = uiCamera.ScreenToWorldPoint(position);
        //世界坐标转本地坐标
        Vector2 pos = this.transform.parent.InverseTransformPoint(position);
        transform.localPosition = pos;
    }

    /// <summary>
    /// 顾客满意评价
    /// </summary>
    /// <param name="content"></param>
    public void ShowHappyText(string content)
    {
        StartCoroutine(ShowHappyEvaluate(content));
    }

    /// <summary>
    /// 顾客不满意评价
    /// </summary>
    /// <param name="content"></param>
    public void ShowUnhappyText(string content)
    {
        StartCoroutine(ShowUnHappyEvaluate(content));
    }

    IEnumerator ShowHappyEvaluate(string textContent)
    {
        content.text = textContent;
        yield return null;
        ChangeShowTextMode();
        yield return null;
        SpriteAdaptionText(content);
        yield return new WaitForSeconds(5);
        textParent.SetActive(false);
        imgParent.SetActive(true);
        enjoyImg.isOn = true;
        yield return new WaitForSeconds(5);
        isUse = false;
    }

    IEnumerator ShowUnHappyEvaluate(string textContent)
    {
        textParent.SetActive(true);
        content.text = textContent;
        yield return null;
        ChangeShowTextMode();
        yield return null;
        SpriteAdaptionText(content);
        yield return new WaitForSeconds(5);
        textParent.SetActive(false);
        imgParent.SetActive(true);
        enjoyImg.isOn = false;
        yield return new WaitForSeconds(5);
        imgParent.SetActive(false);
        StartCoroutine(ShowUnHappyEvaluate(textContent));
    }

    RectTransform contentRT;
    float _width;
    private void SpriteAdaptionText(Text content)
    {
        float width = contentRT.sizeDelta.x;
        float height = contentRT.sizeDelta.y;
        RectTransform rtp = (RectTransform)content.rectTransform.parent;
        rtp.sizeDelta = new Vector2(width + 30, height + 35);
    }

    private void ChangeShowTextMode()
    {
        contentRT = content.rectTransform;
        _width = contentRT.sizeDelta.x;
        if (_width < textMaxWidth)
        {
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            Vector2 size = contentRT.sizeDelta;
            size.x = textMaxWidth;
            contentRT.sizeDelta = size;
        }
    }


}
