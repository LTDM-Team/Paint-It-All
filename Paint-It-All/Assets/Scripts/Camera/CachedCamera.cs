using UnityEngine;

class CachedCamera
{
    public static Camera Camera { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnSceneLoad()
    {
        Camera = Camera.main;
    }
}