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
    private void Awake()
    {
        m_startAndReloadText = startAndReloadButton.GetComponentInChildren<Text>();
        m_startAndReloadText.text = "����˴���ʼ��Ϸ";
        gameTip.text = "ESC���˳���Ϸ ~������&���ؿ���̨";
        gameResult.text = "";
        startAndReloadButton.onClick.AddListener(StartGame);
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
    public void EndGame()
    {
        startAndReloadButton.gameObject.SetActive(true);
    }
    public void ReloadGame()
    {
        GameManager.Instance.GameReload();
        startAndReloadButton.onClick.RemoveAllListeners();
        startAndReloadButton.onClick.AddListener(StartGame);
        m_startAndReloadText.text = "����˴���ʼ��Ϸ";
        gameTip.text = "ESC���˳���Ϸ ~������&���ؿ���̨";
        gameResult.text = "";

    }
}
