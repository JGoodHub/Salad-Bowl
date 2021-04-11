using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static object lockObject = new object();
    private static T instance;

    [HideInInspector] public bool dying;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    // Search for existing instance.
                    T[] tArr = Resources.FindObjectsOfTypeAll<T>();

                    instance = tArr.Length > 0 ? tArr[0] : null;

                    // Create new instance if one doesn't already exist.
                    if (instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    }
                }

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Debug.Log($"Instance of Singleton |{typeof(T)}| already exists, destroying this copy '{gameObject.name}'", gameObject);
            Destroy(gameObject);
            dying = true;
        }
    }

}

