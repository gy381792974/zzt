//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CubeConfig_DataBase
{
	//通过ID拿数据
	public static CubeConfig_Property GetPropertyByID(int id)
	{
		return CubeConfig_Data.GetCubeConfig_DataByID(id);
	}

	//通过下标拿数据
	public static CubeConfig_Property GetPropertyByIndex(int index)
	{
		return CubeConfig_Data.GetCubeConfig_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CubeConfig_Data.ArrayLenth;
	}

	//获取数组
	public static CubeConfig_PropertyBase[] GetArray(int index)
	{
		return CubeConfig_Data.DataArray;
	}
}

public class CubeConfig_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// ab包名
	/// </summary>
	public int[] BoxIds { get; set; }
}
