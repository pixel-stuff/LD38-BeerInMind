using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Libs.Graph;

public class Character : MonoBehaviour {

	public WhisperTalkManager m_whisperTalk;

	public EditorNode m_startNode;
	public Libs.Graph.Graph currentGraph;
	public Node currentNode;
	public bool isOnBar = false;
	public Vector3 finalPlace;

    Character()
    {
        currentGraph = new Libs.Graph.Graph(new Node());
    }
	// Use this for initialization
	void Start ()
	{
		Node node = new Node(System.DateTime.Now,
			(System.UInt32)m_startNode.lifetime,
			m_startNode.text);
		print(m_startNode.text);
		CreateNode(node, m_startNode);
		currentGraph = new Libs.Graph.Graph(node);

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

		PrintGraph(currentGraph.GetCurrentNode(), listConditions);
		currentNode = null;
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

	void Update(){
		if (currentNode != (Node)currentGraph.GetCurrentNode()) {
			// Node non changer

			
		} else {
			currentNode = (Node)currentGraph.GetCurrentNode ();
			if (!isOnBar) {
				
				// Appear On Door
				return;

			}
		}
	}

	void DisplayWhisper(string text){
		//CALL BUBULE MANAGER
		m_whisperTalk.StartDisplayWhisper(text);
	}

	void WhisperClick(){
		//CALL First view discussion
	}

	public void OnCharacEnter(){
		//PLAY DING DING SOUND
	}

	public void OnEnterFinished(){
		this.gameObject.transform.position = finalPlace;
	}
}