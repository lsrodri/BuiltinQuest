using UnityEngine;

public class FlyBack : MonoBehaviour
{
    private System.Collections.Generic.HashSet<GameObject> flyingBackObjects = new System.Collections.Generic.HashSet<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        ProductObject product = collision.gameObject.GetComponent<ProductObject>();
        if (product != null && product.isProduct && !flyingBackObjects.Contains(collision.gameObject))
        {
            flyingBackObjects.Add(collision.gameObject);
            StartCoroutine(FlyBackRoutine(collision.gameObject, product.originalPosition, product.originalRotation));
        }
    }

    private System.Collections.IEnumerator FlyBackRoutine(GameObject obj, Vector3 targetPosition, Quaternion targetRotation)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Color originalColor = renderer ? renderer.material.color : Color.white;
        Color flashColor = Color.yellow;
        if (renderer)
            renderer.material.color = flashColor;

        Vector3 originalScale = obj.transform.localScale;
        Vector3 flashScale = originalScale * 1.2f;
        obj.transform.localScale = flashScale;

        Collider col = obj.GetComponent<Collider>();
        if (col) col.enabled = false;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        yield return new WaitForSeconds(1f);

        // Nudge up to avoid ground/collider overlap
        Vector3 safePosition = targetPosition + Vector3.up * 0.05f;
        obj.transform.position = safePosition;
        obj.transform.rotation = targetRotation;

        if (renderer)
            renderer.material.color = originalColor;
        obj.transform.localScale = originalScale;

        if (col) col.enabled = true;
        if (rb)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        flyingBackObjects.Remove(obj);
    }
}