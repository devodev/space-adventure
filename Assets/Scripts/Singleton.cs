using UnityEngine;

// https://gamedev.stackexchange.com/a/177029
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    public static T Instance { get; private set; }

    protected void Awake() {
        Instance = (T)this;
    }
}
