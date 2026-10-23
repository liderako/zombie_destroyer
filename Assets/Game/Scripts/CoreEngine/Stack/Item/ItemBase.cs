using Configs.Items;
using DG.Tweening;
using Gameplay.Stack.Interactions;
using Gameplay.Stack.Item.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Extensions;

namespace Gameplay.Stack.Item
{
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] protected MeshFilter meshFilter;
        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] private ItemMoverLocal moverLocal;
        [SerializeField] private ItemMoverTransform moverTransform;
        [SerializeField] private ItemAnim itemAnim;

        private IItemReceiver receiver;
        protected float height;

        public GameObject targetParent { get; set; }

        public GameObject MeshRenderObject => meshRenderer.gameObject;

        public Tween TweenScale { get; set; }
        
        public ItemAnim ItemAnim => itemAnim;
        public float Height => height;
        [ShowInInspector, ReadOnly] public ItemConfigBase Config { get; private set; }
        public bool Moving => moverTransform.enabled;

        public int Capacity;

        public virtual bool VisualEnabled
        {
            get => meshRenderer.enabled;
            set => meshRenderer.enabled = value;
        }

        public void SetMaterials(Material[] material)
        {
            meshRenderer.sharedMaterials = material;
        }

        protected virtual void Awake()
        {
            StopMove();
            moverTransform.OnReached += OnReachedWorldLocation;
        }

        private void OnReachedWorldLocation()
        {
            receiver?.ItemStacked(this);
        }

        protected virtual void OnDisable()
        {
            VisualEnabled = true;
            TweenScale?.Kill();
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Load(ItemConfigBase itemConfig)
        {
            if (Config == itemConfig)
                return;
            Config = itemConfig;
            Config.GetMeshData(out Mesh mesh, out var materials, out height);
            meshFilter.sharedMesh = mesh;
            if (!meshRenderer.materials.IsSame(materials))
            {
                meshRenderer.materials = materials;
            }
#if UNITY_EDITOR
            name = $"Item {Config.name}";
            if (!Application.isPlaying)
                this.Dirty();
#endif
        }

        public void SetConfig(ItemConfigBase itemConfig)
        {
            if (Config == itemConfig)
                return;
            Config = itemConfig;
        }

        public void Load(MeshRenderer meshRendererInput, Mesh mesh)
        {
            if (meshRendererInput == null)
            {
                return;
            }
            meshFilter.sharedMesh = mesh;
            meshRenderer.sharedMaterials = meshRendererInput.sharedMaterials;
        }

        public void SetStack(IItemReceiver value, in Vector3 localPos, float speedMultiplier, bool warp)
        {
            receiver = value;
            ItemMover other;
            
            if (warp)
            {
                transform.parent = value.Container;
                transform.localPosition = localPos;
                transform.localRotation = Quaternion.identity;
            }
            if (transform.parent == value.Container)
            {
                moverLocal.SetOffset(localPos);
                other = moverTransform;
            }
            else
            {
                other = moverLocal;
                moverTransform.SpeedMultiplier = speedMultiplier;
                moverTransform.SetTarget(value.Container, localPos);
            }

            other.enabled = false;
            other.SetOffsetSilently(localPos);
        }

        public void AddStackOffset(in Vector3 value)
        {
            ItemMover other;
            if (moverTransform.enabled)
            {
                moverTransform.AddOffset(value);
                other = moverLocal;
            }
            else
            {
                moverLocal.AddOffset(value);
                other = moverTransform;
            }

            other.AddOffsetSilently(value);
        }

        private void StopMove()
        {
            moverLocal.enabled = false;
            moverTransform.enabled = false;
        }
    }
}