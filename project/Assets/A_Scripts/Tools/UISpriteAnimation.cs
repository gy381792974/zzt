using System;
using UnityEngine;
using UnityEngine.UI;

//该脚本主要用于播放序列帧
public class UISpriteAnimation : MonoBehaviour
{
    public Sprite[] SpriteArray;
    public float m_sep = 0.05f;
    public string abName;
    private SpriteRenderer m_spriteRender;
    private float m_delta = 0;
    private int m_curFrame = 0;
    public bool isStatr = false;
    public bool isLoop = false;
    public Action _CallBackAction=null;
    public int FrameCount
    {
        get
        {
            return SpriteArray.Length;
        }
    }

    private void OnEnable()
    {
        PlayAnitiom();
    }
    
    private void PlayAnitiom(int frame = 0)
    {
        if (SpriteArray == null || SpriteArray.Length <= 0)
        {
            SpriteArray = AssetMgr.Instance.LoadAllAssets<Sprite>(abName);
        }
        if (m_spriteRender == null)
        {
            m_spriteRender = this.GetComponent<SpriteRenderer>();
        }
        if (frame >= FrameCount)
        {
            m_curFrame= frame = 0;
        }
        m_spriteRender.sprite = SpriteArray[frame];
        //m_Image.SetNativeSize();
        isStatr = true;
    }

    void Update()
    {
        if (!isStatr)
        {
            return;
        }
        m_delta += Time.deltaTime;
        if (m_delta > m_sep)
        {
            m_delta = 0;
            m_curFrame++;
            if (isLoop)
            {
                PlayAnitiom(m_curFrame);
            }
            else
            {
                if (m_curFrame == FrameCount)
                {
                    isStatr = false;
                    OnAnimationComplete();
                    return;
                }
                PlayAnitiom(m_curFrame);
            }
        }
    }

    private void OnAnimationComplete()
    {
        if (_CallBackAction != null)
        {
            _CallBackAction();
        }
        this.gameObject.SetActive(false);
    }
    

}
