using Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Collections
{
    public class DemoRandomElement : MonoBehaviour
    {
        [SerializeField]
        private RandomList<string> list;


        private RandomList<string> runtimeList;

        private void Start()
        {
            runtimeList = new RandomList<string>();
            foreach (var value in list.Values)
                runtimeList.Add(value);
        }
#if ODIN_INSPECTOR
        [HideInEditorMode]
        [Button]
        public RandomList<string>.Element GetRandom([PropertyRange(0, 1)] float value)
        {
            return runtimeList.GetRandom(value);
        }
#endif
    }
}