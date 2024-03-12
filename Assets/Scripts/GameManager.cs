
using UnityEngine;

public class GameManager
{
    /// <summary>
    /// 单例
    /// </summary>
    private static GameManager instance = new GameManager();
    public static GameManager Instance => instance;
    /// <summary>
    /// 游戏逻辑
    /// </summary>
    public GameLogic gameLogic;
    /// <summary>
    /// 游戏UI面板
    /// </summary>
    public GamePanel gamePanel;
    /// <summary>
    /// 游戏控制台
    /// </summary>
    public ConsolePanel consolePanel;
    /// <summary>
    /// 游戏测试数据管理
    /// </summary>
    public QACentre qACentre;
    /// <summary>
    /// 默认先手
    /// </summary>
    public bool defaultIsPlayerTurn;
    /// <summary>
    /// 默认先手图案
    /// </summary>
    public int defaultPattern;
    //控制台是否已隐藏
    public bool m_IsConsoleHide = false;
    /// <summary>
    /// 游戏开始
    /// </summary>
    public void GameStart()
    {
        if (gameLogic == null) return;
        if (qACentre != null)
            qACentre.GameSetStart(gameLogic.isPlayerTurn, gameLogic.nowPattern);
        if (consolePanel != null) 
            consolePanel.EnableConsole();
        gameLogic.gameIsRunning = true;
    }
    /// <summary>
    /// 游戏重开
    /// </summary>
    public void GameReload()
    {
        if (gameLogic == null) return;
        gameLogic.isPlayerTurn = defaultIsPlayerTurn;
        gameLogic.nowPattern = defaultPattern;
        gameLogic.Reload();

    }
    /// <summary>
    /// 游戏结束
    /// </summary>
    /// <param name="isRaw"></param>
    public void GameEnd(bool isRaw)
    {
        gameLogic.gameIsRunning = false;
        if (gamePanel != null) 
        {
            if (isRaw) gamePanel.gameResult.text = "平局!";
            else gamePanel.gameResult.text = (GameLogic.Instance.isPlayerTurn ? "玩家" : "电脑") + "胜利!";
            gamePanel.EndGame();
        }
        if (qACentre != null)
            qACentre.RecordThisGameSet(isRaw, GameLogic.Instance.isPlayerTurn);
        if (consolePanel != null)
        {
            consolePanel.DisableConsole();
            consolePanel.Report("Game End");
        }
           

    }
    /// <summary>
    /// 游戏进入到下一个回合
    /// </summary>
    /// <param name="contains"></param>
    public void GameStep(string contains)
    {
        if (gamePanel != null)
        {
            gamePanel.gameTip.text = contains;
        }
    }
    /// <summary>
    /// 撤销一步
    /// </summary>
    public void Quash()
    {
        if(qACentre == null)
        {
            consolePanel.Report("<color=red>This func need enable QAcentre</color>");
            return;
        }
        int quashPlace = qACentre.RemoveStep();
        if (quashPlace < 1)
        {
            consolePanel.Report("<color=red>Over Stack</color>");
            return;
        }
        gameLogic.slots[quashPlace].icon = -1;
        gameLogic.nowPattern = (gameLogic.nowPattern == 0 ? 1 : 0);
        gameLogic.isPlayerTurn = (gameLogic.isPlayerTurn == false ? true : false);
        GameStep(gameLogic.isPlayerTurn == true ? "玩家回合" : "电脑回合");
        Report("<color=yellow>Quash Succeed</color>");
    }
    /// <summary>
    /// 捕获一次操作（玩家或电脑）
    /// </summary>
    /// <param name="where"></param>
    public void Record(int where)
    {
        if (qACentre == null) return;
        qACentre.AddStep(where);
    }
    /// <summary>
    /// 向控制台发送一条信息
    /// </summary>
    /// <param name="where"></param>
    public void Report(string contains)
    {
        if (consolePanel == null) return;
        consolePanel.Report(contains);
    }
    /// <summary>
    /// 在控制台上显示当前局的信息
    /// </summary>
    /// <returns></returns>
    public void ShowNowGameSetData()
    {
        if (qACentre == null)
        {
            consolePanel.Report("<color=red>This func need enable QAcentre</color>");
            return;
        }
        Report(qACentre.GetThisGameSetDataToDirectChinese());
    }
    /// <summary>
    /// 打印本游戏数据到文件
    /// </summary>
    /// <returns></returns>
    public void exportData()
    {
        if (qACentre == null)
        {
            consolePanel.Report("<color=red>This func need enable QAcentre</color>");
            return;
        }
        qACentre.PrintData(Application.persistentDataPath);
        Report("文件已保存在\n" + Application.persistentDataPath);
        UnityEngine.GUIUtility.systemCopyBuffer = Application.persistentDataPath;
        Report("路径已复制到剪贴板");
    }
    /// <summary>
    /// 唤起或隐藏控制台
    /// </summary>
    public void AwakeOrHideConsole()
    {
        if (!m_IsConsoleHide)
        {
            consolePanel.gameObject.SetActive(false);
            m_IsConsoleHide = true;
        }
        else
        {
            consolePanel.gameObject.SetActive(true);
            m_IsConsoleHide = false;
        }
    }
   
}
