using System.Collections;
using UnityEngine;

public class ExplosionShake : MonoBehaviour
{
    /*
     * Shakes camera during explosion
     */
    public float duration = 1f;
    public AnimationCurve animationCurve;
    public bool start = false;

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float strength = animationCurve.Evaluate(elapsedTime / duration) * 5;

            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
    }

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }
}
