using UnityEngine;
using UnityEngine.InputSystem;

public class GunMechanics : MonoBehaviour
{
    private Playeractions mPlayeractions;
    public float explosionForce = 10f;
    public float forceMultiplier = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mPlayeractions = new Playeractions();
        mPlayeractions.player.Enable();

        mPlayeractions.player.Shoot.performed += Shoot;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot button pressed");
        // Implement shooting logic here

        if(context.performed)
        {
            // Perform shooting action
            Debug.Log("Shooting!");
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
            {
                if(hitInfo.collider.name == "Can")
                {
                    if(hitInfo.transform.TryGetComponent<CanGame>(out CanGame canGame))
                    {
                        canGame.score += 1;
                        canGame.scoreUI.text = $"Score: {canGame.score}";
                    }
                }

                Debug.Log($"Hit: {hitInfo.collider.name}");
                // Implement hit logic here
                if(hitInfo.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.AddExplosionForce(explosionForce, hitInfo.point, 5f, 1f, ForceMode.Impulse);
                    rb.AddForceAtPosition(transform.forward * forceMultiplier, hitInfo.point, ForceMode.Impulse);
                }
                if(hitInfo.transform.TryGetComponent<GameActionTrigger>(out GameActionTrigger gat))
                {
                    if (!gat.ignoreShootRaycast)
                        return;
                    else
                    {
                        gat.enterActions.ForEach(action => action.Action());
                    }
                }
            }

        }

    }


}
