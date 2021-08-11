//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Device_DataBase
{
	//通过ID拿数据
	public static Device_Property GetPropertyByID(int id)
	{
		return Device_Data.GetDevice_DataByID(id);
	}

	//通过下标拿数据
	public static Device_Property GetPropertyByIndex(int index)
	{
		return Device_Data.GetDevice_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Device_Data.ArrayLenth;
	}

	//获取数组
	public static Device_PropertyBase[] GetArray(int index)
	{
		return Device_Data.DataArray;
	}
}

public class Device_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] Build_name { get; set; }
	/// <summary>
	/// 介绍
	/// </summary>
	public int[] Build_intro { get; set; }
}
