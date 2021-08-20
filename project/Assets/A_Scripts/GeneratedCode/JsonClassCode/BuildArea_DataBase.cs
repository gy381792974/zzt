//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class BuildArea_DataBase
{
	//通过ID拿数据
	public static BuildArea_Property GetPropertyByID(int id)
	{
		return BuildArea_Data.GetBuildArea_DataByID(id);
	}

	//通过下标拿数据
	public static BuildArea_Property GetPropertyByIndex(int index)
	{
		return BuildArea_Data.GetBuildArea_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return BuildArea_Data.ArrayLenth;
	}

	//获取数组
	public static BuildArea_PropertyBase[] GetArray(int index)
	{
		return BuildArea_Data.DataArray;
	}
}

public class BuildArea_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 备注
	/// </summary>
	public string BZ { get; set; }
	/// <summary>
	/// 是否解锁 1解锁 
	/// </summary>
	public int IsUnLock { get; set; }
	/// <summary>
	/// 遮挡区 是使用BuldMaskArea的id -1没有遮挡
	/// </summary>
	public int MaskAreaId { get; set; }
}
