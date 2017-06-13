using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Wangz.IFly;

public class Test : MonoBehaviour
{
    public IFlyBase m_ifly;
    public Button m_init;
    public Button m_start;
    public Button m_stop;

    // Use this for initialization
    void Start()
    {
        m_init.onClick.AddListener(() =>
        {
            m_ifly.Init();
        });
        m_start.onClick.AddListener(() =>
        {
            //m_ifly.Init();
            m_ifly.StartSpeech();
            m_ifly.StopSpeech();
        });
        m_stop.onClick.AddListener(() =>
        {
            m_ifly.StopSpeech();
        });
    }
}
