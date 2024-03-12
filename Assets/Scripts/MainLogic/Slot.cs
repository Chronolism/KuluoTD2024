using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    /// <summary>
    /// �ڱ�����ʱ���裬�߼�ĸ��
    /// </summary>
    public GameLogic logic;
    /// <summary>
    /// ���ӱ��
    /// </summary>
    public int index;
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    public bool isFilled => icon == -1 ? false : true;
    /// <summary>
    /// ���࣬��Ϊֻ��˫����Ϸ������û��ʹ��ö�٣�ʹ��0Ϊcross���棩��1Ϊcircle��Ȧ��,-1Ϊ��
    /// ӦΪ���ڶ�������ʱ�䣬ʹ��ʱ�Ǽ�ʱ�ģ�������ı�ʱӦ�����ڲ��������߼�
    /// </summary>
    public int icon = -1;
    /// <summary>
    /// ��ǰ����Ƿ��ڴ�����
    /// </summary>
    public bool ifPointIn = false;
    //�������Image���
    Image m_Image;
    //�����ϵ�icon
    int m_displayIcon = -1;
    //������Դ
    public Sprite cross_Icon;
    public Sprite circle_Icon;
    //��Я����͸���ȵķ�ʽ
    CanvasGroup m_canvasGroup;
  

    //������ע��
    void Awake()
    {
        if (!this.TryGetComponent<Image>(out m_Image)) Debug.LogError("Slot�ű���������һ��������Image�����������");
        if (m_canvasGroup == null) m_canvasGroup = this.AddComponent<CanvasGroup>();
        m_canvasGroup.alpha = 0;
        cross_Icon = Resources.Load<Sprite>("ArtRes/Cross");
        circle_Icon = Resources.Load<Sprite>("ArtRes/Circle");
    }

    //��������붯������
    void Update()
    {
        if(icon != m_displayIcon)
        {
            switch (icon)
            {
                case -1:
                    m_canvasGroup.alpha = 0;
                    m_Image.sprite = null;
                    break;
                case 0:
                    m_canvasGroup.alpha = 1;
                    m_Image.sprite = cross_Icon;
                    break;
                case 1:
                    m_canvasGroup.alpha = 1;
                    m_Image.sprite = circle_Icon;
                    break;
            }
            m_displayIcon = icon;
        }
        if(logic.isPlayerTurn && Input.GetMouseButtonDown(0) && ifPointIn)
        {
            if (!isFilled) logic.Set(index);
  
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isFilled)
        {
            m_canvasGroup.alpha = 0;
            m_Image.sprite = null;
        }
        ifPointIn = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isFilled)
        {
            m_canvasGroup.alpha = 0.5f;
            m_Image.sprite = (logic.nowPattern == 0 ? cross_Icon : circle_Icon);
        }
        ifPointIn = true;
    }
   
}
