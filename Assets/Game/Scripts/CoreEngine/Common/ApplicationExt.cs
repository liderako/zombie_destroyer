using UnityEngine;

namespace Common
{
    public static class ApplicationExt
    {
        public static bool Quitting { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void GameInit()
        {
            Quitting = false;
            Application.quitting += Callback;
        }

        private static void Callback()
        {
            Quitting = true;
        }
    }
}