//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class GuideStep_DataBase
{
	//通过ID拿数据
	public static GuideStep_Property GetPropertyByID(int id)
	{
		return GuideStep_Data.GetGuideStep_DataByID(id);
	}

	//通过下标拿数据
	public static GuideStep_Property GetPropertyByIndex(int index)
	{
		return GuideStep_Data.GetGuideStep_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return GuideStep_Data.ArrayLenth;
	}

	//获取数组
	public static GuideStep_PropertyBase[] GetArray(int index)
	{
		return GuideStep_Data.DataArray;
	}
}

public class GuideStep_PropertyBase
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
	public string LightAnchor { get; set; }
	/// <summary>
	/// 锚点类型（0:2d 1:3d）
	/// </summary>
	public int AnchorType { get; set; }
	/// <summary>
	/// 操作方式（0 点击任意区域 1点击特定区域）
	/// </summary>
	public int ShowStep { get; set; }
}
