using UnityEngine;

public class ProductObject : MonoBehaviour
{
    public bool isProduct = true;
    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;
    public float minYOffset = -1.0f; // How far below original y before reset
    public AudioClip fallSound; // Optional: assign in inspector

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (isProduct && transform.position.y < originalPosition.y + minYOffset)
        {
            // Play sound if assigned
            if (fallSound != null)
                AudioSource.PlayClipAtPoint(fallSound, transform.position);

            // Reset position and rotation
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            // Optionally, reset velocity if using Rigidbody
            var rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}