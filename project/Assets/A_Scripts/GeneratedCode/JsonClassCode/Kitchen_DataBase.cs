//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Kitchen_DataBase
{
	//通过ID拿数据
	public static Kitchen_Property GetPropertyByID(int id)
	{
		return Kitchen_Data.GetKitchen_DataByID(id);
	}

	//通过下标拿数据
	public static Kitchen_Property GetPropertyByIndex(int index)
	{
		return Kitchen_Data.GetKitchen_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Kitchen_Data.ArrayLenth;
	}

	//获取数组
	public static Kitchen_PropertyBase[] GetArray(int index)
	{
		return Kitchen_Data.DataArray;
	}
}

public class Kitchen_PropertyBase
{
	/// <summary>
	/// 厨具Id
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 厨具等级
	/// </summary>
	public int level { get; set; }
	/// <summary>
	/// 厨具名称
	/// </summary>
	public int[] Name { get; set; }
	/// <summary>
	/// 厨具介绍
	/// </summary>
	public int[] Intro { get; set; }
	/// <summary>
	/// 建造初始价格_增长倍率
	/// </summary>
	public int[] buildPrice { get; set; }
	/// <summary>
	/// 每阶段升级初始价格_增长倍率
	/// </summary>
	public int[] upgradePrice { get; set; }
	/// <summary>
	/// 当前阶段升级奖励
	/// </summary>
	public int reward { get; set; }
	/// <summary>
	/// 图标Icon
	/// </summary>
	public string Icon { get; set; }
	/// <summary>
	/// 当前阶段最大等级
	/// </summary>
	public int maxLevel { get; set; }
}
