using UnityEngine;

public class GameCamera
{
    private CameraHolder _cameraHolder;
    
    public void SetCamera()
    {
        var holder = new GameObject("Camera");
        _cameraHolder = holder.AddComponent<CameraHolder>();

        var cameraPrefab = R.Camera;
        var cameraObject = Object.Instantiate(cameraPrefab, holder.transform, false);
        cameraObject.name = "MainCamera";
        var camera = cameraObject.GetComponent<Camera>();
        
        _cameraHolder.cam = camera;
    }

    public Camera GetCamera() => _cameraHolder.cam;
}