using UnityEngine;
using UnityEditor;
 
public class GraphEditor : EditorWindow
{

    Rect window1;
    Rect window2;

    private string FilePath = "";
    private bool working = false;
    Character character = null;

    [MenuItem("Window/Graph editor")]
    static void ShowEditor()
    {
        GraphEditor editor = EditorWindow.GetWindow<GraphEditor>();
        editor.Init();
    }

    public void Init()
    {
        window1 = new Rect(10, 10, 100, 100);
        window2 = new Rect(210, 210, 100, 100);
        working = false;
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //FilePath = EditorGUILayout.TextField("Text Field", FilePath);
        if (GUILayout.Button("Load", GUILayout.Width(100), GUILayout.Height(30)))
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
        }
        character = (Character)EditorGUI.ObjectField(new Rect(3, 3, position.width - 6, 20), "Find Graph", character, typeof(Character), true);
        GUILayout.EndHorizontal();
        DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows

        BeginWindows();
        window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
        window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
        EndWindows();
    }

    private void DrawGraph(Libs.Graph.GraphNode _node)
    {
        Node currentNode = (Node)_node;
        foreach(Edge e in currentNode.Edges)
        {
            e.GetExitNode();
            DrawGraph(e.GetExitNode());
        }
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