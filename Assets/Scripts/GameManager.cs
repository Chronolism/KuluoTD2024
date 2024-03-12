
using UnityEngine;

public class GameManager
{
    /// <summary>
    /// ����
    /// </summary>
    private static GameManager instance = new GameManager();
    public static GameManager Instance => instance;
    /// <summary>
    /// ��Ϸ�߼�
    /// </summary>
    public GameLogic gameLogic;
    /// <summary>
    /// ��ϷUI���
    /// </summary>
    public GamePanel gamePanel;
    /// <summary>
    /// ��Ϸ����̨
    /// </summary>
    public ConsolePanel consolePanel;
    /// <summary>
    /// ��Ϸ�������ݹ���
    /// </summary>
    public QACentre qACentre;
    /// <summary>
    /// Ĭ������
    /// </summary>
    public bool defaultIsPlayerTurn;
    /// <summary>
    /// Ĭ������ͼ��
    /// </summary>
    public int defaultPattern;
    //����̨�Ƿ�������
    public bool m_IsConsoleHide = false;
    /// <summary>
    /// ��Ϸ��ʼ
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
    /// ��Ϸ�ؿ�
    /// </summary>
    public void GameReload()
    {
        if (gameLogic == null) return;
        gameLogic.isPlayerTurn = defaultIsPlayerTurn;
        gameLogic.nowPattern = defaultPattern;
        gameLogic.Reload();

    }
    /// <summary>
    /// ��Ϸ����
    /// </summary>
    /// <param name="isRaw"></param>
    public void GameEnd(bool isRaw)
    {
        gameLogic.gameIsRunning = false;
        if (gamePanel != null) 
        {
            if (isRaw) gamePanel.gameResult.text = "ƽ��!";
            else gamePanel.gameResult.text = (GameLogic.Instance.isPlayerTurn ? "���" : "����") + "ʤ��!";
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
    /// ��Ϸ���뵽��һ���غ�
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
    /// ����һ��
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
        GameStep(gameLogic.isPlayerTurn == true ? "��һغ�" : "���Իغ�");
        Report("<color=yellow>Quash Succeed</color>");
    }
    /// <summary>
    /// ����һ�β�������һ���ԣ�
    /// </summary>
    /// <param name="where"></param>
    public void Record(int where)
    {
        if (qACentre == null) return;
        qACentre.AddStep(where);
    }
    /// <summary>
    /// �����̨����һ����Ϣ
    /// </summary>
    /// <param name="where"></param>
    public void Report(string contains)
    {
        if (consolePanel == null) return;
        consolePanel.Report(contains);
    }
    /// <summary>
    /// �ڿ���̨����ʾ��ǰ�ֵ���Ϣ
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
    /// ��ӡ����Ϸ���ݵ��ļ�
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
        Report("�ļ��ѱ�����\n" + Application.persistentDataPath);
        UnityEngine.GUIUtility.systemCopyBuffer = Application.persistentDataPath;
        Report("·���Ѹ��Ƶ�������");
    }
    /// <summary>
    /// ��������ؿ���̨
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
