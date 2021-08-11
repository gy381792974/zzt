//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Item_DataBase
{
	//通过ID拿数据
	public static Item_Property GetPropertyByID(int id)
	{
		return Item_Data.GetItem_DataByID(id);
	}

	//通过下标拿数据
	public static Item_Property GetPropertyByIndex(int index)
	{
		return Item_Data.GetItem_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Item_Data.ArrayLenth;
	}

	//获取数组
	public static Item_PropertyBase[] GetArray(int index)
	{
		return Item_Data.DataArray;
	}
}

public class Item_PropertyBase
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
	/// 名称
	/// </summary>
	public int[] Name { get; set; }
	/// <summary>
	/// 说明
	/// </summary>
	public int[] Desc { get; set; }
	/// <summary>
	/// 使用方式
	/// </summary>
	public int UserType { get; set; }
	/// <summary>
	/// 资源路径
	/// </summary>
	public string Path { get; set; }
	/// <summary>
	/// 图标所在目录
	/// </summary>
	public string IconDir { get; set; }
	/// <summary>
	/// 图标名称
	/// </summary>
	public string IconName { get; set; }
	/// <summary>
	/// 游戏初始给的数量
	/// </summary>
	public int InitNum { get; set; }
	/// <summary>
	/// 购买一次的数量
	/// </summary>
	public int BuyNum { get; set; }
	/// <summary>
	/// 价格
	/// </summary>
	public int Price { get; set; }
}
