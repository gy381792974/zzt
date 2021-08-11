//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CubeItem_DataBase
{
	//通过ID拿数据
	public static CubeItem_Property GetPropertyByID(int id)
	{
		return CubeItem_Data.GetCubeItem_DataByID(id);
	}

	//通过下标拿数据
	public static CubeItem_Property GetPropertyByIndex(int index)
	{
		return CubeItem_Data.GetCubeItem_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CubeItem_Data.ArrayLenth;
	}

	//获取数组
	public static CubeItem_PropertyBase[] GetArray(int index)
	{
		return CubeItem_Data.DataArray;
	}
}

public class CubeItem_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 备注1
	/// </summary>
	public string beizhuA { get; set; }
	/// <summary>
	/// ab包名
	/// </summary>
	public string AbName { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string TextureName { get; set; }
	/// <summary>
	/// 音效名称（默认0）
	/// </summary>
	public string SoundName { get; set; }
}
