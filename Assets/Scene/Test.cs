using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Wangz.IFly;

public class Test : MonoBehaviour
{
    public IFlyBase m_ifly;
    public Button m_start;
    public Button m_stop;

    // Use this for initialization
    void Start()
    {
        m_start.onClick.AddListener(() =>
        {
            m_ifly.StartSpeech(3);
        });
        m_stop.onClick.AddListener(() =>
        {
            m_ifly.StopSpeech();
        });
    }
}
