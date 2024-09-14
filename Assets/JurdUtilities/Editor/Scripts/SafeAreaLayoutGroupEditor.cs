using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Jurd.Utilities.SafeAreaLayout;

namespace Jurd.Utilities.Editor
{
    [CustomEditor(typeof(SafeAreaLayoutGroup)), CanEditMultipleObjects]
    public class SafeAreaLayoutGroupEditor : UnityEditor.Editor
    {
        private static readonly GUIContent _previewModeContent = new GUIContent
        {
            text = "Preview Mode",
            tooltip = "This will be used to simulate cutouts in editor while playing or hovering the preview button below",
        };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Editor cutout simulation", EditorStyles.boldLabel);

            int currentPreviewMode = SafeAreaUtilities.CurrentEditorPreviewMode;
            int newPreviewMode = EditorGUILayout.Popup(_previewModeContent, currentPreviewMode, SafeAreaUtilities.EditorPreviewModeNames);
            if (currentPreviewMode != newPreviewMode)
            {
                SafeAreaUtilities.CurrentEditorPreviewMode = newPreviewMode;
                if (Application.isPlaying)
                {
                    RefreshAll();
                }
            }

            bool preview = HoverButton("Hover to Preview Layout");
            if (preview != SafeAreaLayoutGroup.PreviewInEditor)
            {
                SafeAreaLayoutGroup.PreviewInEditor = preview;
                RefreshAll();
            }
        }

        private static void RefreshAll()
        {
            foreach (SafeAreaLayoutGroup safeArea in FindObjectsByType<SafeAreaLayoutGroup>(FindObjectsSortMode.None))
            {
                safeArea.RefreshChildrenAnchors();
                LayoutRebuilder.ForceRebuildLayoutImmediate(safeArea.SelfRectTransform);
            }
            SceneView.RepaintAll();
        }

        private static bool HoverButton(string content)
        {
            GUILayout.Box(content, EditorStyles.miniButton);
            return GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
        }
    }
}