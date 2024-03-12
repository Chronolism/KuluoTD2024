using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    /// <summary>
    /// 在被创建时赋予，逻辑母体
    /// </summary>
    public GameLogic logic;
    /// <summary>
    /// 格子编号
    /// </summary>
    public int index;
    /// <summary>
    /// 是否被填满
    /// </summary>
    public bool isFilled => icon == -1 ? false : true;
    /// <summary>
    /// 种类，因为只有双方游戏，这里没有使用枚举，使用0为cross（叉），1为circle（圈）,-1为空
    /// 应为存在动画播放时间，使用时是及时的，但如果改变时应是由内部处理动画逻辑
    /// </summary>
    public int icon = -1;
    /// <summary>
    /// 当前鼠标是否在此区域
    /// </summary>
    public bool ifPointIn = false;
    //本物体的Image组件
    Image m_Image;
    //画面上的icon
    int m_displayIcon = -1;
    //美术资源
    public Sprite cross_Icon;
    public Sprite circle_Icon;
    //便携处理透明度的方式
    CanvasGroup m_canvasGroup;
  

    //加载与注册
    void Awake()
    {
        if (!this.TryGetComponent<Image>(out m_Image)) Debug.LogError("Slot脚本挂载在了一个不具有Image组件的物体上");
        if (m_canvasGroup == null) m_canvasGroup = this.AddComponent<CanvasGroup>();
        m_canvasGroup.alpha = 0;
        cross_Icon = Resources.Load<Sprite>("ArtRes/Cross");
        circle_Icon = Resources.Load<Sprite>("ArtRes/Circle");
    }

    //输入接收与动画播放
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
