using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogicBase : MonoBehaviour
{
    /// <summary>
    /// ���AI���Ѷ�
    /// </summary>
    public int difficulty;
    /// <summary>
    /// ���AI�ļ�ֵ��,��1-9��0�ÿ�
    /// </summary>
    [Header("���AI�ļ�ֵ��,��1-9��0�ÿ�")]
    public List<float> valueDic = new List<float>();
    /// <summary>
    /// ��Ҫ��д��������Ĭ����ȫ��ѭ��ֵ��
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
