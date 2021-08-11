//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using EazyGF;
using System.Numerics;

public class DeviceLevel_DataBase
{
	//通过ID拿数据
	public static DeviceLevel_Property GetPropertyByID(int id)
	{
		return DeviceLevel_Data.GetDeviceLevel_DataByID(id);
	}

	//通过下标拿数据
	public static DeviceLevel_Property GetPropertyByIndex(int index)
	{
		return DeviceLevel_Data.GetDeviceLevel_DataByIndex(index);
	}

	//获取数组长度
	public static int GetArrayLenth(int chapterID=-1)
	{
		return DeviceLevel_Data.ArrayLenth;
	}

	//获取数组
	public static DeviceLevel_PropertyBase[] GetArray(int index)
	{
		return DeviceLevel_Data.DataArray;
	}
}

public class DeviceLevel_PropertyBase
{
	/// <summary>
	/// Id
	/// </summary>
	public int ID { get; set; }
	/// <summary>
	/// 等级
	/// </summary>
	public int Level { get; set; }
	/// <summary>
	/// 类型
	/// </summary>
	public int Type { get; set; }
	/// <summary>
	/// 需要解锁的金币数
	/// </summary>
	public int Unlock_coin { get; set; }
	/// <summary>
	/// 需要的星星
	/// </summary>
	public int Need_star { get; set; }
	/// <summary>
	/// 摊位容纳人数
	/// </summary>
	public int Capacity_num { get; set; }
	/// <summary>
	/// 资源链接
	/// </summary>
	public string Path_num { get; set; }
	/// <summary>
	/// 停留时间
	/// </summary>
	public int Stay_time { get; set; }
}
