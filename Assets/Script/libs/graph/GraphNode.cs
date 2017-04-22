using System.Collections;
using System.Collections.Generic;

namespace Libs.Graph
{
    public abstract class GraphNode
    {

        private List<GraphEdge> m_edges;
        public List<GraphEdge> Edges { get { return m_edges; } set { } }

        public abstract void OnEnter();
        public abstract void OnExit();
    }
}