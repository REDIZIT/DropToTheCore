using InGame.Level;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace InEditor.GameTools
{
    public class PatternsHeightsEditorWindow : EditorWindow
    {
        [MenuItem("Game Tools/Patterns Heights")]
        public static void ShowWindow()
        {
            currentWindow = GetWindow<PatternsHeightsEditorWindow>();
            currentWindow.Show();
        }


        private GUIStyle _staticRectStyle;
        private GUIStyle _transparentStyle, _selectedStyle;

        private List<AreaSO> indexedPatterns;
        private List<AreaSO> searchedAreas;

        private AreaSO selectedArea;

        private string searchString = "";

        private static PatternsHeightsEditorWindow currentWindow;


        private void OnGUI()
        {
            
            InitStyles();
            IndexPatterns();






            if (currentWindow == null) currentWindow = GetWindow<PatternsHeightsEditorWindow>();
            float windowWidth = currentWindow.position.width;
            float maxDepth = indexedPatterns.Max(c => c.endDepth);

            HandleArrowKeysScrolling();
            DrawSearch();
            DrawDepthHeader(maxDepth, windowWidth);


            searchedAreas = indexedPatterns.Where(c => c.name.ToLower().Contains(searchString.ToLower())).ToList();
            DrawWaterfall(searchedAreas, maxDepth, windowWidth);

           




            DrawSelectedInfo();
        }



        private void InitStyles()
        {
            if (_staticRectStyle == null)
            {
                Texture2D _staticRectTexture = new Texture2D(1, 1);
                Texture2D _transparentTexture = new Texture2D(1, 1);
                Texture2D _selectedTexture = new Texture2D(1, 1);

                _staticRectTexture.SetPixel(0, 0, new Color(60 / 255f, 70 / 255f, 80 / 255f));
                _transparentTexture.SetPixel(0, 0, new Color(1, 1, 1, 0));
                _selectedTexture.SetPixel(0, 0, new Color(120 / 255f, 140 / 255f, 160 / 255f));

                _staticRectTexture.Apply();
                _transparentTexture.Apply();
                _selectedTexture.Apply();

                _staticRectStyle = new GUIStyle();
                _transparentStyle = new GUIStyle();
                _selectedStyle = new GUIStyle();

                _staticRectStyle.normal.background = _staticRectTexture;
                _transparentStyle.normal.background = _transparentTexture;
                _selectedStyle.normal.background = _selectedTexture;
            }
        }
        private void IndexPatterns()
        {
            indexedPatterns = new List<AreaSO>();

            string[] aMaterialFiles = Directory.GetFiles(Application.dataPath + "/GameContent/Patterns/SOs", "*.asset", SearchOption.AllDirectories);
            foreach (string matFile in aMaterialFiles)
            {
                string assetPath = "Assets" + matFile.Replace(Application.dataPath, "").Replace('\\', '/');
                indexedPatterns.Add((AreaSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AreaSO)));
            }

            indexedPatterns = indexedPatterns.OrderBy(c => c.startDepth).ToList();
        }
        private void HandleArrowKeysScrolling()
        {
            if (Event.current != null && Event.current.isKey && Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.DownArrow)
                {
                    int index = searchedAreas.IndexOf(selectedArea);
                    selectedArea = searchedAreas[Mathf.Clamp(index + 1, 0, searchedAreas.Count - 1)];
                    Repaint();
                }
                else if (Event.current.keyCode == KeyCode.UpArrow)
                {
                    int index = searchedAreas.IndexOf(selectedArea);
                    selectedArea = searchedAreas[Mathf.Clamp(index - 1, 0, searchedAreas.Count - 1)];
                    Repaint();
                }
            }
        }



        private void DrawSearch()
        {
            // Ignore arrows key events
            if (Event.current != null && Event.current.isKey && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.DownArrow || Event.current.keyCode == KeyCode.UpArrow))
            {
                return;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Search", GUILayout.ExpandWidth(false));
            searchString = GUILayout.TextField(searchString);
            GUILayout.EndHorizontal();
        }
        private void DrawDepthHeader(float maxDepth, float windowWidth)
        {
            GUILayout.BeginHorizontal();

            float halfKilometers = maxDepth / 500f;
            for (int i = 0; i <= halfKilometers; i++)
            {
                GUILayout.Label((i / 2f) + "km", GUILayout.Width(windowWidth / halfKilometers));
                GUILayout.Space(-4);
            }
            GUILayout.EndHorizontal();
        }
        private void DrawWaterfall(IEnumerable<AreaSO> patterns, float maxDepth, float windowWidth)
        {
            foreach (AreaSO pattern in patterns)
            {
                float normalizedStartDepth = pattern.startDepth / maxDepth;
                float normalizedEndDepth = pattern.endDepth == 0 ? 1 : pattern.endDepth / maxDepth;

                GUILayout.Space(1);
                GUILayout.BeginHorizontal();


                GUILayout.Button(_transparentStyle.normal.background, _transparentStyle, GUILayout.Width(normalizedStartDepth * windowWidth), GUILayout.Height(3));
                if (GUILayout.Button(_staticRectStyle.normal.background, pattern == selectedArea ? _selectedStyle : _staticRectStyle, GUILayout.Width((normalizedEndDepth - normalizedStartDepth) * windowWidth), GUILayout.Height(3)))
                {
                    selectedArea = pattern;
                    EditorGUIUtility.PingObject(selectedArea);

                }


                GUILayout.EndHorizontal();
            }
        }
        private void DrawSelectedInfo()
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
            if(selectedArea.name != selectedArea.prefab.name)
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

            GUILayout.BeginHorizontal();
            GUILayout.Label("Spawn range:", GUILayout.ExpandWidth(false));
            GUILayout.Space(-4);
            GUILayout.Label(selectedArea.startDepth + " - " + (selectedArea.endDepth == 0 ? "Infinity" : selectedArea.endDepth.ToString()), EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();




            GUILayout.EndHorizontal();
        }
    }
}