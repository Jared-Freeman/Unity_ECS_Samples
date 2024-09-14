using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jurd.Utilities.Editor
{
    public static class GameObjectUtilities
    {
        /// <summary>
        /// Returns list of <typeparamref name="T"/> components currently selected in the editor
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <returns></returns>
        public static List<T> GetSelectedComponents<T>()
        {
            List<T> result = new List<T>();
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length <= 0 || selectedObjects == null)
            {
                return result;
            }

            foreach (GameObject go in selectedObjects)
            {
                T c = go.GetComponent<T>();

                if (c != null)
                {
                    result.Add(c);
                }
            }

            return result;
        }
    }
}