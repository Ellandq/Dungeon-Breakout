using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    
    public static T Instance
    {
        get
        {
            if (_instance) return _instance;
            _instance = FindFirstObjectByType<T>();
                
            if (!_instance)
            {
                Debug.LogError($"No instance of {typeof(T)} found in scene.");
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
        // if (_instance == null)
        // {
        //     _instance = this as T;
        // }
        // else if (_instance != this)
        // {
        //     Debug.LogWarning($"{typeof(T)} instance already exists. Destroying duplicate.");
        //     Destroy(gameObject);
        // }
    }
    
    private void OnDestroy()
    {
        _instance = null;
    }
}