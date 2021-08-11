//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class Cube_DataBase
{
	//通过ID拿数据
	public static Cube_Property GetPropertyByID(int id)
	{
		return Cube_Data.GetCube_DataByID(id);
	}

	//通过下标拿数据
	public static Cube_Property GetPropertyByIndex(int index)
	{
		return Cube_Data.GetCube_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return Cube_Data.ArrayLenth;
	}

	//获取数组
	public static Cube_PropertyBase[] GetArray(int index)
	{
		return Cube_Data.DataArray;
	}
}

public class Cube_PropertyBase
{
	/// <summary>
	/// ID（也可以代表关卡）
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 层数
	/// </summary>
	public int Layout { get; set; }
	/// <summary>
	/// 所在位置
	/// </summary>
	public int[] IndexPos { get; set; }
	/// <summary>
	/// 偏移量
	/// </summary>
	public int PosX { get; set; }
	/// <summary>
	/// 偏移量
	/// </summary>
	public int PosY { get; set; }
	/// <summary>
	/// 方块数量
	/// </summary>
	public int CubeNum { get; set; }
	/// <summary>
	/// 方块id
	/// </summary>
	public int CubeLibId { get; set; }
	/// <summary>
	/// 类型(种类不能超过库CubeConfig表的对应种类数)
	/// </summary>
	public int Type { get; set; }
	/// <summary>
	/// 类型（最大相同种类数/单位：双）
	/// </summary>
	public int TypeNum { get; set; }
}
