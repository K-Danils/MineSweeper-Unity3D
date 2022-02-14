using UnityEngine;

public class moveEasterEgg : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
