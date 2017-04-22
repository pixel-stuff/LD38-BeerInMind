using System.Collections;
using System.Collections.Generic;

namespace Libs.Graph
{
    public class Graph
    {

        GraphNode m_startNode;
        GraphNode m_currentNode;

        // Use this for initialization
        public Graph(GraphNode startNode)
        {
            m_startNode = startNode;
            m_currentNode = startNode;
        }

        public GraphNode GetCurrentNode()
        {
            return m_currentNode;
        }

        public void Transition(GraphEdge _edge, GraphEdge.Condition _condition)
        {
            GraphNode nextNode = m_currentNode;
            foreach (GraphEdge e in m_currentNode.Edges)
            {
                nextNode = e.Transition(_condition);
            }
        }
    }
}