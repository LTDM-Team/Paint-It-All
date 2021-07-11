using UnityEngine;

class CachedCamera
{
    public static Camera Camera
    {
        get => _camera != null
            ? _camera
            : _camera = Camera.main;
    }
    private static Camera _camera;
}