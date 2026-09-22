using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameCamera
{
    public event Action<Vector3> OnCameraMoved;
        
    private CameraHolder _cameraHolder;
    
    public void SetCamera()
    {
        var holder = new GameObject("Camera");
        holder.gameObject.transform.position = new Vector3(0, 0, -10);
        _cameraHolder = holder.AddComponent<CameraHolder>();
        
        var cameraPrefab = R.Camera;
        var cameraObject = Object.Instantiate(cameraPrefab, holder.transform, false);
        cameraObject.name = "MainCamera";
        var camera = cameraObject.GetComponent<Camera>();
        
        _cameraHolder.cam = camera;
    }
    
    public Camera GetCamera() => _cameraHolder.cam;
    
    public void MoveCamera(Vector3 to)
    { _cameraHolder.gameObject.transform.position = to; OnCameraMoved?.Invoke(to); }
    
    public Vector3 GetCameraPosition() => _cameraHolder.transform.position;
}
