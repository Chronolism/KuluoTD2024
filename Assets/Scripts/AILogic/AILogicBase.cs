using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogicBase : MonoBehaviour
{
    /// <summary>
    /// 这个AI的难度
    /// </summary>
    public int difficulty;
    /// <summary>
    /// 这个AI的价值表,从1-9，0置空
    /// </summary>
    [Header("这个AI的价值表,从1-9，0置空")]
    public List<float> valueDic = new List<float>();
    /// <summary>
    /// 需要重写，这里是默认完全遵循价值表
    /// </summary>
    /// <param name="slots"></param>
    /// <returns></returns>
    public virtual int AIDone(List<Slot> slots)
    {
        int index = 0;
        float value = 0;
        for (int i = 1; i < slots.Count; i++)
        {
            if (!slots[i].isFilled)
            {
                if (i < valueDic.Count)
                {
                    if (valueDic[i] > value)
                    {
                        value = valueDic[i];
                        index = i;
                    }
                }
            }
        }
        return index;
    }
}
