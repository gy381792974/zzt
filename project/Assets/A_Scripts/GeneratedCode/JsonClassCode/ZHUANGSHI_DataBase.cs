//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class ZHUANGSHI_DataBase
{
	//通过ID拿数据
	public static ZHUANGSHI_Property GetPropertyByID(int id)
	{
		return ZHUANGSHI_Data.GetZHUANGSHI_DataByID(id);
	}

	//通过下标拿数据
	public static ZHUANGSHI_Property GetPropertyByIndex(int index)
	{
		return ZHUANGSHI_Data.GetZHUANGSHI_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return ZHUANGSHI_Data.ArrayLenth;
	}

	//获取数组
	public static ZHUANGSHI_PropertyBase[] GetArray(int index)
	{
		return ZHUANGSHI_Data.DataArray;
	}
}

public class ZHUANGSHI_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 装饰
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
