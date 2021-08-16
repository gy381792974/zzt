//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Stall_DataBase
{
	//通过ID拿数据
	public static Stall_Property GetPropertyByID(int id)
	{
		return Stall_Data.GetStall_DataByID(id);
	}

	//通过下标拿数据
	public static Stall_Property GetPropertyByIndex(int index)
	{
		return Stall_Data.GetStall_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Stall_Data.ArrayLenth;
	}

	//获取数组
	public static Stall_PropertyBase[] GetArray(int index)
	{
		return Stall_Data.DataArray;
	}
}

public class Stall_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 等级
	/// </summary>
	public int Level { get; set; }
	/// <summary>
	/// 需要解锁的金币数
	/// </summary>
	public int UnlockCoin { get; set; }
	/// <summary>
	/// 食物价格
	/// </summary>
	public int FoodPrice { get; set; }
	/// <summary>
	/// 需要的星星
	/// </summary>
	public int NeedStar { get; set; }
	/// <summary>
	/// 取餐点数
	/// </summary>
	public int CapacityNum { get; set; }
	/// <summary>
	/// 摊位排队人数
	/// </summary>
	public int QueueNum { get; set; }
	/// <summary>
	/// 停留时间
	/// </summary>
	public int StayTime { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] BuildName { get; set; }
	/// <summary>
	/// 介绍
	/// </summary>
	public int[] BuildIntro { get; set; }
	/// <summary>
	/// 说明
	/// </summary>
	public int[] Desc { get; set; }
	/// <summary>
	/// 食物图标
	/// </summary>
	public string foodIcon { get; set; }
	/// <summary>
	/// 标题图片
	/// </summary>
	public string title { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string IconName { get; set; }
	/// <summary>
	/// 动画bundle包名
	/// </summary>
	public string SPBundleName { get; set; }
	/// <summary>
	/// sp动画资源名
	/// </summary>
	public string[] SPAssetName { get; set; }
	/// <summary>
	/// sp动画位置
	/// </summary>
	public float[] SPPos1 { get; set; }
	/// <summary>
	/// sp动画位置
	/// </summary>
	public float[] SPPos2 { get; set; }
	/// <summary>
	/// sp动画位置
	/// </summary>
	public float[] SPPos3 { get; set; }
	/// <summary>
	/// sp动画缩放
	/// </summary>
	public float[] SPScale { get; set; }
	/// <summary>
	/// sp旋转位置1
	/// </summary>
	public int[] SPRota1 { get; set; }
	/// <summary>
	/// sp旋转位置2
	/// </summary>
	public int[] SPRota2 { get; set; }
	/// <summary>
	/// sp旋转位置3
	/// </summary>
	public int[] SPRota3 { get; set; }
}
