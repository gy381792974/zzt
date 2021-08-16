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
	/// 厨房设施ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 厨房设施图标
	/// </summary>
	public string Icon { get; set; }
}
