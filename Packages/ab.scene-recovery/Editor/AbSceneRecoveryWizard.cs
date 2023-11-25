using System.IO;
using UnityEditor;
using UnityEngine;

namespace Ab.SceneRecovery
{
    public class AbSceneRecoveryWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        [MenuItem("Window/シーン復元")]
        private static void Open()
        {
            var window = CreateInstance<AbSceneRecoveryWindow>();
            window.titleContent = new GUIContent("シーン復元");
            window.ShowUtility();
        }

        protected void OnGUI()
        {
            EditorGUILayout.LabelField("シーン復元");

            DrawBackupList();
        }
        private void DrawBackupList()
        {
            if (!Directory.Exists("Library/SceneBackup"))
            {
                return;
            }

            using (var s = new GUILayout.ScrollViewScope(_scrollPosition, GUILayout.ExpandHeight(true)))
            {
                _scrollPosition = s.scrollPosition;
                foreach (var file in Directory.GetFiles("Library/SceneBackup"))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label(File.GetLastWriteTime(file).ToString("F"));
                        if (GUILayout.Button("復元"))
                        {
                            var path = EditorUtility.SaveFilePanelInProject("シーンを保存する", "Backup.unity", "unity", "シーンを保存する");
                            File.Copy(file, path, true);
                            AssetDatabase.ImportAsset(path);
                            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
                        }
                    }
                }
            }
            
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("version 0.0.0");
            }
        }
    }
}
