using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Extensions;

namespace Configs.Items
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/Create ItemConfig")]
    public class ItemConfigBase : ScriptableObject
    {
        [System.Serializable]
        private struct Visual
        {
            [OnValueChanged(nameof(OnMeshRendererChanged))]
            public MeshRenderer prefab;

            [ReadOnly] public Mesh sharedMesh;
            [ReadOnly] public Vector3 meshSize;

            public void OnMeshRendererChanged()
            {
                sharedMesh = prefab.GetComponent<MeshFilter>().sharedMesh;
                meshSize = sharedMesh.bounds.size;
            }
        }


        [Space]
        [PreviewField(ObjectFieldAlignment.Left)]
        [SerializeField]
        private Sprite icon;

        [SerializeField] private Visual[] visuals;
        public Sprite Icon => icon;

        [SerializeField] private float ConstStepY;

        public Vector3 MaxRotateForEveryCoordinates;
        public Vector3 originRotate;
        
        public void GetMeshData(out Mesh mesh, out Material[] materials, out float height)
        {
            ref readonly Visual random = ref visuals[visuals.GetRandomIndex()];
            mesh = random.sharedMesh;
            materials = random.prefab.sharedMaterials;
            if (ConstStepY != 0)
            {
                height = ConstStepY;
            }
            else
            {
                height = random.meshSize.y;   
            }
        }


#if UNITY_EDITOR
        [ShowIf(nameof(HasVisual))]
        [Button]
        private void SetNameAsPrefab()
        {
            UnityEditor.AssetDatabase.RenameAsset(UnityEditor.AssetDatabase.GetAssetPath(this), visuals[0].prefab.name);
            this.Dirty();
        }

        private bool HasVisual => visuals != null && visuals.Length > 0;

        [Button]
        private void Validate()
        {
            for (int i = 0; i < visuals.Length; i++)
            {
                ref Visual visual = ref visuals[i];
                visual.OnMeshRendererChanged();
            }
        }
#endif
    }
}