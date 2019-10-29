using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace UDK.Editor.GitUtils
{
	/// <summary>
	/// Remove empty folders automatically.
	/// </summary>
	public class RemoveEmptyFolders : UnityEditor.AssetModificationProcessor
	{
		public const string menuEntryText = "UDK/Remove Empty Folders Automatically";
		static readonly StringBuilder logBuilder = new StringBuilder();
		static readonly List<DirectoryInfo> emptyDirectories = new List<DirectoryInfo>();

		/// <summary>
		/// Raises the initialize on load method event.
		/// </summary>
		[InitializeOnLoadMethod]
		static void Init()
		{
			EditorApplication.delayCall += () => Valid();
			UnityWindowFocus.OnUnityWindowFocusChanged += (focus) => {RemoveNow();};
		}

		/// <summary>
		/// Raises the will save assets event.
		/// </summary>
		static string[] OnWillSaveAssets(string[] paths)
		{
			// If menu is unchecked, do nothing.
			if (!EditorPrefs.GetBool(menuEntryText, false))
				return paths;
		
			RemoveNow();

			return paths;
		}

		public static void RemoveNow()
		{
			// Get empty directories in Assets directory
			emptyDirectories.Clear();
			var assetsDir = Application.dataPath + Path.DirectorySeparatorChar;

			DirectoryInfo assetsDirInfo = new DirectoryInfo(assetsDir);

			FindEmptyDirectories(assetsDirInfo);
			
			if (emptyDirectories.Contains(assetsDirInfo)) emptyDirectories.Remove(assetsDirInfo);

			// When empty directories has detected, remove the directory.
			if (0 < emptyDirectories.Count)
			{
				logBuilder.Length = 0;
				logBuilder.AppendFormat("Remove {0} empty directories as following:\n", emptyDirectories.Count);
				foreach (var emptyDir in emptyDirectories)
				{
					logBuilder.AppendFormat("- {0}\n", emptyDir.FullName.Replace(assetsDir, ""));
					FileUtil.DeleteFileOrDirectory(emptyDir.FullName);
					FileUtil.DeleteFileOrDirectory(emptyDir.FullName + ".meta");
				}

				// UNITY BUG: Debug.Log can not set about more than 15000 characters.
				logBuilder.Length = Mathf.Min(logBuilder.Length, 15000);
				Debug.Log(logBuilder.ToString());
				logBuilder.Length = 0;

				AssetDatabase.Refresh();
			}
		}

		/// <summary>
		/// Toggles the menu.
		/// </summary>
		[MenuItem(menuEntryText)]
		static void OnClickMenu()
		{
			// Check/Uncheck menu.
			bool isChecked = !Menu.GetChecked(menuEntryText);
			Menu.SetChecked(menuEntryText, isChecked);

			// Save to EditorPrefs.
			EditorPrefs.SetBool(menuEntryText, isChecked);

			OnWillSaveAssets(null);
		}
		
		[MenuItem(menuEntryText, true)]
		static bool Valid()
		{
			// Check/Uncheck menu from EditorPrefs.
			Menu.SetChecked(menuEntryText, EditorPrefs.GetBool(menuEntryText, true));
			return true;
		}



		/// <summary>
		/// Get empty directories.
		/// </summary>
		static bool FindEmptyDirectories(DirectoryInfo dir)
		{
			bool isEmpty = true;

			if (dir.Exists)
				isEmpty = dir.GetDirectories().Count(x => !FindEmptyDirectories(x)) == 0	// Are sub directories empty?
				&& dir.GetFiles("*.*").All(x => x.Extension == ".meta");	// No file exist?

			// Store empty directory to results.
			if (isEmpty)
				emptyDirectories.Add(dir);
			return isEmpty;
		}
	}
}