using System;
using UnityEditor;

namespace UDK.Editor
{
    [InitializeOnLoad]
    public class UnityWindowFocus
    {
        public static event Action<bool> OnUnityWindowFocusChanged = (focus) => { };
        private static bool _appFocused;
        static UnityWindowFocus()
        {
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            if (!_appFocused && UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnUnityWindowFocusChanged(true);
            }
            else if (_appFocused && !UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                _appFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnUnityWindowFocusChanged(false);
            }
        }
    }
}
