//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Restaurant_DataBase
{
	//通过下标拿数据
	public static Restaurant_Property GetPropertyByIndex(int index)
	{
		return Restaurant_Data.GetRestaurant_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Restaurant_Data.ArrayLenth;
	}

	//获取数组
	public static Restaurant_PropertyBase[] GetArray(int index)
	{
		return Restaurant_Data.DataArray;
	}
}

public class Restaurant_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int restaurant_ID { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] Build_name { get; set; }
	/// <summary>
	/// 介绍
	/// </summary>
	public int[] Build_intro { get; set; }
}
