using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLogic : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static GameLogic instance;
    public static GameLogic Instance => instance;
    /// <summary>
    /// 游戏是否正在运行，用于暂停开始结束，即有效操作是否被读取
    /// </summary>
    public bool gameIsRunning = false;
    /// <summary>
    /// 目前需要放置的种类，因为只有双方游戏，这里没有使用枚举，使用0为cross（叉），1为circle（圈）
    /// </summary>
    public int nowPattern = 0;
    /// <summary>
    /// 玩家不一定执固定的种类
    /// </summary>
    public bool isPlayerTurn = true;
    /// <summary>
    /// 场上所有的Slot
    /// </summary>
    public List<Slot> slots = new List<Slot>();
    /// <summary>
    ///  当前使用的Ai逻辑
    /// </summary>
    public AILogicBase activeAILogic;
    /// <summary>
    /// Ai停顿的时间
    /// </summary>
    public float aiWaitTime = 0.5f;
    //Ai停顿计时器
    float m_aiWaitTime;


    //这里通过预制体直接加载，只需注册即可，也可以通过循环加载到指定位置再调用
    void Awake()
    {
        instance = this as GameLogic;
        if (activeAILogic == null) activeAILogic = FindObjectOfType<AILogicBase>().GetComponent<AILogicBase>();
        //先加一个0序号占位符
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
    /// 输入指定值到指定位置是否会胜利
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
    /// 仅检测场面是否平局
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
    /// 下一步棋
    /// </summary>
    /// <param name="input"></param>
    public void Set(int input)
    {
        if (!gameIsRunning) return;
        slots[input].icon = nowPattern;
        GameManager.Instance.Record(input);//接入GM
        GameManager.Instance.Report((isPlayerTurn == true ? "Player" : "CP") + " place " +  (nowPattern == 0 ? "<color=red>red cross</color>" : "<color=green>green circle</color>") + " on " + input);
        
        if (LogicWinTest(input, nowPattern))
        {
            GameManager.Instance.GameEnd(false);
            return;//接入GM
        }
        else if (LogicRawTest()) GameManager.Instance.GameEnd(true);//接入GM
        else
        {            
            nowPattern = (nowPattern == 0 ? 1 : 0);
            isPlayerTurn = (isPlayerTurn == false ? true : false);
            GameManager.Instance.GameStep(isPlayerTurn == true ? "玩家回合" : "电脑回合");
        }

    }
    /// <summary>
    /// 重置关卡
    /// </summary>
    public void Reload()
    {
        for (int i = 1; i < slots.Count; i++)
        {
            slots[i].icon = -1;
        }
    }
    /// <summary>
    /// 考虑到用交错数组有点大材小用
    /// 这里返回2个2个数字的二维数组
    /// 即传入一个目标点位后需要检测胜利的条件
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
        // 这里设计了一个缓冲，模拟Ai的思考，可以增强玩家体验
        if (!isPlayerTurn && gameIsRunning)
        {
            m_aiWaitTime += Time.deltaTime;
            if(m_aiWaitTime > aiWaitTime)
            {
                m_aiWaitTime = 0;
                Set(activeAILogic.AIDone(slots));
            }
        }
        //临时放置于此的便携退出与唤起控制台
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.BackQuote)) GameManager.Instance.AwakeOrHideConsole();
    }
}
