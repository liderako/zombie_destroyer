using Pathfinding;
using UnityEngine;

namespace CoreGrid
{
    public interface ICellComponent
    {
        public GraphNode Node { get; set; }
        public Vector3 Position { get; }
        public int ArrayIndex { get; set; }
        public bool IsWalkableNode { get; }
        public void ChangeStatusWalkableNode(bool status);
        
        public GameObject GameObject { get; }
        
        public void UpdateVisibility(bool status);
    }
}