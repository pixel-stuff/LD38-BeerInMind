using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Libs.Graph
{
    [System.Serializable]
    public class JSONNode
    {
        public string id;
        public string label;
        public string title;
        public string day;
        public string hourminut;
        public string lifetime;
        public string text;
        public string minitext;
        public string textminitype;
        public string mood;
    }
    [System.Serializable]
    public class JSONEdge
    {
        public string from;
        public string to;
        public string type;
        public string arrows;
        public string label;
        public bool processed = false;
    }
    [System.Serializable]
    public class JSONGraph
    {
        public List<JSONNode> nodes;
        public List<JSONEdge> edges;
        public JSONGraph()
        {
            nodes = new List<JSONNode>();
            edges = new List<JSONEdge>();
        }

        public JSONNode GetRoot()
        {
            foreach (JSONNode n in nodes)
            {
                bool isStartID = true;
                foreach (JSONEdge e in edges)
                {
                    if (n.id == e.to)
                    {
                        isStartID = false;
                    }
                }
                if (isStartID)
                {
                    return n;
                }
            }
            return null;
        }

        public List<JSONEdge> GetEdgesFromNode(JSONNode _node)
        {
            List<JSONEdge> rEdges = new List<JSONEdge>();
            foreach (JSONEdge e in edges)
            {
                if (_node.id == e.from && !e.processed)
                {
                    rEdges.Add(e);
                }
            }
            return rEdges;
        }

        public JSONNode GetNodeFromID(string _id)
        {
            foreach (JSONNode n in nodes)
            {
                if (n.id == _id)
                {
                    return n;
                }
            }
            return null;
        }
    }
}
