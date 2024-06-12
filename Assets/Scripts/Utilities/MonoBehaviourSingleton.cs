
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : Component
{
  static readonly string kGameObjectName = "_Global";
  protected static T _instance;

  public static T instance
  {
    get
    {
      if (_instance)
      {
        return _instance;
      }

      T temp = FindObjectOfType<T>();
      GameObject go;

      if (temp == null)
      {
        Debug.LogWarning(
          $"No instance of {typeof(T)} found. Spawning {kGameObjectName} GameObject and adding it as a component.");
        go = GameObject.Find(kGameObjectName);

        if (go == null)
        {
          go = new GameObject(kGameObjectName);
        }
      }
      else
      {
        go = temp.gameObject;
      }

      _instance = go.GetComponent<T>();

      if (_instance)
      {
        return _instance;
      }

      _instance = FindObjectOfType<T>();

      return _instance;
    }
  }

  public virtual void OnAwake()
  {
    _instance = gameObject.GetComponent<T>();
  }

  public virtual void Enable()
  {
    _instance = gameObject.GetComponent<T>();
  }

  public virtual void Disable()
  {
    _instance = null;
  }
}

