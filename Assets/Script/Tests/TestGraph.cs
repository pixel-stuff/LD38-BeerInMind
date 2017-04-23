using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libs.Graph;

public class TestGraph : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {

        Node start = new Node(2, "Need a beer");
        Node child1 = new Node(2, "Thanks for the beer !");
        Node child2 = new Node(2, "FUCK YOU MOTHAFUCKA");

        Edge.Condition conditionBeer = new Edge.Condition(Edge.Condition.ENUM.BEERLIGHT);
        Edge.Condition conditionDoor = new Edge.Condition(Edge.Condition.ENUM.DOOR);

        Edge edge1 = new Edge(start, child1, conditionBeer);
        Edge edge2 = new Edge(start, child2, conditionDoor);

        start.Edges.Add(edge1);
        start.Edges.Add(edge2);

        Graph test = new Graph(start);

        List<Edge.Condition> listConditions = new List<Edge.Condition>();
        listConditions.Add(conditionBeer);
        listConditions.Add(conditionDoor);

        PrintGraph(test.GetCurrentNode(), listConditions);
	}

    private void PrintGraph(GraphNode _node, List<Edge.Condition> _conditions)
    {
        GraphNode currentNode = _node;
        print(currentNode.ToString());
        foreach (Edge.Condition c in _conditions)
        {
            GraphNode transition = currentNode.Transition(c);
            if(transition!=currentNode)
            {
                PrintGraph(transition, _conditions);
            }
        }
    }
}
