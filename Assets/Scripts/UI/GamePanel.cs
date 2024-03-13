using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    public Button startAndReloadButton;
    Text m_startAndReloadText;
    public Text gameTip;
    public Text gameResult;
    public Image gameWinWay;
    public List<Sprite> winWays = new List<Sprite>();
    private void Awake()
    {
        m_startAndReloadText = startAndReloadButton.GetComponentInChildren<Text>();
        m_startAndReloadText.text = "点击此处开始游戏";
        gameTip.text = "ESC键退出游戏 ~键唤起&隐藏控制台";
        gameResult.text = "";
        startAndReloadButton.onClick.AddListener(StartGame);
        gameWinWay.gameObject.SetActive(false);
    }
    public void StartGame()
    {
        GameManager.Instance.GameStart();
        startAndReloadButton.onClick.RemoveAllListeners();
        startAndReloadButton.onClick.AddListener(ReloadGame);
        startAndReloadButton.gameObject.SetActive(false);
        m_startAndReloadText.text = "点击此处再次游戏";
        gameTip.text = (GameLogic.Instance.isPlayerTurn == true ? "玩家回合" : "电脑回合");
    }
    public void EndGame(bool isRaw)
    {
        startAndReloadButton.gameObject.SetActive(true);
        if (isRaw) return;
        gameWinWay.sprite = winWays[(int)GameManager.Instance.gameLogic.winWay];
        gameWinWay.gameObject.SetActive(true);
    }
    public void ReloadGame()
    {
        GameManager.Instance.GameReload();
        startAndReloadButton.onClick.RemoveAllListeners();
        startAndReloadButton.onClick.AddListener(StartGame);
        m_startAndReloadText.text = "点击此处开始游戏";
        gameTip.text = "ESC键退出游戏 ~键唤起&隐藏控制台";
        gameResult.text = "";
        gameWinWay.gameObject.SetActive(false);
    }
}
