//Generator by Tools
//Editor by YS
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class Restaurant_Property : Restaurant_PropertyBase
{

}

public class Restaurant_Data
{
	//对象数组
	public static Restaurant_Property[] DataArray;
	//对象数组长度
	public static int ArrayLenth;
	public static void SetRestaurantDataLenth()
	{
		 ArrayLenth = DataArray.Length;
	}


	//通过下标获取数据
	public static Restaurant_Property GetRestaurant_DataByIndex(int _index)
	{
		if (_index < 0 || _index >= ArrayLenth)
		{
			Debug.LogError("DataArray下标越界："+_index);
			return DataArray[0];
		}
		return DataArray[_index];
	}
}
