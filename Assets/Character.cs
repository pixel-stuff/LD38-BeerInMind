using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour {

	public EditorNode m_startNode;
	// Use this for initialization
	void Start ()
    {
        Node node = new Node(System.DateTime.Now,
                            (System.UInt32)m_startNode.lifetime,
                            m_startNode.text);
        print(m_startNode.text);
        CreateNode(node, m_startNode);
        Libs.Graph.Graph graph = new Libs.Graph.Graph(node);

        List<Edge.Condition> listConditions = new List<Edge.Condition>();
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.OPENING));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.BEERLIGHT));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.BEERBROWN));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.DOOR));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.KEYS));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.PHONE_POLICE));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.PHONE_TAXI));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.BARCLOSED));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.MINOR));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.FIRESTARTING));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.ENDOFTHEDAY));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.BASEBALLBAT));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.TV));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.FREEBEER));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.OTHER));
        listConditions.Add(new Edge.Condition(Edge.Condition.ENUM.DEFAULT));

        PrintGraph(graph.GetCurrentNode(), listConditions);
    }

    void CreateNode(Node _node, EditorNode _eNode)
    {
        EditorNode currentNode = _eNode;
        foreach (EditorEdge c in currentNode.edges)
        {
            EditorNode transition = c.targetNode;
            Node node = new Node(System.DateTime.Now,
                                (System.UInt32)transition.lifetime,
                                transition.text);
            Edge.Condition condition = new Edge.Condition(c.condition);
            Edge edge = new Edge(_node, node, condition);
            _node.Edges.Add(edge);
            CreateNode(node, c.targetNode);
        }
    }

    private void PrintGraph(Libs.Graph.GraphNode _node, List<Edge.Condition> _conditions)
    {
        Libs.Graph.GraphNode currentNode = _node;
        print(currentNode.ToString());
        foreach (Edge.Condition c in _conditions)
        {
            Libs.Graph.GraphNode transition = currentNode.Transition(c);
            if (transition != currentNode)
            {
                PrintGraph(transition, _conditions);
            }
        }
    }
}
