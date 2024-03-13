using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsolePanel : MonoBehaviour
{
    //����Я���ܰ�ť
    public Button lastStepButton;
    public Button showNowGameSetDataButton;
    public Button gameStopButton;
    Text gameStopButtonText;
    public Button exportDataButton;
    //�Ҳ����̨
    public Text consoleWindow;
    public InputField consoleInputField;

    private void Awake()
    {
        gameStopButtonText = gameStopButton.GetComponentInChildren<Text>();
        lastStepButton.interactable = false;
        showNowGameSetDataButton.interactable = false;
        gameStopButton.interactable = false;
        exportDataButton.interactable = false;
        consoleInputField.onEndEdit.AddListener(Command);
    }
    public void EnableConsole()
    {
        lastStepButton.interactable = true;
        showNowGameSetDataButton.interactable = true;
        gameStopButton.interactable = true;
        exportDataButton.interactable = true;

        lastStepButton.onClick.AddListener(GameManager.Instance.Quash);
        showNowGameSetDataButton.onClick.AddListener(GameManager.Instance.ShowNowGameSetData);
        gameStopButton.onClick.AddListener(StopAndContinueGame);
        exportDataButton.onClick.AddListener(GameManager.Instance.exportData);

    }
    public void DisableConsole()
    {
        lastStepButton.interactable = false;
        showNowGameSetDataButton.interactable = false;
        gameStopButton.interactable = false;
        exportDataButton.interactable = false;

        lastStepButton.onClick.RemoveAllListeners();
        showNowGameSetDataButton.onClick.RemoveAllListeners();
        gameStopButton.onClick.RemoveAllListeners();
        exportDataButton.onClick.RemoveAllListeners();
    }

    //�ڿ���̨Window��ӡ���� 
    public void Report(string contains)
    {
        consoleWindow.text += "\n" + contains;
    }
    //���ھ�������Ҫ���ٶ����߼�����ϣ�����ͨ������ʵ��
    void StopAndContinueGame()
    {
        if(GameManager.Instance.gameLogic != null)
        {
           if(GameManager.Instance.gameLogic.gameIsRunning)
           {
                GameManager.Instance.gameLogic.gameIsRunning = false;
                gameStopButtonText.text = "<color=blue>��ͣ��</color>";

           }
           else
           {
                GameManager.Instance.gameLogic.gameIsRunning = true;
                gameStopButtonText.text = "��Ϸ��ͣ";
           }
        }
    }
    //ָ�
    public void Command(string command)
    {
        if (command == "Help")
        {
            Report("Clear����;\nFirst�ı�Ĭ������˳��;\nPattern�ı�Ĭ������ͼ��;\nStart�ɿ���̨��ʼ��Ϸ;\nReload�ɿ���̨������Ϸ;");
            return;
        }
        if (command == "Clear")
        {
            consoleWindow.text = "Clear Succeed";
            return;
        }
        if (command == "First")
        {
            GameManager.Instance.defaultIsPlayerTurn = GameManager.Instance.defaultIsPlayerTurn ? false : true;
            Report("�޸ĳɹ�����һ����Ч");
            return;
        }
        if (command == "Pattern")
        {
            GameManager.Instance.defaultPattern = GameManager.Instance.defaultPattern == 0 ? 1 : 0;
            Report("�޸ĳɹ�����һ����Ч");
            return;
        }
        if (command == "Start")
        {
            if (GameManager.Instance.gameLogic.gameIsRunning)
            {
                Report("<color=red>��Ϸ��������</color>");
                return;
            }
            if (GameManager.Instance.gamePanel == null)
                GameManager.Instance.GameStart();
            else GameManager.Instance.gamePanel.StartGame();
            Report("���ɿ���̨��ʼ��Ϸ");
            return;
        }
        if (command == "Reload")
        {
            if (GameManager.Instance.gameLogic.gameIsRunning)
            {
                Report("<color=red>��Ϸ��������</color>");
                return;
            }
            if (GameManager.Instance.gamePanel == null)
                GameManager.Instance.GameReload();
            else GameManager.Instance.gamePanel.ReloadGame();
            Report("���ɿ���̨������Ϸ");
            return;
        }
        Report("<color=red>δָ֪�ʹ��Help��ѯ����ָ��</color>");
    }
}
