using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float minYRotation;
    [SerializeField] float maxYRotation;

    GameObject CameraHolder;
    Vector3 addedRotationCam;
    Vector3 addedRotation;

    PlayerMovementController controller;

    void Start()
    {
        CameraHolder = this.gameObject.transform.GetChild(0).gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        addedRotation = Vector3.zero;
        addedRotationCam = Vector3.zero;

        addedRotation += Vector3.up * Input.GetAxis("Mouse X") * rotationSpeed;
        addedRotationCam += Vector3.left * Input.GetAxis("Mouse Y") * rotationSpeed;

        this.transform.eulerAngles += (addedRotation + controller.enemaVolatileOffset * 15) * Time.deltaTime;
        CameraHolder.transform.eulerAngles += (addedRotationCam + (controller.enemaVolatileOffset * 15))* Time.deltaTime;

    }
}
