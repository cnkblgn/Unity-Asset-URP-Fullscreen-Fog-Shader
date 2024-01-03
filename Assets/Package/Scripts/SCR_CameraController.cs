using UnityEngine;

public class SCR_CameraController : MonoBehaviour
{
    [field: Header("_")]
    [field: SerializeField] private bool useAcceleration = false;

    [field: Space(10), Header("_")]
    [field: SerializeField, Min(0.01f)] private float lookSpeed = 1.0f;
    [field: SerializeField, Min(1.0f)] private float lookAcceleration = 50.0f;

    [field: Space(10), Header("_")]
    [field: SerializeField, Min(1.0f)] private float moveSpeed = 10.0f;
    [field: SerializeField, Min(1.0f)] private float moveAcceleration = 25.0f;

    private Transform thisTransform = null;
    private Vector3 currentMovementVelocity = Vector3.zero;
    private Vector3 targetMovementVelocity = Vector3.zero;
    private Vector2 currentLookVelocity = Vector2.zero;
    private Vector2 targetLookVelocity = Vector2.zero;
    private Vector2 inputMove = Vector2.zero;
    private Vector2 inputLook = Vector2.zero;
    private int inputAltidute = 0;
    private float inputScroll = 0.0f;
    private float speedMultiplier = 1.0f;

    private void Awake() => thisTransform = GetComponent<Transform>();
    private void Update()
    {
        inputScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        inputAltidute = Input.GetKey(KeyCode.Q) ? 1 : Input.GetKey(KeyCode.E) ? -1 : 0;
        inputLook = new(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        inputMove = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputMove = inputMove.normalized;

        speedMultiplier = Mathf.Max(0.1f, speedMultiplier + (inputScroll > 0 ? 0.1f : inputScroll < 0 ? -0.1f : 0));
        targetMovementVelocity = speedMultiplier * moveSpeed * (Vector3.up * inputAltidute + thisTransform.TransformVector(inputMove.x, 0, inputMove.y));

        if (useAcceleration)
        {
            currentMovementVelocity = Vector3.Lerp(currentMovementVelocity, targetMovementVelocity, moveAcceleration * Time.deltaTime);
        }
        else
        {
            currentMovementVelocity = targetMovementVelocity;
        }

        transform.position += currentMovementVelocity * Time.deltaTime;
    }
    private void LateUpdate()
    {
        targetLookVelocity = new(inputLook.x * lookSpeed, -inputLook.y * lookSpeed);

        if (useAcceleration)
        {
            currentLookVelocity = Vector2.Lerp(currentLookVelocity, targetLookVelocity, lookAcceleration * Time.deltaTime);
        }
        else
        {
            currentLookVelocity = targetLookVelocity;
        }

        thisTransform.rotation *= Quaternion.AngleAxis(currentLookVelocity.y, Vector3.right);
        thisTransform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + currentLookVelocity.x, transform.eulerAngles.z);
    }
}
