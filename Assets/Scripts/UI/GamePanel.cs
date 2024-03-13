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
        m_startAndReloadText.text = "����˴���ʼ��Ϸ";
        gameTip.text = "ESC���˳���Ϸ ~������&���ؿ���̨";
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
        m_startAndReloadText.text = "����˴��ٴ���Ϸ";
        gameTip.text = (GameLogic.Instance.isPlayerTurn == true ? "��һغ�" : "���Իغ�");
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
        m_startAndReloadText.text = "����˴���ʼ��Ϸ";
        gameTip.text = "ESC���˳���Ϸ ~������&���ؿ���̨";
        gameResult.text = "";
        gameWinWay.gameObject.SetActive(false);
    }
}
