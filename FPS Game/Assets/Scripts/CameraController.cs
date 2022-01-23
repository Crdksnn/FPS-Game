using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    [SerializeField] Transform target;

    private float startFOV;
    private float targetFOV;

    public float zoomSpeed = 3f;
    public Camera theCam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(transform.gameObject);
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void Start()
    {
        startFOV = theCam.fieldOfView;
        targetFOV = startFOV;
    }

    
    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);

    }

    public void ZoomIn(float newZoom)
    {
        targetFOV = newZoom;
    }

    public void ZoomOut()
    {
        targetFOV = startFOV;
    }

}
