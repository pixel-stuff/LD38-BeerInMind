using System;
using Libs.Graph;

public class Node : GraphNode
{
    public enum eTextMiniType
    {
        DEFAULT,
        CHARACTERENTRY,
        CHARACTEREXIT,
        BEER,
        DRINK,
        WAIT,
        TV,
        NOTV,
        FIRE,
        ENDOFTHEDAY,
        HYGIENA,
        MINOR,
        DRUNKGUY,
        FIGHT
    }

    public enum eMood
    {
        DEFAULT,
        HAPPY,
        INFIRE
    }
    public uint m_day;
    public uint m_hour;
    public uint m_minut;
    private UInt32 m_nTicksDuration;
    private string m_text;
    private string m_minitext;
    private eTextMiniType m_eTextMiniType;
    private eMood m_eMood;

    public Node()
    {
        m_nTicksDuration = 1;
        m_text = "";
    }

    public Node(
        uint _day,
        uint _hour,
        uint _minut,
        UInt32 _nTicks,
        string _text,
        string _minitext,
        eTextMiniType _eTextMiniType,
        eMood _eMood)
    {
        m_day = _day;
        m_hour = _hour;
        m_minut = _minut;
        m_nTicksDuration = _nTicks;
        m_text = _text;
        m_minitext = _minitext;
        m_eTextMiniType = _eTextMiniType;
        m_eMood = _eMood;
    }

    public override void OnEnter()
    {
        throw new NotImplementedException();
    }
    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return m_nTicksDuration + " " + m_text;
    }
}
