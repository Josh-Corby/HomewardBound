using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : JMC
{
    protected static GameManager GM { get { return GameManager.INSTANCE; } }
    protected static PlayerLocomotion PL { get { return PlayerLocomotion.INSTANCE; } }
    protected static PlayerManager PM { get { return PlayerManager.INSTANCE; } }
    protected static CameraManager CM { get { return CameraManager.INSTANCE; } }
    protected static AnimatorManager AM { get { return AnimatorManager.INSTANCE; } }
    protected static InputManager IM { get { return InputManager.INSTANCE; } }
    protected static FlashLight FL { get { return FlashLight.INSTANCE; } }
    protected static UIManager UI { get { return UIManager.INSTANCE; } }
    protected static BuildManager BM { get { return BuildManager.INSTANCE; } }
    protected static InteractionZone IZ { get { return InteractionZone.INSTANCE; } }

}

public class GameBehaviour<T> : GameBehaviour where T : GameBehaviour
{
    private static T instance_;
    public static T INSTANCE
    {
        get
        {
            if (instance_ == null)
            {
                instance_ = GameObject.FindObjectOfType<T>();
                if (instance_ == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    singleton.AddComponent<T>();
                }
            }
            return instance_;
        }
    }
    protected virtual void Awake()
    {
        if (instance_ == null)
        {
            instance_ = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
