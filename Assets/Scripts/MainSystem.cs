using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [Header("启用基本游戏逻辑")]
    public bool enableBaseLogic;
    [Header("默认是否为玩家先手")]
    public bool playerFirst;
    [Header("先手使用的图案")]
    public E_Pattern firstPattern;
    [Header("使用的电脑AI配置")]
    public AILogicBase optionalAI;
    [Header("启用游戏UI面板")]
    public bool enableGamePanel;
    [Header("启用控制台")]
    public bool enableConsole;
    [Header("启用测试中心")]
    public bool enableQACentre;
    //Canvas
    GameObject m_CanvasGameObject;
    void Awake()
    {
        m_CanvasGameObject = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/Canvas"));
        Instantiate<GameObject>(optionalAI.gameObject, m_CanvasGameObject.transform);
        GameManager.Instance.defaultIsPlayerTurn = playerFirst;
        GameManager.Instance.defaultPattern = (int)firstPattern;
        if (enableBaseLogic) GameManager.Instance.gameLogic = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/GameLogic"), m_CanvasGameObject.transform).GetComponent<GameLogic>();
        if (enableGamePanel) GameManager.Instance.gamePanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/GamePanel"), m_CanvasGameObject.transform).GetComponent<GamePanel>();
        if (enableConsole) GameManager.Instance.consolePanel = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/ConsolePanel"), m_CanvasGameObject.transform).GetComponent<ConsolePanel>();
        if (enableQACentre) GameManager.Instance.qACentre = Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/QACentre"), m_CanvasGameObject.transform).GetComponent<QACentre>();
        
    }

}
public enum E_Pattern
{
    红叉_0号, 绿圈_1号
}