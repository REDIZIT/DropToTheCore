using InGame.Level;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace InEditor.GameTools
{
    public class NewPatternEditorWindow : EditorWindow
    {
        private static NewPatternEditorWindow window;
        private AreaSO selectedArea;



        [MenuItem("Game Tools/New pattern")]
        public static void ShowWindow()
        {
            GetWindow();
            window.Show();
        }

        private void OnGUI()
        {
            GetWindow();

            if (selectedArea == null || selectedArea.prefab == null)
            {
                DrawDragAndDropZone();
            }
            else
            {
                DrawEditZone();
            }
        }

        private void DrawDragAndDropZone()
        {
            GUIStyle style = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter
            };
            style.normal.textColor = Color.white;



            Rect myRect = GUILayoutUtility.GetRect(window.position.width, window.position.height, GUILayout.ExpandWidth(true));
            if (myRect.Contains(Event.current.mousePosition))
            {
                if (DragAndDrop.objectReferences.Length > 0)
                {
                    GameObject dropped = DragAndDrop.objectReferences[0] as GameObject;
                    if (dropped == null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        GUI.Box(myRect, "This is not object from scene!", style);
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.DragPerform)
                    {
                        bool isPrefab = PrefabUtility.GetPrefabType(dropped) == PrefabType.Prefab;
                        if (!isPrefab)
                        {
                            GameObject prefab = CreatePrefab(dropped);

                            CreateScriptableArea(prefab);
                        }

                        Event.current.Use();
                    }
                }
                
            }

            GUI.Box(myRect, "Drag and Drop pattern from scene", style);
        }
        private void DrawEditZone()
        {
            if (selectedArea == null) return;

            GUIStyle h1 = new GUIStyle()
            {
                fontSize = 24,
                padding = new RectOffset(5, 0, 0, 0),
                fontStyle = FontStyle.Bold
            };
            h1.normal.textColor = Color.white;


            GUILayout.BeginHorizontal();


            // Draw preview
            Texture2D preview = AssetPreview.GetAssetPreview(selectedArea.prefab);
            if (preview != null)
            {
                GUILayout.Box(preview, GUILayout.Width(preview.width), GUILayout.Height(preview.height));
                Repaint();
            }



            // Draw texts
            GUILayout.BeginVertical();

            GUILayout.Space(14);
            GUILayout.Label(selectedArea.name, h1);
            if (selectedArea.name != selectedArea.prefab.name)
            {
                GUILayout.Label("Prefab has another name: " + selectedArea.prefab.name);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Show in inspector", GUILayout.ExpandWidth(false)))
            {
                EditorGUIUtility.PingObject(selectedArea);
                Selection.activeObject = selectedArea;
            }
            if (GUILayout.Button("Select prefab", GUILayout.ExpandWidth(false)))
            {
                EditorGUIUtility.PingObject(selectedArea.prefab);
                Selection.activeObject = selectedArea.prefab;
            }
            GUILayout.EndHorizontal();



            GUILayout.Space(10);

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("Spawn range:", GUILayout.ExpandWidth(false));
            //GUILayout.Space(-4);


            int newStartDepth = EditorGUILayout.DelayedIntField("Start depth (in km)", selectedArea.startDepth / 1000, GUILayout.ExpandWidth(false)) * 1000;
            int newEndDepth = EditorGUILayout.DelayedIntField("End depth (in km)", selectedArea.endDepth / 1000, GUILayout.ExpandWidth(false)) * 1000;


            if (newStartDepth != selectedArea.startDepth || newEndDepth != selectedArea.endDepth)
            {
                selectedArea.startDepth = newStartDepth;
                selectedArea.endDepth = newEndDepth;
                CorrectFilesPathes();
            }

            //GUILayout.EndHorizontal();

            GUILayout.EndVertical();




            GUILayout.EndHorizontal();
        }



        private GameObject CreatePrefab(GameObject origin)
        {
            return PrefabUtility.SaveAsPrefabAssetAndConnect(origin, "Assets/GameContent/Patterns/Prefabs/" + origin.name + ".prefab", InteractionMode.UserAction);
        }
        private void CreateScriptableArea(GameObject prefab)
        {
            AreaSO pattern = new AreaSO()
            {
                prefab = prefab,
                name = prefab.name
            };

            AssetDatabase.CreateAsset(pattern, "Assets/GameContent/Patterns/SOs/" + prefab.name + ".asset");
            selectedArea = pattern;
        }
        private void CorrectFilesPathes()
        {
            if (selectedArea == null) return;


            string actualSOPath = AssetDatabase.GetAssetPath(selectedArea);
            string actualPrefabPath = AssetDatabase.GetAssetPath(selectedArea.prefab);

            string subFolder = selectedArea.startDepth == 0 ? "" : selectedArea.startDepth.ToString();
            string correctSOPath = "Assets/GameContent/Patterns/SOs/" + subFolder + "/" + selectedArea.name + ".asset";
            string correctPrefabPath = "Assets/GameContent/Patterns/Prefabs/" + subFolder + "/" + selectedArea.prefab.name + ".prefab";



            if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(correctSOPath))) AssetDatabase.CreateFolder("Assets/GameContent/Patterns/SOs", new FileInfo(correctSOPath).Directory.Name);
            if (!AssetDatabase.IsValidFolder(Path.GetDirectoryName(correctPrefabPath))) AssetDatabase.CreateFolder("Assets/GameContent/Patterns/Prefabs", new FileInfo(correctPrefabPath).Directory.Name);


            if (actualSOPath != correctSOPath) AssetDatabase.MoveAsset(actualSOPath, correctSOPath);
            if (actualPrefabPath != correctPrefabPath) AssetDatabase.MoveAsset(actualPrefabPath, correctPrefabPath);
        }

        private static void GetWindow()
        {
            if (window == null) window = GetWindow<NewPatternEditorWindow>();
        }
    }
}