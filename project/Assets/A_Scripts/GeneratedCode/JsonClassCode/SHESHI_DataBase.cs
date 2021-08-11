//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class SHESHI_DataBase
{
	//通过ID拿数据
	public static SHESHI_Property GetPropertyByID(int id)
	{
		return SHESHI_Data.GetSHESHI_DataByID(id);
	}

	//通过下标拿数据
	public static SHESHI_Property GetPropertyByIndex(int index)
	{
		return SHESHI_Data.GetSHESHI_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return SHESHI_Data.ArrayLenth;
	}

	//获取数组
	public static SHESHI_PropertyBase[] GetArray(int index)
	{
		return SHESHI_Data.DataArray;
	}
}

public class SHESHI_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 字符串1
	/// </summary>
	public string name { get; set; }
	/// <summary>
	/// 金币
	/// </summary>
	public int GOLD { get; set; }
	/// <summary>
	/// 类型
	/// </summary>
	public int type { get; set; }
	/// <summary>
	/// 所需星星
	/// </summary>
	public int star { get; set; }
}
