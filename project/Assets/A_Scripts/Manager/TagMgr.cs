using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 如果需要增加Tag,请在StartTag之后，EndTag之前加
/// </summary>
public enum TagType
{
    StartTag,//必须在之后加
    CashCheck,
    ChangeCloth,
    ClassRoom,
    LiveRoom,
    OfficeRoom,
    ShopRoom,
    CanTingRoom,
    WaitOpen,
    Reception,
    Admission,//招生
    Promote,
    Fountain,
    WishHouse,
    TechTree,
    ReceiveHolyWaterUI,
    Dementor,//摄魂怪
    ShopRoomGoldUI,//商店收营员头上的金币UI
    Reporter,//记者
    FlyHorse,//飞马
    MagicTreeUI,
    EndTag,//必须在之前加
}

public class TagMgr : Singleton<TagMgr>
{
    public string GetTag(TagType tagType)
    {
        return tagType.ToString();
    }

    /// <summary>
    /// 将所有的Tag装进数组中
    /// </summary>
    /// <returns></returns>
    public string[] ConverTagToArray()
    {
        int startIndex = (int)TagType.StartTag;
        int endIndex= (int)TagType.EndTag;
        int arrayLenth = endIndex - startIndex+1;
        string []targetArray=new string[arrayLenth];
        
        for (int i = startIndex ; i < arrayLenth; i++)
        {
            targetArray[i]= GetTag((TagType) i);
        }

        return targetArray;
    }
}
