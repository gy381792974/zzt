using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMouse : MonoBehaviour
{

    duiduipeng ex;    //获取实例,用于获取listdes；
    MeshRenderer mr;
    MeshRenderer mr2;

    // Use this for initialization
    void Start()
    {
        mr = this.gameObject.GetComponent<MeshRenderer>();  //获取组件
        ex = GameObject.Find("Plane").GetComponent<duiduipeng>();
    }


    void OnMouseDown()
    {
        mr.material.color = Color.red;          //点击后的颜色为设置为红色
        ex.listdes.Add(this.gameObject);
        if (ex.listdes.Count == 2)
        {
            if (ex.listdes[0].name == ex.listdes[1].name)
            {
                Destroy(ex.listdes[0], 0.3f);    //0.3秒后销毁物体
                Destroy(ex.listdes[1], 0.3f);

                ex.listdes.Clear();             //保险起见，清空集合
            }
            else   //如果两次点击的不是相同的物体
            {
                mr.material.color = Color.white;   //将当前物体变成默认颜色，白色
                mr2 = ex.listdes[0].GetComponent<MeshRenderer>();
                mr2.material.color = Color.white;  //之前选中的物体变回原来颜色

                ex.listdes.Clear();
            }
        }
    }
}