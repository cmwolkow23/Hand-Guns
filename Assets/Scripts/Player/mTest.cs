using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class mTest : MonoBehaviour
{
    private Playeractions mPlayeractions;
    [SerializeField]
    private float mMoveSpeed = 5f;
    [SerializeField]
    private float jumpPower = 5f;
    public bool grounded = false;

    private Rigidbody mRB;
    private Vector3 mMovementInput;
    private Quaternion mLookInput;
    [SerializeField]
    private CinemachinePanTilt look;

    void Start()
    {
        mPlayeractions = new Playeractions();
        mPlayeractions.player.Enable();

        mRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ReadMovementInput();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void ReadMovementInput()
    {
        Vector2 moveInput = mPlayeractions.player.move.ReadValue<Vector2>();
        mMovementInput = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        mLookInput = Quaternion.Euler(0f, look.PanAxis.Value, 0f);
        transform.rotation = mLookInput;
        if (mPlayeractions.player.Jump.IsPressed() && grounded == true)
        {
            grounded = false;
            mRB.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void HandleMovement()
    {
        // Transform movement input to be relative to where the player is looking
        Vector3 relativeMovement = transform.TransformDirection(mMovementInput);
        relativeMovement.y = 0f; // Keep movement on horizontal plane only

        mRB.MovePosition(mRB.position + relativeMovement * mMoveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    void OnDestroy()
    {
        mPlayeractions?.Dispose();
    }
}
