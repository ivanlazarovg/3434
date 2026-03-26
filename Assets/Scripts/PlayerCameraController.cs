using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float minYRotation;
    [SerializeField] float maxYRotation;
    [SerializeField] float enemaRecoilMultiplier;

    GameObject CameraHolder;
    Vector3 addedRotationCam;
    Vector3 addedRotation;
    float currentYRot;

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

        currentYRot += (addedRotationCam + (controller.enemaVolatileOffset * enemaRecoilMultiplier)).x * Time.deltaTime;
        currentYRot = Mathf.Clamp(currentYRot, minYRotation, maxYRotation);

        this.transform.eulerAngles += (addedRotation + controller.enemaVolatileOffset * enemaRecoilMultiplier) * Time.deltaTime;
    }

    private void LateUpdate()
    {
        CameraHolder.transform.eulerAngles = new Vector3(currentYRot, this.transform.eulerAngles.y, 0);

    }
}
