//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class CustomerNormal_DataBase
{
	//通过ID拿数据
	public static CustomerNormal_Property GetPropertyByID(int id)
	{
		return CustomerNormal_Data.GetCustomerNormal_DataByID(id);
	}

	//通过下标拿数据
	public static CustomerNormal_Property GetPropertyByIndex(int index)
	{
		return CustomerNormal_Data.GetCustomerNormal_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return CustomerNormal_Data.ArrayLenth;
	}

	//获取数组
	public static CustomerNormal_PropertyBase[] GetArray(int index)
	{
		return CustomerNormal_Data.DataArray;
	}
}

public class CustomerNormal_PropertyBase
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
	/// 随机生成权重
	/// </summary>
	public int CreatWeight { get; set; }
	/// <summary>
	/// 名字
	/// </summary>
	public int[] Name { get; set; }
	/// <summary>
	/// 没有解锁时显示的介绍
	/// </summary>
	public int[] LockIntro { get; set; }
	/// <summary>
	/// 正常显示的介绍(说明)
	/// </summary>
	public int[] Intro { get; set; }
	/// <summary>
	/// 角色名字
	/// </summary>
	public string RoleName { get; set; }
	/// <summary>
	/// 资源链接
	/// </summary>
	public string Path { get; set; }
	/// <summary>
	/// 解锁条件（必须解锁的摊位ID）
	/// </summary>
	public int[] StallCondit { get; set; }
	/// <summary>
	/// 解锁条件（必须解锁的设施ID）
	/// </summary>
	public int[] EquipCondit { get; set; }
	/// <summary>
	/// 解锁条件（必须解锁的装饰ID）
	/// </summary>
	public int[] AdornCondit { get; set; }
	/// <summary>
	/// 必去的摊位id
	/// </summary>
	public int MustGoStall { get; set; }
	/// <summary>
	/// 二次拿餐盘的概率
	/// </summary>
	public int SecondMlRatio { get; set; }
	/// <summary>
	/// 三次那餐盘的概率
	/// </summary>
	public int ThreeTimesMRatio { get; set; }
	/// <summary>
	/// 买完食物去餐厅的概率
	/// </summary>
	public int[] MealRatio { get; set; }
	/// <summary>
	/// 解锁条件（必须解锁的摊位ID）
	/// </summary>
	public int[] BuyFoodNum { get; set; }
	/// <summary>
	/// 给小费数量（餐费的倍数）
	/// </summary>
	public int TipMultiple { get; set; }
	/// <summary>
	/// 给小费的概率
	/// </summary>
	public int TipRatio { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string IconName { get; set; }
}
