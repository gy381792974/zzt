//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Adorn_DataBase
{
	//通过ID拿数据
	public static Adorn_Property GetPropertyByID(int id)
	{
		return Adorn_Data.GetAdorn_DataByID(id);
	}

	//通过下标拿数据
	public static Adorn_Property GetPropertyByIndex(int index)
	{
		return Adorn_Data.GetAdorn_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Adorn_Data.ArrayLenth;
	}

	//获取数组
	public static Adorn_PropertyBase[] GetArray(int index)
	{
		return Adorn_Data.DataArray;
	}
}

public class Adorn_PropertyBase
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
	/// 等级
	/// </summary>
	public int Level { get; set; }
	/// <summary>
	/// 需要解锁的金币数
	/// </summary>
	public int UnlockCoin { get; set; }
	/// <summary>
	/// 需要的星星
	/// </summary>
	public int NeedStar { get; set; }
	/// <summary>
	/// 装饰容纳人数
	/// </summary>
	public int CapacityNum { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] BuildName { get; set; }
	/// <summary>
	/// 介绍
	/// </summary>
	public int[] BuildIntro { get; set; }
	/// <summary>
	/// 说明
	/// </summary>
	public int[] Desc { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string IconName { get; set; }
	/// <summary>
	/// 每5s增加额外收益
	/// </summary>
	public int ExtraGold { get; set; }
}
