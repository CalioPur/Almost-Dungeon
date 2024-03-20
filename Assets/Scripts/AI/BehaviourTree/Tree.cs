using System;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {

        protected Node origin = null;


        private void Awake()
        {
            OnStart();
        }

        private void OnStart()
        {
            origin = InitTree();
        }

        private void Update()
        {
            // if (origin != null)
            //     origin.Evaluate(origin);
        }
        
        public Node getOrigin()
        {
            return origin;
        }

        public void ResetBlackboard()
        {
            origin?.ClearAllData();
            origin?.SetDataInBlackboard("Init", true);
        }

        protected abstract Node InitTree();
    }
}