using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libs.Graph
{
    public abstract class GraphEdge
    {
        private GraphNode m_enter;
        private GraphNode m_exit;
        private Condition m_condition;

        public abstract class Condition
        {
            public abstract bool Test();
        }

        public GraphEdge(GraphNode _enter, GraphNode _exit)
        {
            m_enter = _enter;
            m_exit = _exit;
        }

        public GraphNode Transition(Condition _condition)
        {
            if (m_condition.Equals(_condition))
            {
                return m_exit;
            }
            return m_enter;
        }
    }
}