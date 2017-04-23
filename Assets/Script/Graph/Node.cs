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
        TVON,
        TVOFF,
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
    private int m_day;
    private int m_hour;
    private int m_minut;
    private int m_nTicksDuration;
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
        int _day,
        int _hour,
        int _minut,
        int _nTicks,
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

    public int GetDay()
    {
        return m_day;
    }

    public int GetHour()
    {
        return m_hour;
    }

    public int GetMinut()
    {
        return m_minut;
    }

    public int GetTicksDuration()
    {
        return m_nTicksDuration;
    }

    public string GetText()
    {
        return m_text;
    }

    public string GetMiniText()
    {
        return m_minitext;
    }

    public eTextMiniType GetTextMiniType()
    {
        return m_eTextMiniType;
    }

    public eMood GetMood()
    {
        return m_eMood;
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
