using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWalkLight : Singleton<UIWalkLight>
{
    [SerializeField] Material material;
    [SerializeField] float interval;
    [SerializeField] float speed;
    Vector2 mainOffset = new Vector2(1, 0);
    float timer = 0;
    public void Init()
    {



    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > interval / 100)
        {
            timer = 0;
            mainOffset.x -= speed;
            material.SetTextureOffset("_MainTex", mainOffset);
            if (mainOffset.x <= -1)
            {
                mainOffset.x = 1;
            }
        }
    }


    public void StartWalkLight(RectTransform rt, float length)
    {
        RectTransform rtp = rt.parent as RectTransform;
        rt.sizeDelta = new Vector2(rtp.sizeDelta.x * length, rt.sizeDelta.y);
        //StartCoroutine(WalkLight());
    }

    //如何扫光
    private IEnumerator WalkLight()
    {
        while (true)
        {

            if (mainOffset.x <= -1)
            {
                mainOffset.x = 1;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //private void OnDisable()
    //{
    //    ResetPosition();
    //}

    private void ResetPosition()
    {
        material.SetTextureOffset("_MainTex", new Vector2(0, 0));
    }

}
