using UnityEngine;

public class CameraController : MonoBehaviour
{
  public static CameraController Instance { get; private set; }

  // Main camera variables
  GameObject cameraPivotReference;
  public GameObject mainCameraPivot;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  public void LoadCamera(string sceneId)
  {
    cameraPivotReference = GameObject.Find(sceneId + "_camera_reference");

    if (cameraPivotReference == null)
    {
      Debug.LogError("Camera reference not found!!! " + sceneId + "_cameraReference");
      return;
    }

    Debug.Log("Camera reference Found! " + sceneId + "_cameraReference");

    mainCameraPivot.transform.position = new Vector3(cameraPivotReference.transform.position.x, cameraPivotReference.transform.position.y, cameraPivotReference.transform.position.z);
  }
}
