using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libs.Graph
{
    public abstract class GraphEdge
    {
        protected GraphNode m_enter;
        protected GraphNode m_exit;
        protected Condition m_condition;

        public abstract class Condition
        {
            public abstract bool Equals(Condition _condition);
        }

        public GraphEdge(GraphNode _enter, GraphNode _exit, Condition _condition)
        {
            m_enter = _enter;
            m_exit = _exit;
            m_condition = _condition;
        }

        public GraphNode GetExitNode()
        {
            return m_exit;
        }

        public bool Transition(Condition _condition, out GraphNode _outNode)
        {
            _outNode = m_enter;
            if (m_condition.Equals(_condition))
            {
                _outNode = m_exit;
                return true;
            }
            return false;
        }

        public abstract bool TransitionDefault(out GraphNode _outNode);
    }
}