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
    List<Libs.Graph.GraphNode> m_nodes = null;
    List<Libs.Graph.GraphEdge> m_edges = null;

    private bool m_editing = false;
    private bool m_drawingEdge = false;
    private Rect m_drawingEdgeFrom;

    [MenuItem("Window/Graph editor")]
    static void ShowEditor()
    {
        GraphEditor editor = EditorWindow.GetWindow<GraphEditor>();
        editor.Init();
    }

    public GraphEditor()
    {
        m_windowRects = new List<Rect>();
        m_nodes = new List<Libs.Graph.GraphNode>();
        m_edges = new List<Libs.Graph.GraphEdge>();
    }

    public void Init()
    {
        //window1 = new Rect(10, 10, 100, 100);
        //window2 = new Rect(210, 210, 100, 100);
        working = false;
    }

    void OnGUI()
    {
        m_editing = character != null;
        if(!m_editing)
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
            if (character!=null && m_graph == null)
            {
                m_graph = character.currentGraph;
                m_nodes.AddRange(m_graph.ToList());
            }
        }
        else
        {
            BeginWindows();
            //window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
            //window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
            /*if(character!=null && character.currentGraph==null)
            {
                Node startNode = new Node();
                character.currentGraph = new Libs.Graph.Graph(startNode);
            }
            if(character != null && m_graph == null)
            {
                m_graph = character.currentGraph;
            }*/
            int id = 0;
            if(m_windowRects==null)
            {
                m_windowRects = new List<Rect>();
            }
            if (m_nodes == null)
            {
                m_nodes = new List<Libs.Graph.GraphNode>();
            }
            for (int i = m_windowRects.Count;i<m_nodes.Count;i++)
            {
                m_windowRects.Add(new Rect(10, 10, 100, 100));
            }
            foreach(Node n in m_nodes)
            {
                if(m_drawingEdge && m_windowRects[id]==m_drawingEdgeFrom)
                {
                    m_windowRects[id] = GUI.Window(id, m_windowRects[id], DrawFixedDraggableNodeWindow, n.ToString());
                }
                else
                {
                    m_windowRects[id] = GUI.Window(id, m_windowRects[id], DrawDraggableNodeWindow, n.ToString());
                }

                id++;
            }
                //DrawGraph(m_graph.GetCurrentNode(), MakeNode((Node)m_graph.GetCurrentNode(), new Vector2(10, 50)));
            EndWindows();

            UnityEngine.Event currentEvent = UnityEngine.Event.current;

            if (m_drawingEdge && m_drawingEdgeFrom!=null)
            {
                DrawNodeCurve(m_drawingEdgeFrom, new Rect(currentEvent.mousePosition.x, currentEvent.mousePosition.y, 1, 1));
            }
            if (currentEvent.type == EventType.ContextClick)
            {
                m_drawingEdge = false;
                int idr = -1;
                for(int i = 0;i< m_windowRects.Count;i++)
                {
                    if (m_windowRects[i].Contains(currentEvent.mousePosition))
                    {
                        idr = i;
                    }
                }
                Debug.Log(idr);
                Vector2 mousePos = currentEvent.mousePosition;
                // Now create the menu, add items and show it
                GenericMenu menu = new GenericMenu();
                if(idr<0)
                {
                    menu.AddItem(new GUIContent("CreateNode"), false, CreateNode, mousePos);
                }
                else
                {
                    menu.AddItem(new GUIContent("LinkNode"), false, CreateEdge, (System.UInt32)idr);
                }
                menu.AddSeparator("");
                menu.ShowAsContext();
                currentEvent.Use();
            }
        }
    }

    /*private void DrawGraph(Libs.Graph.GraphNode _node, Rect _rect)
    {
        Node currentNode = (Node)_node;
        foreach(Edge e in currentNode.Edges)
        {
            Node n = (Node)e.GetExitNode();
            Rect window = MakeNode(n, new Vector2(10, 50));
            DrawNodeCurve(_rect, window); // Here the curve is drawn under the windows

            DrawGraph(e.GetExitNode(), window);
        }
    }*/

    void CreateNode(object obj)
    {
        Node n = new Node();
        Vector2 pos = (Vector2)obj;
        m_nodes.Add(n);
        m_windowRects.Add(new Rect(pos.x, pos.y, 100, 100));
    }

    void CreateEdge(object obj)
    {
        m_drawingEdge = true;
        m_drawingEdgeFrom = m_windowRects[(int)((System.UInt32)obj)];
    }

    private Rect MakeNode(Node n, Vector2 pos)
    {
        Rect window = new Rect(pos.x, pos.y, 100, 100);
        m_windowRects.Add(window);
        return GUI.Window(m_currentId++, window, DrawNodeWindow, n.ToString());
    }

    void DrawNodeWindow(int id)
    {
        UnityEngine.Event currentEvent = UnityEngine.Event.current;
        if (currentEvent.type == UnityEngine.EventType.MouseDown && currentEvent.button == 0)
        {
            int idr = -1;
            for (int i = 0; i < m_windowRects.Count; i++)
            {
                if (m_windowRects[i].Contains(currentEvent.mousePosition))
                {
                    idr = i;
                }
            }
            if (idr > -1)
            {
                m_drawingEdgeFrom = m_windowRects[idr];
                m_drawingEdge = true;
            }
        }
        /*if (currentEvent.type == UnityEngine.EventType.MouseDown && currentEvent.button == 0)
        {
            Debug.Log("should work");
        }*/
    }
    void DrawFixedDraggableNodeWindow(int id)
    {
        DrawNodeWindow(id);
    }

    void DrawDraggableNodeWindow(int id)
    {
        DrawNodeWindow(id);
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

    void OnDestroy()
    {
        m_editing = false;
        m_graph = null;
        character = null;
        Debug.Log("Closing window");
    }
}