using System.Collections.Generic;
using Pathfinding;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CoreGrid
{
    public class GeneratorGrid : MonoBehaviour
    {
        [SerializeField] private AstarPath astarPath;
#if UNITY_EDITOR
        [SerializeField] private GameObject prefabEditorCell;
#endif
        public List<ICellComponent> AllCells = new List<ICellComponent>();

        [Button]
        public void GenerateGrid()
        {
            ClearOldCells();
            CreateCellPrefabs();
        }
        
        private void ClearOldCells()
        {
            if (AllCells.Count != 0)
            {
                for (int i = 0; i < AllCells.Count; i++)
                {
                    if (AllCells[i] != null)
                    {
                        DestroyImmediate(AllCells[i].GameObject);
                    }
                }
                AllCells.Clear();
            }
        }
        
        private void CreateCellPrefabs()
        {
            var up = new Vector3(0, 0.1f, 0);
            AstarPath.active.Scan();
            var gridGraph = astarPath.graphs[0] as GridGraph;
            var path = ConstantPath.Construct(Vector3.zero,  (gridGraph.Depth * gridGraph.Width) * 1000 + 1);
            AstarPath.StartPath(path);
            path.BlockUntilCalculated();
            int i = 0;
#if UNITY_EDITOR
            if (prefabEditorCell == null)
            {
                Debug.LogError("Prefab Editor Cell is null. Please set it in the inspector.");
                return;
            }
            foreach (var node in path.allNodes)
            {
                var go = PrefabUtility.InstantiatePrefab(prefabEditorCell) as GameObject;
                go.transform.position = (Vector3)node.position + up;
                go.transform.parent = transform;
                go.name += $"-{i}";
                var cell = go.GetComponent<ICellComponent>();
                cell.Node = node;
                cell.ArrayIndex = i;
                AllCells.Add(cell);
                i += 1;
                EditorUtility.SetDirty(go);
            }
            EditorUtility.SetDirty(this);   
#endif
        }
    }
}