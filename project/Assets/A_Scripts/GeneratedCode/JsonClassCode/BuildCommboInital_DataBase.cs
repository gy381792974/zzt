//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class BuildCommboInital_DataBase
{
	//通过ID拿数据
	public static BuildCommboInital_Property GetPropertyByID(int id)
	{
		return BuildCommboInital_Data.GetBuildCommboInital_DataByID(id);
	}

	//通过下标拿数据
	public static BuildCommboInital_Property GetPropertyByIndex(int index)
	{
		return BuildCommboInital_Data.GetBuildCommboInital_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return BuildCommboInital_Data.ArrayLenth;
	}

	//获取数组
	public static BuildCommboInital_PropertyBase[] GetArray(int index)
	{
		return BuildCommboInital_Data.DataArray;
	}
}

public class BuildCommboInital_PropertyBase
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
	/// 初始时是否显示（0代表没有解锁无法显示在场景里面   -1代表该建筑不在使用）
	/// </summary>
	public int[] InitLevel { get; set; }
}
