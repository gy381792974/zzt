using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This扩展
/// </summary>
public static class ThisExtend 
{
    /// <summary>
    /// 字典扩展
    /// 查询第一个或者最后一个满足条件的键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="selfDict"></param>
    /// <param name="func"></param>
    /// <param name="firstOrEnd"></param>
    /// <returns></returns>
    public static KeyValuePair<TKey, TValue> Find<TKey, TValue>(this Dictionary<TKey, TValue> selfDict, Func<KeyValuePair<TKey, TValue>, bool> func ,bool firstOrEnd)
    {
        KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>();

        foreach (var item in selfDict)
        {
            if (func.Invoke(item)) {
                keyValuePair = item;
                if (firstOrEnd) {
                    break; }
               
            }
        }
        return keyValuePair;
    }

    /// <summary>
    /// 字典扩展
    /// 查询并返回所有符合条件的键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="selfDict"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Dictionary<TKey, TValue> FindAll<TKey, TValue>(this Dictionary<TKey, TValue> selfDict, Func<KeyValuePair<TKey, TValue>, bool> func) {

        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        foreach (var item in selfDict)
        {
            if (func(item)) {
                dict.Add(item.Key,item.Value);
            }
        }

        return dict;



    }


}
