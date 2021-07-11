using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
class CameraFollowAssigner : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void AssignToPlayer(Player player)
    {
        _virtualCamera.Follow = player.transform;
    }
}