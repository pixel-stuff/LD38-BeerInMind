using System;
using Libs.Graph;

public class Node : GraphNode
{
    private DateTime m_startTime;
    private UInt32 m_nTicksDuration;
    private string m_text;

    public Node()
    {
        m_startTime = DateTime.Now;
        m_nTicksDuration = 1;
        m_text = "";
    }

    public Node(DateTime _time, UInt32 _nTicks, string _text)
    {
        m_startTime = _time;
        m_nTicksDuration = _nTicks;
        m_text = _text;
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
        return m_startTime.ToLongDateString() + " " + m_nTicksDuration + " " + m_text;
    }
}
