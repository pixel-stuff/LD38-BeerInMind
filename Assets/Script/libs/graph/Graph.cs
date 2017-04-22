using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Libs.Graph
{
    public class Graph
    {
        GraphNode m_currentNode;

        public delegate GraphNode CreateNode(JSONNode _node);
        public delegate GraphEdge CreateEdge(JSONEdge _edge, GraphNode _nodeFrom, GraphNode _nodeTo);

        public Graph(string filepath, CreateNode _cbCreateNode, CreateEdge _cbCreateEdge)
        {
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

        public void Transition(GraphEdge _edge, GraphEdge.Condition _condition)
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
                JSONNode nTo = _graph.GetNodeFromID(e.to);
                GraphNode newNode = _cbCreateNode(nTo);
                GraphEdge newEdge = _cbCreateEdge(e, currentNode, newNode);
                currentNode.Edges.Add(newEdge);
                JSONMakeNode(nTo, newNode, _graph, _cbCreateNode, _cbCreateEdge);
            }
        }

        public List<GraphNode> ToList()
        {
            return m_currentNode.ToList();
        }
    }
}