//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CARD_DataBase
{
	//通过ID拿数据
	public static CARD_Property GetPropertyByID(int id)
	{
		return CARD_Data.GetCARD_DataByID(id);
	}

	//通过下标拿数据
	public static CARD_Property GetPropertyByIndex(int index)
	{
		return CARD_Data.GetCARD_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CARD_Data.ArrayLenth;
	}

	//获取数组
	public static CARD_PropertyBase[] GetArray(int index)
	{
		return CARD_Data.DataArray;
	}
}

public class CARD_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 名称
	/// </summary>
	public string name { get; set; }
	/// <summary>
	/// 翻译
	/// </summary>
	public string fanyi { get; set; }
	/// <summary>
	/// 介绍
	/// </summary>
	public string jieshao { get; set; }
	/// <summary>
	/// 解锁条件
	/// </summary>
	public string tiaojian { get; set; }
}
