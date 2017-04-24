using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libs.Graph
{
    public class Graph
    {
        GraphNode m_currentNode;
        class Link
        {
            public string from;
            public string to;
            public GraphEdge instance;

            public Link(string _from, string _to, GraphEdge _instance)
            {
                from = _from;
                to = _to;
                instance = _instance;
            }
        }
        private Dictionary<string, GraphNode> m_nodeDictionary;
        private List<JSONEdge> m_linkDictionary;

        public delegate GraphNode CreateNode(JSONNode _node);
        public delegate GraphEdge CreateEdge(JSONEdge _edge, GraphNode _nodeFrom, GraphNode _nodeTo);

        public Graph(string filepath, CreateNode _cbCreateNode, CreateEdge _cbCreateEdge)
        {
            m_nodeDictionary = new Dictionary<string, GraphNode>();
            m_linkDictionary = new List<JSONEdge>();
            JSONParse(filepath, _cbCreateNode, _cbCreateEdge);
        }

        // Use this for initialization
        public Graph(GraphNode startNode)
        {
            m_currentNode = startNode;
        }

        public GraphNode GetCurrentNode()
        {
            return m_currentNode;
        }

        public void Transition(GraphEdge.Condition _condition)
        {
            GraphNode nextNode = m_currentNode;
            foreach (GraphEdge e in m_currentNode.Edges)
            {
                GraphNode transittedNode = nextNode;
                if(e.Transition(_condition, out transittedNode))
                {
                    nextNode = transittedNode;
                }
            }
			m_currentNode = nextNode;
        }

		public void Transition(string text)
		{
			GraphNode nextNode = m_currentNode;
			foreach (GraphEdge e in m_currentNode.Edges)
			{
				GraphNode transittedNode = nextNode;
				if(e.Transition(new Edge.Condition(Edge.Condition.ENUM.OTHER, text), out transittedNode))
				{
					nextNode = transittedNode;
				}
			}
			m_currentNode = nextNode;
		}

        public void JSONParse(string _filepath, CreateNode _cbCreateNode, CreateEdge _cbCreateEdge)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(_filepath, System.Text.Encoding.Default);
            string jsonData = reader.ReadToEnd();
            Debug.Log(jsonData);
            JSONGraph jsonGraph = JsonUtility.FromJson<JSONGraph>(jsonData);
            
            Debug.Log(jsonGraph.nodes.Count + " " + jsonGraph.nodes.Count);
            FillFromJSON(jsonGraph, _cbCreateNode, _cbCreateEdge);
        }

        public void FillFromJSON(JSONGraph _graph, CreateNode _cbCreateNode, CreateEdge _cbCreateEdge)
        {
            if (_cbCreateNode != null)
            {
                // looking for the starting ID
                JSONNode root = _graph.GetRoot();
                GraphNode startNode = _cbCreateNode(root);
                m_currentNode = startNode;
                JSONMakeNode(root, startNode, _graph, _cbCreateNode, _cbCreateEdge);
            }
        }

        void JSONMakeNode(JSONNode node, GraphNode currentNode, JSONGraph _graph, CreateNode _cbCreateNode, CreateEdge _cbCreateEdge)
        {
            foreach(JSONEdge e in _graph.GetEdgesFromNode(node))
            {
                if (!(m_linkDictionary.Exists(x => x.from == e.from && x.to == e.to && x.type == e.type && x.label.Equals(e.label))))
                {
                    JSONNode nTo = _graph.GetNodeFromID(e.to);
                    GraphNode newNode = null;
                    if (!m_nodeDictionary.TryGetValue(e.to, out newNode))
                    {
                        newNode = _cbCreateNode(nTo);
                        m_nodeDictionary.Add(e.to, newNode);
                    }
                    
                    GraphEdge newEdge = _cbCreateEdge(e, currentNode, newNode);
                    m_linkDictionary.Add(e);
                    currentNode.Edges.Add(newEdge);
                    e.processed = true;
                    JSONMakeNode(nTo, newNode, _graph, _cbCreateNode, _cbCreateEdge);
                }
            }
        }

        public List<GraphNode> ToList()
        {
            return m_currentNode.ToList();
        }
    }
}