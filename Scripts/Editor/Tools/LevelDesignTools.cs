using UnityEngine;
using UnityEditor;

namespace UDK.Editor
{
    public static class LevelDesignTools
    {
        const string menu_preffix = "UDK/Level Design Tools/";
        const string y_up_menu = menu_preffix + "Select Up Axis/Y";
        const string z_up_menu = menu_preffix + "Select Up Axis/Z";

        const bool default_Y = true;
        const bool default_Z = false;

        // --- Boilerplate code needed for menu checkboxes to work ---

        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.delayCall += () =>
            {
                MenuItemsValidateFunction();
            };
        }

        [MenuItem(y_up_menu, true, 999)]
        [MenuItem(z_up_menu, true, 999)]
        static bool MenuItemsValidateFunction()
        {
            Menu.SetChecked(z_up_menu, EditorPrefs.GetBool(z_up_menu, default_Z));
            Menu.SetChecked(y_up_menu, EditorPrefs.GetBool(y_up_menu, default_Y));
            return true;
        }

        // --- Actual Y mode menu entries ---

        [MenuItem(y_up_menu, false, 999)]
        static void Set_Y_UP()
        {
            // if already checked, do nothing
            if (EditorPrefs.GetBool(y_up_menu, default_Y))
                return;

            // if not checked, check menu
            Menu.SetChecked(y_up_menu, true);
            Menu.SetChecked(z_up_menu, false);

            // Save to EditorPrefs
            EditorPrefs.SetBool(y_up_menu, true);
            EditorPrefs.SetBool(z_up_menu, false);

            Debug.Log("[UDK] Y Up enabled for level design tools");
        }

        [MenuItem(z_up_menu, false, 999)]
        static void Set_Z_UP()
        {
            // if already checked, do nothing
            if (EditorPrefs.GetBool(z_up_menu, default_Z))
                return;

            // if not checked, check menu
            Menu.SetChecked(z_up_menu, true);
            Menu.SetChecked(y_up_menu, false);

            // Save to EditorPrefs.
            EditorPrefs.SetBool(z_up_menu, true);
            EditorPrefs.SetBool(y_up_menu, false);

            Debug.Log("[UDK] Z Up enabled for level design tools");
        }

        // --- UP Mode Utilities ---

        enum UP_MODE
        {
            Y,
            Z
        }

        static UP_MODE GetUpMode()
        {
            if (EditorPrefs.GetBool(z_up_menu, default_Z) && !EditorPrefs.GetBool(y_up_menu, default_Y))
                return UP_MODE.Z;
            if (!EditorPrefs.GetBool(z_up_menu, default_Z) && EditorPrefs.GetBool(y_up_menu, default_Y))
                return UP_MODE.Y;
            return UP_MODE.Y;
        }

        static void AlignToNormal(this Transform t, Vector3 normal)
        {
            if (GetUpMode() == UP_MODE.Y)
                t.up = normal;
            else
                t.forward = normal;
        }

        static Quaternion GetQuaternionUp()
        {
            if (GetUpMode() == UP_MODE.Y)
                return Quaternion.identity;
            else
                return Quaternion.Euler(Vector3.right * -90f);
        }

        // --- Commands ---

        [MenuItem(menu_preffix + "Average rotations to up", false, 100)]
        static void LerpPrefabsRotationsToUp()
        {
            Undo.RecordObjects(Selection.transforms, "Selected gameobjects rotated");
            foreach (var t in Selection.transforms)
            {
                t.rotation = Quaternion.Lerp(t.rotation, GetQuaternionUp(), 0.5f);
            }
        }

        // Alt + Shift + F
        [MenuItem(menu_preffix + "Move to raycast #&f", false, 2)]
        static void BringToFrontGroundSnapped()
        {
            Camera sceneCamera = UnityEditor.SceneView.GetAllSceneCameras()[0];

            bool groundFound = Physics.Raycast(sceneCamera.transform.position, sceneCamera.transform.forward, out RaycastHit hit);

            if (!groundFound || hit.collider.isTrigger)
                return;

            Undo.RecordObjects(Selection.transforms, "Snap gameobjects to front ground");
            foreach (var t in Selection.transforms)
            {
                t.position = hit.point;
                t.AlignToNormal(hit.normal);
            }
        }

        // Alt + F
        [MenuItem(menu_preffix + "Bring to camera &f", false, 3)]
        static void BringToFront()
        {
            Camera sceneCamera = UnityEditor.SceneView.GetAllSceneCameras()[0];

            Undo.RecordObjects(Selection.transforms, "Bring gameobjects to front");
            foreach (var t in Selection.transforms)
            {
                t.position = sceneCamera.transform.position + sceneCamera.transform.forward * 2f;
            }
        }

        // Ctrl + G
        [MenuItem(menu_preffix + "Snap to ground %g", false, 1)]
        static void SnapToGround()
        {
            Undo.RecordObjects(Selection.transforms, "Snap gameobjects to ground");
            foreach (var t in Selection.transforms)
            {
                if (Physics.Raycast(t.position, Vector3.down, out RaycastHit hit))
                {
                    t.position = hit.point;
                    t.AlignToNormal(hit.normal);
                }
            }
        }
    }
}