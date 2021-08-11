//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class guide_step_DataBase
{
	//通过ID拿数据
	public static guide_step_Property GetPropertyByID(int id)
	{
		return guide_step_Data.Getguide_step_DataByID(id);
	}

	//通过下标拿数据
	public static guide_step_Property GetPropertyByIndex(int index)
	{
		return guide_step_Data.Getguide_step_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return guide_step_Data.ArrayLenth;
	}

	//获取数组
	public static guide_step_PropertyBase[] GetArray(int index)
	{
		return guide_step_Data.DataArray;
	}
}

public class guide_step_PropertyBase
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
	/// 引导步骤
	/// </summary>
	public int step { get; set; }
	/// <summary>
	/// 高亮锚点
	/// </summary>
	public string lightAnchor { get; set; }
	/// <summary>
	/// 锚点类型
	/// </summary>
	public int AnchorType { get; set; }
}
