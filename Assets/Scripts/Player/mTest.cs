using UnityEngine;
using UnityEngine.InputSystem;

public class mTest : MonoBehaviour
{
    private Playeractions mPlayeractions;
    [SerializeField]
    private float mMoveSpeed = 5f;

    private Rigidbody mRB;
    private Vector3 mMovementInput;

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
    }

    private void HandleMovement()
    {
        mRB.MovePosition(mRB.position + mMovementInput * mMoveSpeed * Time.fixedDeltaTime);
    }

    void OnDestroy()
    {
        mPlayeractions?.Dispose();
    }
}
