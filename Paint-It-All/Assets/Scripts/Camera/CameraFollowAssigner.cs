using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFollowAssigner : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        AssignToPlayer();
    }

    private void AssignToPlayer()
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        _virtualCamera.Follow = playerInput.transform;
    }
}