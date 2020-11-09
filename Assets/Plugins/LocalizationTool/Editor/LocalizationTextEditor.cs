using Assets.SimpleLocalization;
using UnityEditor;
using UnityEngine;

namespace Localization
{
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LocalizedText targetComponent = (LocalizedText)target;

            string newLocalizationKey = EditorGUILayout.TextField("Localization key", targetComponent.LocalizationKey);

            if (targetComponent.LocalizationKey != newLocalizationKey)
            {
                targetComponent.LocalizationKey = newLocalizationKey;
                EditorUtility.SetDirty(targetComponent);
            }
            

            if (!LocalizationManager.HasLocalization(targetComponent.LocalizationKey))
            {
                var style = new GUIStyle();
                style.normal.textColor = Color.red;
                EditorGUILayout.LabelField("There is no such key!", style);
            }

            if (GUILayout.Button("Select key"))
            {
                LocalizationEditorWindow.SelectKey((string selectedKey) =>
                {
                    targetComponent.LocalizationKey = selectedKey;
                });
            }
        }
    }

    [CustomEditor(typeof(LocalizedTextMesh))]
    public class LocalizedTextMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LocalizedTextMesh targetComponent = (LocalizedTextMesh)target;

            targetComponent.LocalizationKey = EditorGUILayout.TextField("Localization key", targetComponent.LocalizationKey);

            if (!LocalizationManager.HasLocalization(targetComponent.LocalizationKey))
            {
                var style = new GUIStyle();
                style.normal.textColor = Color.red;
                EditorGUILayout.LabelField("There is no such key!", style);
            }

            if (GUILayout.Button("Select key"))
            {
                LocalizationEditorWindow.SelectKey((string selectedKey) =>
                {
                    targetComponent.LocalizationKey = selectedKey;
                });
            }
        }
    }
}