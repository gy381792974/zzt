//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class TANWEI_DataBase
{
	//通过ID拿数据
	public static TANWEI_Property GetPropertyByID(int id)
	{
		return TANWEI_Data.GetTANWEI_DataByID(id);
	}

	//通过下标拿数据
	public static TANWEI_Property GetPropertyByIndex(int index)
	{
		return TANWEI_Data.GetTANWEI_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return TANWEI_Data.ArrayLenth;
	}

	//获取数组
	public static TANWEI_PropertyBase[] GetArray(int index)
	{
		return TANWEI_Data.DataArray;
	}
}

public class TANWEI_PropertyBase
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
	/// JB
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
