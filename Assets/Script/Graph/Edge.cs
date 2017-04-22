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
            BEERLIGHT,
            BEERBROWN,
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
}
