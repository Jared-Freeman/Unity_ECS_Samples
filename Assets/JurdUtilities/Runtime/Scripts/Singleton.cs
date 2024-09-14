using UnityEngine;

namespace Jurd.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static bool InstanceExists
        {
            get
            {
                return _instance != null;
            }
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //find instance if it exists
                    _instance = Object.FindFirstObjectByType<T>();

                    //Create new instance
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject();
                        gameObject.name = $"[Singleton] {typeof(T).Name}";
                        _instance = gameObject.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this as T;

            if (gameObject.scene.name != "DontDestroyOnLoad")
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}