using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController Instance { get; private set; }

    [SerializeField] CinemachineVirtualCamera vCam;
    CinemachineTransposer transposer;


    private void Awake()
    {
        Instance = this;
        transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    public void ResetCam()
    {
        vCam.ForceCameraPosition(PlayerMovementController.Instance.transform.position + transposer.m_FollowOffset, vCam.transform.rotation);
    }
}
