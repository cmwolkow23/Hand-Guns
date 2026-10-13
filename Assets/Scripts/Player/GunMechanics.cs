using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GunMechanics : MonoBehaviour
{
    private Playeractions mPlayeractions;
    public float explosionForce = 10f;
    public float forceMultiplier = 10f;
    private bool ShotLeft = true;
    public Animator Animator;
    [SerializeField]
    private AudioClip shootSound;
    private float shotVolume = 1f;
    private float shotCooldown = 0.5f; // Cooldown time in seconds
    private float shotCooldownTimer = 0f; // Timer to track cooldown
    private int ammoCapacity = 10;
    private int ammoCount = 10;
    private float reloadTime = 2f;
    public bool isNormalForce = true;
    public bool isExplosiveForce = false;
    public bool isProjectile = false;
    private bool isFiring = false;
    private GameObject projectilePrefab;
    private GameObject ArmPrefab;
    public GameObject projectilePoint;
    public GunChanger gunChanger;
    public List<GameObject> weapons;
    private int currentWeaponIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mPlayeractions = new Playeractions();
        mPlayeractions.player.Enable();

        gunChanger = GetComponentInParent<GunChanger>();

        mPlayeractions.player.Shoot.started += Shoot;
        mPlayeractions.player.Shoot.canceled += Shoot;
        mPlayeractions.player.Swap.performed += SwapWeapon;
        LoadWeapon();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (shotCooldownTimer > 0)
        {
            shotCooldownTimer -= Time.deltaTime;
        }
        //
        if (isFiring && shotCooldownTimer <= 0) 
        {
            Fire();
        }

    }

    private void Shoot(InputAction.CallbackContext context)
    {
        //Debug.Log("Shoot button pressed");
        // Implement shooting logic here

        if(context.started)
        {
            //Debug.Log("shot started");
            isFiring = true;
        }
        if(context.canceled)
        {
            //Debug.Log("shot canceled");
            isFiring = false;
        }

    }
    private void Fire()
    {
        shotCooldownTimer = shotCooldown; // Reset cooldown
        if (ShotLeft && Animator != null)
        {
            Animator.SetTrigger("ShotL");
            ShotLeft = false;
        }
        else
        {
            if (Animator != null)
            {
                Animator.SetTrigger("ShotR");
            }
            ShotLeft = true;
        }

        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            //Debug.Log("Playing shoot sound");
            audioSource.PlayOneShot(shootSound, shotVolume);
        }

        // Perform shooting action
        //Debug.Log("Shooting!");
        if (isProjectile && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectilePoint.transform.position, projectilePoint.transform.rotation);
            if (projectile.TryGetComponent<Rigidbody>(out Rigidbody projRb))
            {
                projRb.AddForce(transform.forward * 2f, ForceMode.Impulse);
            }
        }
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
        {
            if (hitInfo.collider.name == "Can")
            {
                if (hitInfo.transform.TryGetComponent<CanGame>(out CanGame canGame))
                {
                    canGame.score += 1;
                    canGame.scoreUI.text = $"Score: {canGame.score}";
                }
            }

            //Debug.Log($"Hit: {hitInfo.collider.name}");
            // Implement hit logic here
            if (hitInfo.transform.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (isExplosiveForce)
                {
                    rb.AddExplosionForce(explosionForce, hitInfo.point, 5f, 5f, ForceMode.Impulse);
                }
                if (isNormalForce)
                {
                    rb.AddForceAtPosition(transform.forward * forceMultiplier, hitInfo.point, ForceMode.Impulse);
                }

            }
            if (hitInfo.transform.TryGetComponent<GameActionTrigger>(out GameActionTrigger gat))
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
    private void LoadWeapon()
    {
        if(weapons.Count == 0)
        {
            Debug.LogWarning("No weapons assigned to the GunMechanics script.");
            return;
        }
        if (weapons[currentWeaponIndex].TryGetComponent<GunData>(out GunData gunData))
        {
            ArmPrefab = gunData.ArmPrefab;
            isNormalForce = gunData.isNormalForce;
            isExplosiveForce = gunData.isExplosiveForce;
            isProjectile = gunData.isProjectile;
            if(isProjectile)
            {
                projectilePrefab = gunData.projectilePrefab;
            }
            shotCooldown = gunData.fireRate;
            ammoCapacity = gunData.ammoCapacity;
            reloadTime = gunData.reloadTime;
            forceMultiplier = gunData.nForce;
            explosionForce = gunData.eForce;
            shootSound = gunData.ShotSound;
            shotVolume = gunData.AudioVolume;
            ammoCount = ammoCapacity; // Reset ammo count when switching weapons
            if(gunChanger != null && ArmPrefab != null)
            {
                gunChanger.ChangeGunModel(ArmPrefab);
            }
            shotCooldownTimer = 0.5f;
        }
    }
    private void SwapWeapon(InputAction.CallbackContext context)
    {
        // Implement weapon swapping logic here
        //Debug.Log("Swap weapon button pressed");
       if(context.performed)
        {
            // Disable current weapon
            weapons[currentWeaponIndex].SetActive(false);
            // Move to the next weapon
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            // Enable the new weapon
            weapons[currentWeaponIndex].SetActive(true);
            LoadWeapon();
        }


    }

    private void OnDisable()
    {
        mPlayeractions.player.Shoot.performed -= Shoot;
        mPlayeractions.Disable();

        // Dispose of animators to prevent crashes
        if (Animator != null)
        {
            Animator temp = Animator;
            Animator = null;
            temp.Rebind();
        }
    }

}
