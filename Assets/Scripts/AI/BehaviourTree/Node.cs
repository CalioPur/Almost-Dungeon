using System.Collections.Generic;

namespace BehaviourTree
{
    public enum NodeState
    {
        Running,
        Success,
        Failure,
        Abort
    }
    public abstract class Node
    {
        public Node Parent;
        
        protected NodeState state;
        protected Node[] children;

        private Dictionary<string, object> data = new ();

        protected Node()
        {
            Parent = null;
        }

        protected Node(params Node[] _children)
        {
            int cpt = -1;
            children = new Node[_children.Length];
            foreach (Node child in _children)
            {
                AttachNode(child, ++cpt);
            }
        }

        private void AttachNode(Node node, int index)
        {
            node.Parent = this;
            children[index] = node;
        }

        public abstract NodeState Evaluate(Node root);

        public void SetDataInBlackboard(string key, object value)
        {
            Node node = Parent;
            while (node != null)
            {
                if (node.Parent == null)
                {
                    node.data[key] = value;
                    return;
                }
                node = node.Parent;
            }
        }

        protected T GetData<T>(string key) where T : class
        {
            Node node = Parent;
            while (node != null)
            {
                if (node.Parent == null)
                {
                    object value;
                    if (node.data.TryGetValue(key, out value))
                        return (T)value;
                    return null;
                }
                node = node.Parent;
            }
            return null;
        }

        protected object GetData(string key)
        {
            Node node = Parent;
            while (node != null)
            {
                if (node.Parent == null)
                {
                    object value;
                    if (node.data.TryGetValue(key, out value))
                        return value;
                    return null;
                }

                node = node.Parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (data.ContainsKey(key))
            {
                data.Remove(key);
                return true;
            }

            Node node = Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.Parent;
            }

            return false;
        }
        
        public bool ClearAllData()
        {
            if (Parent == null)
            {
                data.Clear();
                return true;
            }
            
            Node node = this;
            while (node.Parent != null)
            {
                node = node.Parent;
            }
            
            data.Clear();
            return true;
        }
        
    }
}

