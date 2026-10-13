using UnityEngine;

public class GunChanger : MonoBehaviour
{
    public void ChangeGunModel(GameObject newGunModel)
    {
        // Destroy the current gun model if it exists
        foreach (Transform child in transform)
        {
            if(child.CompareTag("arm"))
            {
                Destroy(child.gameObject);
            }
        }
        // Instantiate the new gun model as a child of this GameObject
        GameObject newGun = Instantiate(newGunModel, transform.position, transform.rotation);
        newGun.transform.SetParent(transform);
        AssignAnimator();
    }
    public void AssignAnimator()
    {
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            GunMechanics gunMechanics = GetComponentInChildren<GunMechanics>();
            if (gunMechanics != null)
            {
                gunMechanics.Animator = animator;
            }
        }
    }
}
