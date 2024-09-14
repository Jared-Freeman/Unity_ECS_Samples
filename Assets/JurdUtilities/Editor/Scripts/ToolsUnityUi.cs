using UnityEditor;
using UnityEngine;

namespace Jurd.Utilities.Editor
{
    public static class ToolsUnityUI
    {
        // Hotkey: Alt+A
        [MenuItem("Tools/Anchors to Corners (RectTransform)/Width and Height &a")]
        public static void MoveAnchorsToCorners()
        {
            GameObjectUtilities.GetSelectedComponents<RectTransform>().ForEach(it =>
            {
                Undo.RecordObject(it, "MoveAnchorsToCorners");
                UiAnchorExtensions.MoveAnchorsToCorners(it);
            });
        }

        [MenuItem("Tools/Anchors to Corners (RectTransform)/Width")]
        public static void MoveAnchorsToCorners_Width()
        {
            GameObjectUtilities.GetSelectedComponents<RectTransform>().ForEach(it =>
            {
                Undo.RecordObject(it, "MoveAnchorsToCorners_Width");
                UiAnchorExtensions.MoveAnchorsToCorners_Width(it);
            });
        }

        [MenuItem("Tools/Anchors to Corners (RectTransform)/Height")]
        public static void MoveAnchorsToCorners_Height()
        {
            GameObjectUtilities.GetSelectedComponents<RectTransform>().ForEach(it =>
            {
                Undo.RecordObject(it, "MoveAnchorsToCorners_Height");
                UiAnchorExtensions.MoveAnchorsToCorners_Height(it);
            });
        }
    }
}