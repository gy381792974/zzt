//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class StallLevel_DataBase
{
	//通过ID拿数据
	public static StallLevel_Property GetPropertyByID(int id)
	{
		return StallLevel_Data.GetStallLevel_DataByID(id);
	}

	//通过下标拿数据
	public static StallLevel_Property GetPropertyByIndex(int index)
	{
		return StallLevel_Data.GetStallLevel_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return StallLevel_Data.ArrayLenth;
	}

	//获取数组
	public static StallLevel_PropertyBase[] GetArray(int index)
	{
		return StallLevel_Data.DataArray;
	}
}

public class StallLevel_PropertyBase
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
	/// 备注
	/// </summary>
	public string bz { get; set; }
	/// <summary>
	/// 食物的售价和倍率
	/// </summary>
	public int[] FoodSell { get; set; }
	/// <summary>
	/// 食物初始价格和成长值
	/// </summary>
	public int[] FoodPrice { get; set; }
	/// <summary>
	/// 升级建筑的价格和成长值
	/// </summary>
	public int[] BuildPrice { get; set; }
	/// <summary>
	/// 升级取餐位置的价格/成长值
	/// </summary>
	public int[] TakeMealPrice { get; set; }
	/// <summary>
	/// 升级排队位置的价格/成长值
	/// </summary>
	public int[] QueuePrice { get; set; }
	/// <summary>
	/// 食物最大等级
	/// </summary>
	public int FoodMaxLevel { get; set; }
	/// <summary>
	/// 需要调料瓶的数量
	/// </summary>
	public int BotNeedNum { get; set; }
	/// <summary>
	/// 最大取餐位数量
	/// </summary>
	public int TakeMealMax { get; set; }
	/// <summary>
	/// 最大排队数量
	/// </summary>
	public int MaxQueueNum { get; set; }
}
