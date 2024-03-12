using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    private static GameLogic instance;
    public static GameLogic Instance => instance;
    /// <summary>
    /// ��Ϸ�Ƿ��������У�������ͣ��ʼ����������Ч�����Ƿ񱻶�ȡ
    /// </summary>
    public bool gameIsRunning = false;
    /// <summary>
    /// Ŀǰ��Ҫ���õ����࣬��Ϊֻ��˫����Ϸ������û��ʹ��ö�٣�ʹ��0Ϊcross���棩��1Ϊcircle��Ȧ��
    /// </summary>
    public int nowPattern = 0;
    /// <summary>
    /// ��Ҳ�һ��ִ�̶�������
    /// </summary>
    public bool isPlayerTurn = true;
    /// <summary>
    /// �������е�Slot
    /// </summary>
    public List<Slot> slots = new List<Slot>();
    /// <summary>
    ///  ��ǰʹ�õ�Ai�߼�
    /// </summary>
    public AILogicBase activeAILogic;
    /// <summary>
    /// Aiͣ�ٵ�ʱ��
    /// </summary>
    public float aiWaitTime = 0.5f;
    //Aiͣ�ټ�ʱ��
    float m_aiWaitTime;


    //����ͨ��Ԥ����ֱ�Ӽ��أ�ֻ��ע�ἴ�ɣ�Ҳ����ͨ��ѭ�����ص�ָ��λ���ٵ���
    void Awake()
    {
        instance = this as GameLogic;
        if (activeAILogic == null) activeAILogic = FindObjectOfType<AILogicBase>().GetComponent<AILogicBase>();
        //�ȼ�һ��0���ռλ��
        if (slots.Count == 0) slots.Add(null);
        foreach (Slot slot in this.GetComponentsInChildren<Slot>())
        {
            slot.logic = this;
            slots.Add(slot);
        }
        for (int i = 1; i < slots.Count; i++)
        {
            slots[i].index = i;
        }
        isPlayerTurn = GameManager.Instance.defaultIsPlayerTurn;
        nowPattern = GameManager.Instance.defaultPattern;
    }
    /// <summary>
    /// ����ָ��ֵ��ָ��λ���Ƿ��ʤ��
    /// </summary>
    /// <param name="input"></param>
    /// <param name="targetIcon"></param>
    /// <returns></returns>
    public bool LogicWinTest(int input,int targetIcon)
    {
        int[,] targetTest = WinRule(input);
        for (int i = 0; i < targetTest.GetLength(0); i++)
        {
            if (slots[targetTest[i,0]].icon == targetIcon && slots[targetTest[i, 1]].icon == targetIcon)
                return true;
        }
        return false;
    }
    /// <summary>
    /// ����ⳡ���Ƿ�ƽ��
    /// </summary>
    /// <returns></returns>
    public bool LogicRawTest()
    {
        for (int i = 1; i < slots.Count; i++)
        {
            if (!slots[i].isFilled)
                return false;            
        }
        return true;
    }
    /// <summary>
    /// ��һ����
    /// </summary>
    /// <param name="input"></param>
    public void Set(int input)
    {
        if (!gameIsRunning) return;
        slots[input].icon = nowPattern;
        GameManager.Instance.Record(input);//����GM
        GameManager.Instance.Report((isPlayerTurn == true ? "Player" : "CP") + " place " +  (nowPattern == 0 ? "<color=red>red cross</color>" : "<color=green>green circle</color>") + " on " + input);
        
        if (LogicWinTest(input, nowPattern))
        {
            GameManager.Instance.GameEnd(false);
            return;//����GM
        }
        else if (LogicRawTest()) GameManager.Instance.GameEnd(true);//����GM
        else
        {            
            nowPattern = (nowPattern == 0 ? 1 : 0);
            isPlayerTurn = (isPlayerTurn == false ? true : false);
            GameManager.Instance.GameStep(isPlayerTurn == true ? "��һغ�" : "���Իغ�");
        }

    }
    /// <summary>
    /// ���ùؿ�
    /// </summary>
    public void Reload()
    {
        for (int i = 1; i < slots.Count; i++)
        {
            slots[i].icon = -1;
        }
    }
    /// <summary>
    /// ���ǵ��ý��������е���С��
    /// ���ﷵ��2��2�����ֵĶ�ά����
    /// ������һ��Ŀ���λ����Ҫ���ʤ��������
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    int[,] WinRule(int index)
    {
        switch (index)
        {
            case 1:
                return new int[,] { { 2, 3 }, { 4, 7 }, { 5, 9 } };
            case 2:
                return new int[,] { { 1, 3 }, { 5, 8 } };
            case 3:
                return new int[,] { { 1, 2 }, { 6, 9 }, { 5, 7 } };
            case 4:
                return new int[,] { { 5, 6 }, { 1, 7 } };
            case 5:
                return new int[,] { { 4, 6 }, { 2, 8 }, { 1, 9 }, { 3, 7 } };
            case 6:
                return new int[,] { { 4, 5 }, { 3, 9 } };
            case 7:
                return new int[,] { { 8, 9 }, { 1, 4 }, { 5, 3 } };
            case 8:
                return new int[,] { { 7, 9 }, { 2, 5 } };
            case 9:
                return new int[,] { { 7, 8 }, { 3, 6 }, { 1, 5 } };
        }
        return null;
    }
    void Update()
    {
        // ���������һ�����壬ģ��Ai��˼����������ǿ�������
        if (!isPlayerTurn && gameIsRunning)
        {
            m_aiWaitTime += Time.deltaTime;
            if(m_aiWaitTime > aiWaitTime)
            {
                m_aiWaitTime = 0;
                Set(activeAILogic.AIDone(slots));
            }
        }
        //��ʱ�����ڴ˵ı�Я�˳��뻽�����̨
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.BackQuote)) GameManager.Instance.AwakeOrHideConsole();
    }
}
