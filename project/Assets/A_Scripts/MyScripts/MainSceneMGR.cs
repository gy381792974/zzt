using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;

public class MainSceneMGR : Singleton<MainSceneMGR>
{
    Transform createPos;
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    //创建AI对象 并将AI对象加入队列
    public IEnumerator CreateAiObj()
    {
        CreateInChild();
        //等待两秒再生成
        yield return new WaitForSeconds(2f);
        while (true)
        {
            // AssetMgr.Instance.LoadGameobj("Capsule");
            Transform obj  = AssetMgr.Instance.LoadGameobjFromPool("Capsule");
            obj.SetParent(createPos);
            obj.transform.position = createPos.transform.position;
            Debug.Log(createPos);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private Transform CreateInChild()
    {
        createPos = transform.Find("StartPosition");
        return createPos;
    }



}
