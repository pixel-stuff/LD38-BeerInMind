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
            FREEBEER,
            OTHER,
            TIMEOUT,
            DEFAULT
        }
        private ENUM m_condition;
        public ENUM Value { get { return m_condition; } set { } }
        public Condition(ENUM _condition)
        {
            m_condition = _condition;
        }

        public override bool Equals(GraphEdge.Condition _condition)
        {
            return m_condition == ((Edge.Condition)_condition).Value;
        }
    }

    public Edge(GraphNode _enter, GraphNode _exit, Condition _condition) : base(_enter, _exit, _condition)
    {
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
