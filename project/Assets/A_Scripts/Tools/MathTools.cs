using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EazyGF
{
    public class MathTools
    {
        /// <summary>
        /// 保留2位小数点
        /// </summary>
        /// <returns></returns>
        public static float ConvertTwoDecimal<T>(T t) where T : struct
        {
            return (float)Math.Round(Convert.ToDecimal(t), 2);
        }


        /// <summary>
        /// 随机打乱Array数组
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="numArray">需要打乱的数组</param>
        /// <returns>返回打乱后的数组</returns>
        public static T[] RandomArray<T>(T[] numArray)
        {
            for (int i = 0; i < numArray.Length; i++)
            {
                T temp = numArray[i];
                int randomIndex = Random.Range(0, numArray.Length);
                numArray[i] = numArray[randomIndex];
                numArray[randomIndex] = temp;
            }
            return numArray;
        }

        //随机打乱List数组
        public static List<T> RandomList<T>(List<T> numLsit)
        {
            List<T> newList = new List<T>();
            foreach (T item in numLsit)
            {
                newList.Insert(Random.Range(0, newList.Count), item);
            }
            return newList;
        }

        /// <summary>
        /// 根据权重从数组中随机出一个下标
        /// </summary>
        /// <param name="weightList"></param>
        /// <param name="totalWeight">当前数组总权重，因为这个权重值基本是固定的，最好在外部算好了传进来，免得多走一次循环</param>
        /// <returns></returns>
        public static int GetRandIndexByWeidth(List<int> weightList, int totalWeight = -1)
        {
            if (totalWeight == -1)
            {
                totalWeight = 0;
                for (int i = 0; i < weightList.Count; i++)
                {
                    totalWeight += weightList[i];
                }
            }
            int randomNum = Random.Range(0, totalWeight);
            for (int i = 0; i < weightList.Count; i++)
            {
                if (randomNum <= weightList[i])
                {
                    return i;
                }
                randomNum -= weightList[i];
            }
            Debug.LogError("算法有误，找不到权重下标！请打死写这个算法的人！");
            return 0;
        }

        /// <summary>
        /// 根据权重从数组中随机出一个下标
        /// </summary>
        /// <param name="weightList"></param>
        /// <param name="totalWeight">当前数组总权重，因为这个权重值基本是固定的，最好在外部算好了传进来，免得多走一次循环</param>
        /// <returns></returns>
        public static int GetRandIndexByWeidth(int[] weightList, int totalWeight = -1)
        {
            if (totalWeight == -1)
            {
                totalWeight = 0;
                for (int i = 0; i < weightList.Length; i++)
                {
                    totalWeight += weightList[i];
                }
            }

            int randomNum = Random.Range(0, totalWeight);
            for (int i = 0; i < weightList.Length; i++)
            {
                if (randomNum <= weightList[i])
                {
                    return i;
                }
                randomNum -= weightList[i];
            }
            Debug.LogError("算法有误，找不到权重下标！请打死写这个算法的人！");
            return 0;
        }

    }
}

