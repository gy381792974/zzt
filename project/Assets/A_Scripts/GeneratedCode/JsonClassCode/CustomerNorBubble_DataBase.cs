//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CustomerNorBubble_DataBase
{
	//通过ID拿数据
	public static CustomerNorBubble_Property GetPropertyByID(int id)
	{
		return CustomerNorBubble_Data.GetCustomerNorBubble_DataByID(id);
	}

	//通过下标拿数据
	public static CustomerNorBubble_Property GetPropertyByIndex(int index)
	{
		return CustomerNorBubble_Data.GetCustomerNorBubble_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CustomerNorBubble_Data.ArrayLenth;
	}

	//获取数组
	public static CustomerNorBubble_PropertyBase[] GetArray(int index)
	{
		return CustomerNorBubble_Data.DataArray;
	}
}

public class CustomerNorBubble_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 备注
	/// </summary>
	public string bz { get; set; }
	/// <summary>
	/// 去特定摊位的次数
	/// </summary>
	public int SpeStallNum { get; set; }
	/// <summary>
	/// 满足条件（摊位升级的等级）
	/// </summary>
	public int ConParam { get; set; }
	/// <summary>
	/// 开心文本ID
	/// </summary>
	public int[] HappyTxt { get; set; }
	/// <summary>
	/// 吐糟文本ID
	/// </summary>
	public int[] RantTxt { get; set; }
}
