using UnityEngine;
using System.Collections;

public class ProductSpawning : MonoBehaviour
{
    public GameObject firstGroup;
    public GameObject secondGroup;

    // Call this from your button's OnClick event
    public void FadeSequence()
    {
        StartCoroutine(FadeGroups());
    }

    private IEnumerator FadeGroups()
    {
        yield return StartCoroutine(FadeGroup(firstGroup, false, 1f));
        yield return StartCoroutine(FadeGroup(secondGroup, true, 1f));
    }

    // Fades all child renderers in the group and enables/disables colliders
    private IEnumerator FadeGroup(GameObject group, bool fadeIn, float duration)
    {
        Renderer[] renderers = group.GetComponentsInChildren<Renderer>();
        Collider[] colliders = group.GetComponentsInChildren<Collider>();

        // Enable/disable colliders at the start of fade
        foreach (Collider col in colliders)
        {
            col.enabled = fadeIn;
        }

        float timer = 0f;
        while (timer < duration)
        {
            float alpha = fadeIn ? timer / duration : 1f - (timer / duration);
            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    Color c = mat.color;
                    c.a = alpha;
                    mat.color = c;
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }
        // Ensure final alpha
        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                Color c = mat.color;
                c.a = fadeIn ? 1f : 0f;
                mat.color = c;
            }
        }
    }
}
