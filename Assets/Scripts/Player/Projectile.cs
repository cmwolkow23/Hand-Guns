using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public bool isRocket = false;
    public float rocketSpeed = 10f;
    public AudioClip explosionSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRocket)
        {
            // Implement rocket-specific behavior
            transform.position += transform.up * Time.deltaTime * rocketSpeed; // Example rocket movement
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Projectile collided with: " + collision.gameObject.name);
        if (Physics.SphereCastAll(transform.position, explosionRadius, Vector3.up).Length > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider hit in colliders)
            { 
                //Debug.Log("Applying explosion force to: " + hit.gameObject.name);
                if (hit.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionForce/2, ForceMode.Impulse);
                    //rb.AddForce(Vector3.up * explosionForce/50, ForceMode.Impulse);
                }
            }
        }
        if (TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            //audioSource.PlayOneShot(explosionSound);
            StartCoroutine(PlayExplosionSoundAndDestroy(audioSource));
        }
    }
    private IEnumerator PlayExplosionSoundAndDestroy(AudioSource audioSource)
    {
        audioSource.PlayOneShot(explosionSound, .6f);
        gameObject.GetComponent<MeshRenderer>().enabled = false; // Hide the projectile mesh
        gameObject.GetComponent<Collider>().enabled = false; // Disable the collider to prevent further collisions
        yield return new WaitForSeconds(explosionSound.length);
        Destroy(gameObject);
    }
}
