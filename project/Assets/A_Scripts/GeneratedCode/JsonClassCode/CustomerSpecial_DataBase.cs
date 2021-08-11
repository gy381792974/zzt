//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CustomerSpecial_DataBase
{
	//通过ID拿数据
	public static CustomerSpecial_Property GetPropertyByID(int id)
	{
		return CustomerSpecial_Data.GetCustomerSpecial_DataByID(id);
	}

	//通过下标拿数据
	public static CustomerSpecial_Property GetPropertyByIndex(int index)
	{
		return CustomerSpecial_Data.GetCustomerSpecial_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CustomerSpecial_Data.ArrayLenth;
	}

	//获取数组
	public static CustomerSpecial_PropertyBase[] GetArray(int index)
	{
		return CustomerSpecial_Data.DataArray;
	}
}

public class CustomerSpecial_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] Name { get; set; }
	/// <summary>
	/// 没有解锁的介绍
	/// </summary>
	public int[] LockIntro { get; set; }
	/// <summary>
	/// 正常显示的介绍
	/// </summary>
	public int[] Intro { get; set; }
	/// <summary>
	/// 特殊事件触发时的对话内容
	/// </summary>
	public int[] Content { get; set; }
	/// <summary>
	/// 游戏成功(获得奖励)时的对话内容
	/// </summary>
	public int[] GameSuc { get; set; }
	/// <summary>
	/// 游戏失败（没有获得奖励）时的对话内容
	/// </summary>
	public int[] GameFail { get; set; }
	/// <summary>
	/// 游戏界面标题
	/// </summary>
	public int[] titleText { get; set; }
	/// <summary>
	/// 角色索引
	/// </summary>
	public int RoleIndex { get; set; }
	/// <summary>
	/// 角色资源包名
	/// </summary>
	public string ABName { get; set; }
	/// <summary>
	/// 角色背面资源包名
	/// </summary>
	public string ABBackName { get; set; }
	/// <summary>
	/// 角色名字
	/// </summary>
	public string RoleName { get; set; }
	/// <summary>
	/// 角色背面名字
	/// </summary>
	public string RoleBackName { get; set; }
	/// <summary>
	/// 资源链接
	/// </summary>
	public string Path { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string IconName { get; set; }
}
