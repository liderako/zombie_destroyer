using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Entities.Zombies.Editor
{
    // stickman.fbx has no skeleton. This tool builds a humanoid rig for it, so the humanoid clips of the zombie
    // animation pack retarget onto it: a skeleton from StickmanJoints, the mesh skinned to it, a humanoid avatar,
    // and the "View" of the Zombie prefab rebuilt with them
    public static class StickmanRigBuilder
    {
        private const string MODEL = "Assets/Art/Models/stickman.fbx";
        private const string MATERIAL = "Assets/Game/Graphics/Materials/M_Zombie.mat";
        private const string CONTROLLER = "Assets/Game/Animations/AC_Zombie.controller";
        private const string ZOMBIE_PREFAB = "Assets/Game/Prefabs/Entities/Zombie.prefab";
        private const string RIG_FOLDER = "Assets/Game/Animations/Stickman";
        private const string MESH_PATH = RIG_FOLDER + "/SM_StickmanSkinned.asset";
        private const string AVATAR_PATH = RIG_FOLDER + "/StickmanAvatar.asset";
        private const string VIEW = "View";
        private const string BODY = "Body";
        private const string END_SUFFIX = "End";
        // Avoids division by zero for vertices lying on a bone
        private const float WEIGHT_EPSILON = 0.0004f;

        // T-pose joints in model space, measured from the mesh. The model faces +Z, so its left side is -X.
        // "...End" joints only mark where the last bone of a chain ends
        private static readonly (string Bone, string Parent, Vector3 Position)[] StickmanJoints =
        {
            ("Hips", null, new Vector3(0f, 0.55f, 0f)),
            ("Spine", "Hips", new Vector3(0f, 0.68f, 0f)),
            ("Chest", "Spine", new Vector3(0f, 0.9f, 0f)),
            ("Neck", "Chest", new Vector3(0f, 1.16f, 0f)),
            ("Head", "Neck", new Vector3(0f, 1.22f, 0f)),
            ("HeadEnd", "Head", new Vector3(0f, 1.78f, 0f)),
            ("LeftShoulder", "Chest", new Vector3(-0.08f, 1.11f, 0f)),
            ("LeftUpperArm", "LeftShoulder", new Vector3(-0.22f, 1.11f, 0f)),
            ("LeftLowerArm", "LeftUpperArm", new Vector3(-0.5f, 1.11f, 0f)),
            ("LeftHand", "LeftLowerArm", new Vector3(-0.76f, 1.11f, 0f)),
            ("LeftHandEnd", "LeftHand", new Vector3(-0.88f, 1.11f, 0f)),
            ("RightShoulder", "Chest", new Vector3(0.08f, 1.11f, 0f)),
            ("RightUpperArm", "RightShoulder", new Vector3(0.22f, 1.11f, 0f)),
            ("RightLowerArm", "RightUpperArm", new Vector3(0.5f, 1.11f, 0f)),
            ("RightHand", "RightLowerArm", new Vector3(0.76f, 1.11f, 0f)),
            ("RightHandEnd", "RightHand", new Vector3(0.88f, 1.11f, 0f)),
            ("LeftUpperLeg", "Hips", new Vector3(-0.125f, 0.5f, 0f)),
            ("LeftLowerLeg", "LeftUpperLeg", new Vector3(-0.125f, 0.28f, 0f)),
            ("LeftFoot", "LeftLowerLeg", new Vector3(-0.125f, 0.08f, 0f)),
            ("LeftToes", "LeftFoot", new Vector3(-0.125f, 0.03f, 0.15f)),
            ("LeftToesEnd", "LeftToes", new Vector3(-0.125f, 0.03f, 0.24f)),
            ("RightUpperLeg", "Hips", new Vector3(0.125f, 0.5f, 0f)),
            ("RightLowerLeg", "RightUpperLeg", new Vector3(0.125f, 0.28f, 0f)),
            ("RightFoot", "RightLowerLeg", new Vector3(0.125f, 0.08f, 0f)),
            ("RightToes", "RightFoot", new Vector3(0.125f, 0.03f, 0.15f)),
            ("RightToesEnd", "RightToes", new Vector3(0.125f, 0.03f, 0.24f)),
        };

        [MenuItem("Tools/Zombie Destroyer/Rebuild Stickman Rig")]
        public static void Rebuild()
        {
            GameObject root = PrefabUtility.LoadPrefabContents(ZOMBIE_PREFAB);
            try
            {
                Transform oldView = root.transform.Find(VIEW);
                int siblingIndex = oldView.GetSiblingIndex();
                oldView.name = VIEW + "_Old";

                GameObject view = CreateView(root.transform, out Avatar avatar);
                view.transform.SetSiblingIndex(siblingIndex);
                Object.DestroyImmediate(oldView.gameObject);

                var animator = view.AddComponent<Animator>();
                animator.avatar = avatar;
                animator.runtimeAnimatorController = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(CONTROLLER);
                animator.applyRootMotion = false;
                animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

                var viewComponent = new SerializedObject(root.GetComponent<ZombieViewComponent>());
                viewComponent.FindProperty("view").objectReferenceValue = view.transform;
                viewComponent.FindProperty("animator").objectReferenceValue = animator;
                viewComponent.ApplyModifiedPropertiesWithoutUndo();

                PrefabUtility.SaveAsPrefabAsset(root, ZOMBIE_PREFAB);
                Debug.Log($"Stickman rig rebuilt: avatar valid={avatar.isValid}, human={avatar.isHuman}");
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        private static GameObject CreateView(Transform parent, out Avatar avatar)
        {
            var view = new GameObject(VIEW);
            view.transform.SetParent(parent, false);

            var joints = new Dictionary<string, Transform>();
            foreach ((string bone, string boneParent, Vector3 position) in StickmanJoints)
            {
                Transform joint = new GameObject(bone).transform;
                joint.SetParent(boneParent == null ? view.transform : joints[boneParent], false);
                joint.position = view.transform.TransformPoint(position);
                joints[bone] = joint;
            }

            List<Transform> bones = joints.Values.Where(joint => !joint.name.EndsWith(END_SUFFIX)).ToList();
            Mesh mesh = CreateSkinnedMesh(view.transform, bones);

            var body = new GameObject(BODY);
            body.transform.SetParent(view.transform, false);
            var renderer = body.AddComponent<SkinnedMeshRenderer>();
            renderer.sharedMesh = mesh;
            renderer.bones = bones.ToArray();
            renderer.rootBone = joints["Hips"];
            renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(MATERIAL);
            renderer.localBounds = new Bounds(new Vector3(0f, 0.35f, 0f), new Vector3(2f, 2.2f, 1.2f));

            avatar = AvatarBuilder.BuildHumanAvatar(view, CreateHumanDescription(view, bones));
            avatar.name = "StickmanAvatar";
            SaveAsset(avatar, AVATAR_PATH);
            return view;
        }

        // Every vertex follows the two nearest bones of its own chain (legs, torso with head, arms),
        // so an arm never pulls the side of the body with it
        private static Mesh CreateSkinnedMesh(Transform view, List<Transform> bones)
        {
            Mesh source = AssetDatabase.LoadAssetAtPath<GameObject>(MODEL).GetComponent<MeshFilter>().sharedMesh;
            Mesh mesh = Object.Instantiate(source);
            mesh.name = "SM_StickmanSkinned";

            Vector3[] vertices = mesh.vertices;
            var weights = new BoneWeight[vertices.Length];
            for (int v = 0; v < vertices.Length; v++)
            {
                string chain = GetVertexChain(vertices[v]);
                int best = -1;
                int second = -1;
                float bestWeight = 0f;
                float secondWeight = 0f;
                for (int b = 0; b < bones.Count; b++)
                {
                    if (GetBoneChain(bones[b].name) != chain)
                    {
                        continue;
                    }

                    Vector3 start = view.InverseTransformPoint(bones[b].position);
                    Vector3 end = view.InverseTransformPoint(bones[b].GetChild(0).position);
                    float distance = DistanceToSegment(vertices[v], start, end);
                    float weight = 1f / (distance * distance + WEIGHT_EPSILON);
                    if (weight > bestWeight)
                    {
                        second = best;
                        secondWeight = bestWeight;
                        best = b;
                        bestWeight = weight;
                    }
                    else if (weight > secondWeight)
                    {
                        second = b;
                        secondWeight = weight;
                    }
                }

                float total = bestWeight + secondWeight;
                weights[v] = new BoneWeight
                {
                    boneIndex0 = best,
                    weight0 = bestWeight / total,
                    boneIndex1 = second < 0 ? best : second,
                    weight1 = secondWeight / total,
                };
            }

            mesh.boneWeights = weights;
            mesh.bindposes = bones.Select(bone => bone.worldToLocalMatrix * view.localToWorldMatrix).ToArray();
            SaveAsset(mesh, MESH_PATH);
            return mesh;
        }

        private static HumanDescription CreateHumanDescription(GameObject view, List<Transform> bones)
        {
            return new HumanDescription
            {
                human = bones.Select(bone => new HumanBone
                {
                    humanName = bone.name,
                    boneName = bone.name,
                    limit = new HumanLimit { useDefaultValues = true },
                }).ToArray(),
                skeleton = view.GetComponentsInChildren<Transform>().Select(transform => new SkeletonBone
                {
                    name = transform.name,
                    position = transform.localPosition,
                    rotation = transform.localRotation,
                    scale = transform.localScale,
                }).ToArray(),
                upperArmTwist = 0.5f,
                lowerArmTwist = 0.5f,
                upperLegTwist = 0.5f,
                lowerLegTwist = 0.5f,
                armStretch = 0.05f,
                legStretch = 0.05f,
                feetSpacing = 0f,
                hasTranslationDoF = false,
            };
        }

        private static string GetVertexChain(Vector3 point)
        {
            if (point.y > 1f && point.y < 1.26f && Mathf.Abs(point.x) > 0.21f)
            {
                return point.x < 0f ? "LeftArm" : "RightArm";
            }

            if (point.y < 0.47f)
            {
                return point.x < 0f ? "LeftLeg" : "RightLeg";
            }

            return "Spine";
        }

        private static string GetBoneChain(string bone)
        {
            if (bone.Contains("Arm") || bone.Contains("Hand") || bone.Contains("Shoulder"))
            {
                return bone.StartsWith("Left") ? "LeftArm" : "RightArm";
            }

            if (bone.Contains("Leg") || bone.Contains("Foot") || bone.Contains("Toes"))
            {
                return bone.StartsWith("Left") ? "LeftLeg" : "RightLeg";
            }

            return "Spine";
        }

        private static float DistanceToSegment(Vector3 point, Vector3 start, Vector3 end)
        {
            Vector3 segment = end - start;
            float t = Mathf.Clamp01(Vector3.Dot(point - start, segment) / segment.sqrMagnitude);
            return Vector3.Distance(point, start + segment * t);
        }

        private static void SaveAsset(Object asset, string path)
        {
            if (!AssetDatabase.IsValidFolder(RIG_FOLDER))
            {
                AssetDatabase.CreateFolder("Assets/Game/Animations", "Stickman");
            }

            AssetDatabase.DeleteAsset(path);
            AssetDatabase.CreateAsset(asset, path);
        }
    }
}
