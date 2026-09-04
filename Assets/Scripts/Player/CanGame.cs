using TMPro;
using UnityEngine;

public class CanGame : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion rotation;
    public GameObject block;
    private Rigidbody rb;
    public TextMeshProUGUI scoreUI;
    public int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        rotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        block.SetActive(true );
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.linearVelocity.magnitude > 2f && block.activeInHierarchy)
        {
            // Implement can game logic here
            block.SetActive(false );
            scoreUI.gameObject.SetActive(true );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Reset the can's position and velocity when it hits the ground
            block.SetActive(true);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = startPos;
            transform.rotation = rotation;
            score = 0;
            scoreUI.gameObject.SetActive(false);

        }
    }
}
