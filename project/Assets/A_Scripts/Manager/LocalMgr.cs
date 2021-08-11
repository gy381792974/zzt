using System.Collections;
using System.Collections.Generic;
using EazyGF;
using UnityEngine;

public class LocalMgr : Singleton<LocalMgr>
{
    [Header("无需手动赋值")]
    [SerializeField] private Transform[] classRoomChairInsParentArray;//实例化出来的椅子的父物体
    //宿舍部分
    [SerializeField] private Transform[] liveRoomChairInsParentArray;//实例化出来的椅子的父物体
    //商铺部分
    [SerializeField] private Transform[] shopRoomChairInsParentArray;//实例化出来的椅子的父物体
    //餐厅部分
    [SerializeField] private Transform canTingRoomChairInsParent;//实例化出来的椅子的父物体
    //办公室部分
    [SerializeField] private Transform[] officeRoomChairInsParentArray;//实例化出来的椅子的父物体
    //更衣室部分
    [SerializeField] private Transform[] changeClothChairInsParentArray;//实例化出来的椅子的父物体

    [SerializeField] private Vector3 FountainPos;
    [SerializeField] private Vector3 WishHousePos;
    [SerializeField] private Vector3[] TechTreePos;
    [SerializeField] private Vector3 ReporterPos;

    /// <summary>
    /// 宿舍满员图标
    /// </summary>
    public GameObject BedFullShowImg;

    /// <summary>
    /// 宿舍满员图标
    /// </summary>
    public GameObject LodkerFullShowImg_1;
    public GameObject LodkerFullShowImg_2;
    public GameObject LodkerFullShowImg_3;

    public void Init()
    {
        InitClassRoomChairtArray();
        InitLiveRoomChairtArray();
        InitShopRoomChairtArray();
        InitCanTingRoomChairtArray();
        InitOfficeRoomChairtArray();
        InitChangeClothRoomChairtArray();
        InitFountainPos();
    }

    private void InitFountainPos()
    {
        FountainPos = transform.Find("Fountain").position;
        WishHousePos = transform.Find("WishHouse").position;
        ReporterPos = transform.Find("ReporterPos").position;
        TechTreePos =new Vector3[3];
        Transform techParentTrans= transform.Find("TechTree");
        for (int i = 0; i < techParentTrans.childCount; i++)
        {
            TechTreePos[i] = techParentTrans.GetChild(i).position;
        }
    }

    //初始化场景中的父节点
    private void InitClassRoomChairtArray()
    {
        //实例化出来放在的父节点部分
        Transform roomInsParent = transform.Find("ClassRoom");
        
        int roomInsChildCount = roomInsParent.childCount;
        classRoomChairInsParentArray=new Transform[roomInsChildCount];
        for (int i = 0; i < roomInsChildCount; i++)
        {
            classRoomChairInsParentArray[i] = roomInsParent.GetChild(i);
        }
    }
    private void InitOfficeRoomChairtArray()
    {
        //实例化出来放在的父节点部分
        Transform roomInsParent = transform.Find("OfficeRoom");

        int roomInsChildCount = roomInsParent.childCount;
        officeRoomChairInsParentArray = new Transform[roomInsChildCount];
        for (int i = 0; i < roomInsChildCount; i++)
        {
            officeRoomChairInsParentArray[i] = roomInsParent.GetChild(i);
        }
    }
    private void InitChangeClothRoomChairtArray()
    {
        //实例化出来放在的父节点部分
        Transform roomInsParent = transform.Find("ChangeCloth");

        int roomInsChildCount = roomInsParent.childCount;
        changeClothChairInsParentArray = new Transform[roomInsChildCount];
        for (int i = 0; i < roomInsChildCount; i++)
        {
            changeClothChairInsParentArray[i] = roomInsParent.GetChild(i);
        }
    }

    private void InitLiveRoomChairtArray()
    {
        //实例化出来放在的父节点部分
        Transform roomInsParent = transform.Find("LiveRoom");

        int roomInsChildCount = roomInsParent.childCount;
        liveRoomChairInsParentArray = new Transform[roomInsChildCount];
        for (int i = 0; i < roomInsChildCount; i++)
        {
            liveRoomChairInsParentArray[i] = roomInsParent.GetChild(i);
        }
    }
    private void InitShopRoomChairtArray()
    {
        //实例化出来放在的父节点部分
        Transform roomInsParent = transform.Find("ShopRoom");

        int roomInsChildCount = roomInsParent.childCount;
        shopRoomChairInsParentArray = new Transform[roomInsChildCount];
        for (int i = 0; i < roomInsChildCount; i++)
        {
            shopRoomChairInsParentArray[i] = roomInsParent.GetChild(i);
        }
    }

    private void InitCanTingRoomChairtArray()
    {
        canTingRoomChairInsParent = transform.Find("CanTingRoom");
    }

    /// <summary>
    /// 获取教室实例化出来需要的父节点
    /// </summary>
    /// <param name="buildIndex">建造下标</param>
    /// <param name="childIndex">子节点下标，0表示房间的父节点，1表示椅子的父节点</param>
    /// <returns></returns>
    public Transform GetClassRoomPrefabParentTrans(int buildIndex,int childIndex)
    {
        return classRoomChairInsParentArray[buildIndex - 1].GetChild(childIndex);
    }


    /// <summary>
    /// 获取办公室实例化出来需要的父节点
    /// </summary>
    /// <param name="officeRoomID"></param>
    /// <param name="childIndex"></param>
    /// <returns></returns>
    public Transform GetOfficeRoomPrefabParentTrans(int officeRoomID, int childIndex)
    {
        return officeRoomChairInsParentArray[officeRoomID-1].GetChild(childIndex);
    }


    /// <summary>
    /// 获取宿舍实例化出来需要的父节点
    /// </summary>
    /// <param name="liveRoomID"></param>
    /// <param name="childIndex"></param>
    /// <returns></returns>
    public Transform GetLiveRoomPrefabParentTrans(int liveRoomID, int childIndex)
    {
        return liveRoomChairInsParentArray[liveRoomID - 1].GetChild(childIndex);
    }

    public Transform GetShopRoomPrefabParentTrans(int buildIndex, int childIndex)
    {
        return shopRoomChairInsParentArray[buildIndex - 1].GetChild(childIndex);
    }

    public Transform GetCanTingRoomPrefabParentTrans( int childIndex)
    {
        return canTingRoomChairInsParent.GetChild(childIndex);
    }

    public Vector3 GetFountainPos()
    {
        return FountainPos;
    }

    public Vector3 GetWishHousePos()
    {
        return WishHousePos;
    }
    
    public Vector3 GetTechTreePos(int techTreeLev)
    {
        return TechTreePos[techTreeLev - 1];
    }

    public Vector3 GetReporterPos()
    {
        return ReporterPos;
    }

    public Transform GetChangeClothTrans(int index)
    {
        return changeClothChairInsParentArray[index];
    }
}
