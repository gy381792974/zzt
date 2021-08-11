//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Global_DataBase
{
	//通过ID拿数据
	public static Global_Property GetPropertyByID(int id)
	{
		return Global_Data.GetGlobal_DataByID(id);
	}

	//通过下标拿数据
	public static Global_Property GetPropertyByIndex(int index)
	{
		return Global_Data.GetGlobal_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Global_Data.ArrayLenth;
	}

	//获取数组
	public static Global_PropertyBase[] GetArray(int index)
	{
		return Global_Data.DataArray;
	}
}

public class Global_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 结果
	/// </summary>
	public int[] RInt { get; set; }
}
