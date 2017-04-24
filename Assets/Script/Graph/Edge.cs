using System;
using Libs.Graph;

public class Edge : GraphEdge
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class Condition : GraphEdge.Condition
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    {
        [Flags]
        public enum ENUM
        {
            OPENING,
            BEER,
            DOOR,
            KEYS,
            PHONE_POLICE,
            PHONE_TAXI,
            BARCLOSED,
            MINOR,
            FIRESTARTING,
            ENDOFTHEDAY,
            BASEBALLBAT,
            JUKEBOX,
            TV,
            TVOFF,
            FREEBEER,
            OTHER,
            TIMEOUT,
            DEFAULT
        }
        private ENUM m_condition;
        public ENUM Value { get { return m_condition; } set { } }

        private string m_text;
        public string Text { get { return m_text; } set { } }

        public Condition(ENUM _condition)
        {
            m_condition = _condition;
            m_text = "";
        }

        public Condition(ENUM _condition, string _text)
        {
            m_condition = _condition;
            m_text = _text;
        }

        public override bool Equals(GraphEdge.Condition _condition)
        {
            Condition condition = (Condition)_condition;
            if (condition.Value.Equals(Condition.ENUM.OTHER))
            {
                return Text.Equals(condition.Text);
            }
            return m_condition == ((Edge.Condition)_condition).Value;
        }
    }

    private string m_label;
    public string Text { get { return m_label; } set { } }

    public Edge(GraphNode _enter, GraphNode _exit, Condition _condition, string _label) : base(_enter, _exit, _condition)
    {
        m_label = _label;
    }

    public override bool TransitionDefault(out GraphNode _outNode)
    {
        if (m_condition.Equals(Condition.ENUM.DEFAULT))
        {
            _outNode = m_exit;
            return true;
        }
        _outNode = m_enter;
        return false;
    }
}
