using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager2D : MonoBehaviour
{
    public static CameraManager2D instance;

    public enum FollowMode
    {
        SPEED,
        RATIO
    }
    [Header ("Follow Settings")]
    public FollowMode followMode = FollowMode.SPEED;
    public float followSpeed = 5f;
    [Range(0f,1f)]
    public float followRatio = 0.5f;
    public Transform followTarget;

    [System.Serializable]

    public class CameraBounds
    {
        public float Top =1f;
        public float Left=-1f;
        public float Right=1f;
        public float Bottom=-1f;
    }

    [Header("Bounds Settings")]
    public CameraBounds _cameraBounds;

    private Camera _camera;

    private void Awake()
    {
        instance = this;
        _camera = GetComponentInChildren<Camera>();
    }
    private void LateUpdate()
    {
        Vector3 position = transform.position;
        Vector3 followPosition = followTarget.position;
        followPosition = _ClampPositionToScreen(followTarget.position);
        switch (followMode)
        {
            case FollowMode.SPEED:
                position.x = Mathf.Lerp(position.x, followPosition.x, Time.deltaTime * followSpeed);
                position.y = Mathf.Lerp(position.y, followPosition.y, Time.deltaTime * followSpeed);
                break;
            case FollowMode.RATIO:
                position.x = Mathf.Lerp(position.x, followPosition.x, followRatio * Time.timeScale);
                position.y = Mathf.Lerp(position.y, followPosition.y, followRatio * Time.timeScale);
                break;

        }
        transform.position = position;
    }

    private Vector3 _ClampPositionToScreen(Vector3 position)
    {
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(_camera.pixelWidth,_camera.pixelHeight,0f));

        Vector2 screenSize = new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        Vector2 halfScreenSize = screenSize / 2f;

        position.x = Mathf.Clamp(position.x, _cameraBounds.Left + halfScreenSize.x,_cameraBounds.Right - halfScreenSize.x);
        position.y = Mathf.Clamp(position.y, _cameraBounds.Bottom + halfScreenSize.y,_cameraBounds.Top - halfScreenSize.y);

        return position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        float gizmosCubeSize = 0.3f;

        Vector3 topLeft = new Vector3(_cameraBounds.Left, _cameraBounds.Top, transform.position.z);
        Gizmos.DrawCube(topLeft, Vector3.one * gizmosCubeSize);

        Vector3 bottomLeft = new Vector3(_cameraBounds.Left, _cameraBounds.Bottom, transform.position.z);
        Gizmos.DrawCube(bottomLeft, Vector3.one * gizmosCubeSize);

        Vector3 topRight = new Vector3(_cameraBounds.Right, _cameraBounds.Top, transform.position.z);
        Gizmos.DrawCube(topRight, Vector3.one * gizmosCubeSize);

        Vector3 bottomRight = new Vector3(_cameraBounds.Right, _cameraBounds.Bottom, transform.position.z); ;
        Gizmos.DrawCube(bottomRight, Vector3.one * gizmosCubeSize);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
