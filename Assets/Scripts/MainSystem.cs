using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [Header("���û�����Ϸ�߼�")]
    public bool enableBaseLogic;
    [Header("Ĭ���Ƿ�Ϊ�������")]
    public bool playerFirst;
    [Header("����ʹ�õ�ͼ��")]
    public E_Pattern firstPattern;
    [Header("ʹ�õĵ���AI����")]
    public AILogicBase optionalAI;
    [Header("������ϷUI���")]
    public bool enableGamePanel;
    [Header("���ÿ���̨")]
    public bool enableConsole;
    [Header("���ò�������")]
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
    ���_0��, ��Ȧ_1��
}