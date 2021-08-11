//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class BuildMaskArea_DataBase
{
	//通过ID拿数据
	public static BuildMaskArea_Property GetPropertyByID(int id)
	{
		return BuildMaskArea_Data.GetBuildMaskArea_DataByID(id);
	}

	//通过下标拿数据
	public static BuildMaskArea_Property GetPropertyByIndex(int index)
	{
		return BuildMaskArea_Data.GetBuildMaskArea_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return BuildMaskArea_Data.ArrayLenth;
	}

	//获取数组
	public static BuildMaskArea_PropertyBase[] GetArray(int index)
	{
		return BuildMaskArea_Data.DataArray;
	}
}

public class BuildMaskArea_PropertyBase
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
	/// 说明
	/// </summary>
	public int[] Info { get; set; }
	/// <summary>
	/// 解锁花费
	/// </summary>
	public int costCoin { get; set; }
}
