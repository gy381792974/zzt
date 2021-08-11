//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class BuildBase_DataBase
{
	//通过ID拿数据
	public static BuildBase_Property GetPropertyByID(int id)
	{
		return BuildBase_Data.GetBuildBase_DataByID(id);
	}

	//通过下标拿数据
	public static BuildBase_Property GetPropertyByIndex(int index)
	{
		return BuildBase_Data.GetBuildBase_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return BuildBase_Data.ArrayLenth;
	}

	//获取数组
	public static BuildBase_PropertyBase[] GetArray(int index)
	{
		return BuildBase_Data.DataArray;
	}
}

public class BuildBase_PropertyBase
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
	/// <summary>
	/// 类型
	/// </summary>
	public int Type { get; set; }
}
