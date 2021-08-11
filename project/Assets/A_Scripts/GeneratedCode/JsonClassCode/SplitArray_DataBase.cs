//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class SplitArray_DataBase
{
	//通过ID拿数据
	public static SplitArray_Property GetPropertyByID(int id)
	{
		return SplitArray_Data.GetSplitArray_DataByID(id);
	}

	//通过下标拿数据
	public static SplitArray_Property GetPropertyByIndex(int index)
	{
		return SplitArray_Data.GetSplitArray_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return SplitArray_Data.ArrayLenth;
	}

	//获取数组
	public static SplitArray_PropertyBase[] GetArray(int index)
	{
		return SplitArray_Data.DataArray;
	}
}

public class SplitArray_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 字符串1
	/// </summary>
	public string TestStr_1 { get; set; }
	/// <summary>
	/// 字符串2
	/// </summary>
	public string TestStr2 { get; set; }
	/// <summary>
	/// 整数Int
	/// </summary>
	public int TestInt { get; set; }
	/// <summary>
	/// 整数Long
	/// </summary>
	public long TestLong { get; set; }
	/// <summary>
	/// 单精度浮点
	/// </summary>
	public float TestFloat { get; set; }
	/// <summary>
	/// 字符串数组1
	/// </summary>
	public string[] TestStr1Arr { get; set; }
	/// <summary>
	/// 大数字
	/// </summary>
	public BigInteger TestBigInter { get; set; }
	/// <summary>
	/// 双精度浮点
	/// </summary>
	public double TestDouble { get; set; }
	/// <summary>
	/// 字符串数组2
	/// </summary>
	public string[] TestStr1Arr2 { get; set; }
	/// <summary>
	/// 联合数组测试1
	/// </summary>
	public string[] TestMArray { get; set; }
}
