using UnityEngine;
using Cinemachine;
using System.Xml.Serialization;
namespace Features.Gameplay
{
    public class CameraZoomController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    public float zoomSpeed = 5f;

    private float targetFOV;

    void Start()
    {
        targetFOV = vcam.m_Lens.FieldOfView;
    }

    void Update()
    {
        float current = vcam.m_Lens.FieldOfView;

        vcam.m_Lens.FieldOfView = Mathf.Lerp(
            current,
            targetFOV,
            zoomSpeed * Time.deltaTime
        );
    }

    public void ZoomIn()
    {
        targetFOV = 60;
    }

    public void ZoomOut()
    {
        targetFOV = 48f;
    }
}
}
