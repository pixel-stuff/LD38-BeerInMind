using System.Collections;
using System.Collections.Generic;

namespace Libs.Graph
{
    public abstract class GraphNode
    {

        private List<GraphEdge> m_edges;
        public List<GraphEdge> Edges { get { return m_edges; } set { } }

        public GraphNode()
        {
            m_edges = new List<GraphEdge>();
        }

        public abstract void OnEnter();
        public abstract void OnExit();

        public GraphNode Transition(GraphEdge.Condition _condition)
        {
            GraphNode currNode = this;
            foreach(GraphEdge e in m_edges)
            {
                GraphNode transittedNode = currNode;
                if (e.Transition(_condition, out transittedNode))
                {
                    return transittedNode;
                }
            }
            return currNode;
        }

        public List<GraphNode> ToList()
        {
            List<GraphNode> list = new List<GraphNode>();
            list.Add(this);
            foreach (GraphEdge e in Edges)
            {
                list.AddRange(e.GetExitNode().ToList());
            }
            return list;
        }
    }
}