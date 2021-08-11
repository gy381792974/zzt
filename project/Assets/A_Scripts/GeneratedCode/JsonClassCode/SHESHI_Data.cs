//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class SHESHI_Property : SHESHI_PropertyBase
{

}

public class SHESHI_Data
{
	//对象数组
	public static SHESHI_Property[] DataArray;
	//对象数组长度
	public static int ArrayLenth;
	public static void SetSHESHIDataLenth()
	{
		 ArrayLenth = DataArray.Length;
	}

	//通过ID获取数据
	public static SHESHI_Property GetSHESHI_DataByID(int _id)
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
	public static SHESHI_Property GetSHESHI_DataByIndex(int _index)
	{
		if (_index < 0 || _index >= ArrayLenth)
		{
			Debug.LogError("DataArray下标越界："+_index);
			return DataArray[0];
		}
		return DataArray[_index];
	}
}
