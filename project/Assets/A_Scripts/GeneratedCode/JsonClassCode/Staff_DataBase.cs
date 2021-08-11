//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Staff_DataBase
{
	//通过ID拿数据
	public static Staff_Property GetPropertyByID(int id)
	{
		return Staff_Data.GetStaff_DataByID(id);
	}

	//通过下标拿数据
	public static Staff_Property GetPropertyByIndex(int index)
	{
		return Staff_Data.GetStaff_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Staff_Data.ArrayLenth;
	}

	//获取数组
	public static Staff_PropertyBase[] GetArray(int index)
	{
		return Staff_Data.DataArray;
	}
}

public class Staff_PropertyBase
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
	/// 姓名
	/// </summary>
	public int[] Name { get; set; }
	/// <summary>
	/// 图标名字
	/// </summary>
	public string IconName { get; set; }
	/// <summary>
	/// 职位类型（说明）
	/// </summary>
	public int ObjType { get; set; }
	/// <summary>
	/// 职位名称（前台 厨师）
	/// </summary>
	public int[] ObjName { get; set; }
	/// <summary>
	/// 特殊技能说明
	/// </summary>
	public int[] SpecialSkill { get; set; }
	/// <summary>
	/// 技能类型
	/// </summary>
	public int SkillType { get; set; }
	/// <summary>
	/// 资源链接
	/// </summary>
	public string Path { get; set; }
	/// <summary>
	/// 解锁条件文字说明{可以传参数}
	/// </summary>
	public int[] UnlockDes { get; set; }
	/// <summary>
	/// 解锁参数
	/// </summary>
	public int UnlockParam { get; set; }
}
