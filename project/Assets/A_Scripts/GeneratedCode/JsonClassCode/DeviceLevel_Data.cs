﻿//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class DeviceLevel_Property : DeviceLevel_PropertyBase
{

}

public class DeviceLevel_Data
{
	//对象数组
	public static DeviceLevel_Property[] DataArray;
	//对象数组长度
	public static int ArrayLenth;
	public static void SetDeviceLevelDataLenth()
	{
		 ArrayLenth = DataArray.Length;
	}

	//通过ID获取数据
	public static DeviceLevel_Property GetDeviceLevel_DataByID(int _id)
	{
		for (int i = 0; i < ArrayLenth; i++)
		{
			if ( DataArray[i].ID == _id )
			{
				return DataArray[i];
			}
		}
		Debug.LogError("DataArray中没有该ID："+_id);
		return null;
	}

	//通过下标获取数据
	public static DeviceLevel_Property GetDeviceLevel_DataByIndex(int _index)
	{
		if (_index < 0 || _index >= ArrayLenth)
		{
			Debug.LogError("DataArray下标越界："+_index);
			return DataArray[0];
		}
		return DataArray[_index];
	}
}
