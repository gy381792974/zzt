//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CusFashion_DataBase
{
	//通过ID拿数据
	public static CusFashion_Property GetPropertyByID(int id)
	{
		return CusFashion_Data.GetCusFashion_DataByID(id);
	}

	//通过下标拿数据
	public static CusFashion_Property GetPropertyByIndex(int index)
	{
		return CusFashion_Data.GetCusFashion_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CusFashion_Data.ArrayLenth;
	}

	//获取数组
	public static CusFashion_PropertyBase[] GetArray(int index)
	{
		return CusFashion_Data.DataArray;
	}
}

public class CusFashion_PropertyBase
{
	/// <summary>
	/// ID 对应普通顾客表的id
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 摊位id 对应建筑初始化表的id
	/// </summary>
	public int StallId { get; set; }
	/// <summary>
	/// 去的次数
	/// </summary>
	public int GTNum { get; set; }
	/// <summary>
	/// 变形的Id（只有一种变形就填1）
	/// </summary>
	public int FashionId { get; set; }
}
