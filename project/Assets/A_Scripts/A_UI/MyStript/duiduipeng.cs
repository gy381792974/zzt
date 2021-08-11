using System.Collections.Generic;
using UnityEngine;

public class duiduipeng : MonoBehaviour
{

    public List<Vector3> listpos;     //存放物体出现的指点位置 
    public List<GameObject> listgo;   //存放创建物体
 
    public List<GameObject> listdes = new List<GameObject>();   //存放选中物体

    private GameObject go1;              //创建出的物体
    private GameObject go2;

     Sprite[,]arr=new Sprite[9,10];
    void Start()
    {
        Createpos();
        Create();
        Position();
    }

    #region  创建物体，并随机赋值位置
    void Createpos()
    {
        //初始化，位置列表
        for (int i = 0; i <= 6; i += 2)
        {
            for (int j = 0; j <= 6; j += 2)
            {
                listpos.Add(new Vector3(i, 0, j));
            }
        }
    }

    void Create()
    {
        for (int i = 0; i < 8; i++)         //生成不同位置的两个相同物体
        {
            int atype = Random.Range(0, 4);  //随机出现物体类型 

            go1 = GameObject.CreatePrimitive((PrimitiveType)atype);  //创建物体
            go2 = GameObject.CreatePrimitive((PrimitiveType)atype);

            go1.AddComponent<DestroyMouse>();    //给生成物体guazDestoryMouse代码
            go2.AddComponent<DestroyMouse>();

            listgo.Add(go1);
            listgo.Add(go2);                //将生成物体存入列表
            
            go1.name = go2.name = "" + (PrimitiveType)atype + i;//指定生成物体名称一致

        }
    }

    void Position()
    {
        for (int i = 0; i < 16; i++)
        {
            int a = Random.Range(0, listpos.Count);      //从0~list2的长度中随机生成索引值

            listgo[i].transform.position = listpos[a]; //将得到的随机位置赋值给游戏对象

            listpos.Remove(listpos[a]);//移除已经生成物体的位置
        }
    }
    #endregion
}
//然后再创建如下代码即可（注意代码中名称不能打错，和上示代码对应）



