using UnityEngine;

// Applies an explosion force to all nearby rigidbodies
public class Explosion : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;
    public bool explode = false;

    public AudioSource explosionSound;

    private int _count = 0;

    void Update()
    {
        if (explode && _count == 0)
        {
            _count++;

            Vector3 explosionPos = gameObject.transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            var particles = gameObject.GetComponentsInChildren<ParticleSystem>();

            foreach (var particle in particles)
            {
                particle.Play();
            }
            
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                }
            }

            FindObjectOfType<Camera>().GetComponent<ExplosionShake>().start = true;

            if (!explosionSound.isPlaying)
            {
                explosionSound.Play();
            }
            
            explode = false;
        }
    }
}