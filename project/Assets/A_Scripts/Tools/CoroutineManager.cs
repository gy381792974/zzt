using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协程管理类
/// 赋予非MonoBehaviour以调用协程的能力
/// Peanut
/// </summary>
public class CoroutineManager : Singleton<CoroutineManager>
{
    /// <summary>
    /// 启动一个协程
    /// </summary>
    /// <param name="routine"></param>
    public Coroutine DoCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }


    /// <summary>
    /// 等待一段时间做一件事
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="ToDo"></param>
    public void DelayToDo(float waitTime, Action ToDo)
    {
        StartCoroutine(Delay(waitTime, ToDo));
    }
  
    /// <summary>
    /// 等待一段时间(seconds)
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="ToDo"></param>
    /// <returns></returns>
    private IEnumerator Delay(float waitTime, Action ToDo)
    {
        yield return new WaitForSeconds(waitTime);

        if (ToDo != null)
        {
            ToDo();
        }
    }

}
