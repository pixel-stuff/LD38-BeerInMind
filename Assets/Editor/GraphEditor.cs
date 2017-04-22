using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GraphEditor : EditorWindow
{

    List<Rect> m_windowRects = null;
    int m_currentId = 0;

    private string FilePath = "";
    private bool working = false;
    Character character = null;

    private Libs.Graph.Graph m_graph = null;

    [MenuItem("Window/Graph editor")]
    static void ShowEditor()
    {
        GraphEditor editor = EditorWindow.GetWindow<GraphEditor>();
        editor.Init();
    }

    public void Init()
    {
        //window1 = new Rect(10, 10, 100, 100);
        //window2 = new Rect(210, 210, 100, 100);
        working = false;
        m_windowRects = new List<Rect>();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //FilePath = EditorGUILayout.TextField("Text Field", FilePath);
        /*if (GUILayout.Button("Load", GUILayout.Width(100), GUILayout.Height(30)))
        {
            FilePath = EditorUtility.OpenFilePanel("Load", "", ".json");
            if (FilePath.Length != 0)
            {
                Debug.Log(working + " " + FilePath);
            }
        }
        if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(30)))
        {
            FilePath = EditorUtility.OpenFilePanel("Load", "", ".json");
            if (FilePath.Length != 0)
            {
                Debug.Log(working + " " + FilePath);
            }
        }*/
        character = (Character)EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Find Graph", character, typeof(Character), true);
        GUILayout.EndHorizontal();
        //DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows

        BeginWindows();
        //window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
        //window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
        if(character!=null && character.m_graph==null)
        {
            Node startNode = new Node();
            character.m_graph = new Libs.Graph.Graph(startNode);
        }
        m_graph = character.m_graph;
        DrawGraph(character.m_graph.GetCurrentNode(), MakeNode((Node)character.m_graph.GetCurrentNode()));
        EndWindows();

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.ContextClick)
        {
            Vector2 mousePos = currentEvent.mousePosition;
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("CreateNode"), false, CreateNode, "item 1");
            menu.AddSeparator("");
            menu.ShowAsContext();
            currentEvent.Use();
        }
    }

    private void DrawGraph(Libs.Graph.GraphNode _node, Rect _rect)
    {
        Node currentNode = (Node)_node;
        foreach(Edge e in currentNode.Edges)
        {
            Node n = (Node)e.GetExitNode();
            Rect window = MakeNode(n);
            DrawNodeCurve(_rect, window); // Here the curve is drawn under the windows

            DrawGraph(e.GetExitNode(), window);
        }
    }

    void CreateNode(object obj)
    {
        Debug.Log("Selected: " + obj);
    }

    private Rect MakeNode(Node n)
    {
        Rect window = new Rect(10, 50, 100, 100);
        m_windowRects.Add(window);
        return GUI.Window(m_currentId++, window, DrawNodeWindow, n.ToString());
    }

    void DrawNodeWindow(int id)
    {
        GUI.DragWindow();
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}