//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Staff_Level_DataBase
{
	//通过ID拿数据
	public static Staff_Level_Property GetPropertyByID(int id)
	{
		return Staff_Level_Data.GetStaff_Level_DataByID(id);
	}

	//通过下标拿数据
	public static Staff_Level_Property GetPropertyByIndex(int index)
	{
		return Staff_Level_Data.GetStaff_Level_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Staff_Level_Data.ArrayLenth;
	}

	//获取数组
	public static Staff_Level_PropertyBase[] GetArray(int index)
	{
		return Staff_Level_Data.DataArray;
	}
}

public class Staff_Level_PropertyBase
{
	/// <summary>
	/// ID
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 等级
	/// </summary>
	public int Level { get; set; }
	/// <summary>
	/// 技能参数（多少秒放一次  比如每60秒招募4个顾客就是60_2, 一个参数就传一个）
	/// </summary>
	public int[] SkllParam { get; set; }
	/// <summary>
	/// 雇佣价格（第一级默认为雇佣价格)
	/// </summary>
	public int EmpPrice { get; set; }
	/// <summary>
	///  资历时间 (第一级默认为0  最大等级为-1)
	/// </summary>
	public int QuaTime { get; set; }
}
