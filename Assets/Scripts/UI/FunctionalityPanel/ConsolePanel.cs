using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsolePanel : MonoBehaviour
{
    //左侧便携功能按钮
    public Button lastStepButton;
    public Button showNowGameSetDataButton;
    public Button gameStopButton;
    Text gameStopButtonText;
    public Button exportDataButton;
    //右侧控制台
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

    //在控制台Window打印内容 
    public void Report(string contains)
    {
        consoleWindow.text += "\n" + contains;
    }
    //由于尽可能需要减少对主逻辑的耦合，这里通过单例实现
    void StopAndContinueGame()
    {
        if(GameManager.Instance.gameLogic != null)
        {
           if(GameManager.Instance.gameLogic.gameIsRunning)
           {
                GameManager.Instance.gameLogic.gameIsRunning = false;
                gameStopButtonText.text = "<color=blue>暂停中</color>";

           }
           else
           {
                GameManager.Instance.gameLogic.gameIsRunning = true;
                gameStopButtonText.text = "游戏暂停";
           }
        }
    }
    //指令集
    public void Command(string command)
    {
        if (command == "Help")
        {
            Report("Clear清屏;\nFirst改变默认先手顺序;\nPattern改变默认先手图案;\nStart由控制台开始游戏;\nReload由控制台重启游戏;");
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
            Report("修改成功，下一局生效");
            return;
        }
        if (command == "Pattern")
        {
            GameManager.Instance.defaultPattern = GameManager.Instance.defaultPattern == 0 ? 1 : 0;
            Report("修改成功，下一局生效");
            return;
        }
        if (command == "Start")
        {
            if (GameManager.Instance.gameLogic.gameIsRunning)
            {
                Report("<color=red>游戏已在运行</color>");
                return;
            }
            if (GameManager.Instance.gamePanel == null)
                GameManager.Instance.GameStart();
            else GameManager.Instance.gamePanel.StartGame();
            Report("已由控制台开始游戏");
            return;
        }
        if (command == "Reload")
        {
            if (GameManager.Instance.gameLogic.gameIsRunning)
            {
                Report("<color=red>游戏已在运行</color>");
                return;
            }
            if (GameManager.Instance.gamePanel == null)
                GameManager.Instance.GameReload();
            else GameManager.Instance.gamePanel.ReloadGame();
            Report("已由控制台重启游戏");
            return;
        }
        Report("<color=red>未知指令，使用Help查询可用指令</color>");
    }
}
