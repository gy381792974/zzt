//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Chair_DataBase
{
	//通过ID拿数据
	public static Chair_Property GetPropertyByID(int id)
	{
		return Chair_Data.GetChair_DataByID(id);
	}

	//通过下标拿数据
	public static Chair_Property GetPropertyByIndex(int index)
	{
		return Chair_Data.GetChair_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Chair_Data.ArrayLenth;
	}

	//获取数组
	public static Chair_PropertyBase[] GetArray(int index)
	{
		return Chair_Data.DataArray;
	}
}

public class Chair_PropertyBase
{
	/// <summary>
	/// id
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 等级
	/// </summary>
	public int level { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] name { get; set; }
	/// <summary>
	/// 描述
	/// </summary>
	public int[] desc { get; set; }
	/// <summary>
	/// 建造初始价格_升级倍率
	/// </summary>
	public int[] buildPrice { get; set; }
	/// <summary>
	/// 升级初始价格_升级倍率
	/// </summary>
	public int[] upgradePrice { get; set; }
	/// <summary>
	/// 图标
	/// </summary>
	public string icon { get; set; }
	/// <summary>
	/// 当前阶段最大等级
	/// </summary>
	public int maxLevel { get; set; }
	/// <summary>
	/// 区域类型
	/// </summary>
	public int type { get; set; }
}
