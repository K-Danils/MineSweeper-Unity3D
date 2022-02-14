using UnityEngine;

public class SlowlyDisappear : MonoBehaviour
{
    /*
     * Make a gameObject disappear by changing it's scale slowly down to zero, once it has reached 0, it gets deleted
     */
    void Update()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
