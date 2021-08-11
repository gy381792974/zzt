//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Equip_DataBase
{
	//通过ID拿数据
	public static Equip_Property GetPropertyByID(int id)
	{
		return Equip_Data.GetEquip_DataByID(id);
	}

	//通过下标拿数据
	public static Equip_Property GetPropertyByIndex(int index)
	{
		return Equip_Data.GetEquip_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Equip_Data.ArrayLenth;
	}

	//获取数组
	public static Equip_PropertyBase[] GetArray(int index)
	{
		return Equip_Data.DataArray;
	}
}

public class Equip_PropertyBase
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
	/// 资源链接
	/// </summary>
	public string Path { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string IconName { get; set; }
}
