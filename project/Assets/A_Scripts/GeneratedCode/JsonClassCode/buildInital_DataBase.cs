//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class BuildInital_DataBase
{
	//通过ID拿数据
	public static BuildInital_Property GetPropertyByID(int id)
	{
		return BuildInital_Data.GetBuildInital_DataByID(id);
	}

	//通过下标拿数据
	public static BuildInital_Property GetPropertyByIndex(int index)
	{
		return BuildInital_Data.GetBuildInital_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return BuildInital_Data.ArrayLenth;
	}

	//获取数组
	public static BuildInital_PropertyBase[] GetArray(int index)
	{
		return BuildInital_Data.DataArray;
	}
}

public class BuildInital_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 备注
	/// </summary>
	public string bz { get; set; }
	/// <summary>
	/// 建筑类型（1摊位，2设备，3装饰）
	/// </summary>
	public int Type { get; set; }
	/// <summary>
	/// 初始时是否显示（0代表没有解锁无法显示在场景里面   -1代表该建筑不在使用）
	/// </summary>
	public int InitLevel { get; set; }
	/// <summary>
	/// （显示在商店的位置）
	/// </summary>
	public int Shop_Index { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] BuildTitle { get; set; }
	/// <summary>
	/// 位置c
	/// </summary>
	public int BuildPos { get; set; }
	/// <summary>
	/// 所属区域  对应区域表的id（摊位的是特殊处理 在实现上所有摊位使用的是一个区) 2楼餐厅
	/// </summary>
	public int AreaIndex { get; set; }
	/// <summary>
	/// 是否是椅子（就是一个id包含多个建筑的类型 1是组合类型 比如椅子 落地灯 0常规）
	/// </summary>
	public int CommboType { get; set; }
}
