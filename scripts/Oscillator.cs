using UnityEngine;

public class Oscillator : MonoBehaviour
{
    /*
     * Creates Eliptical movement for a given gameObject
     */
    public float speed;
    public float width;
    public float height;

    private float _timeCounter;

    void Update()
    {
        _timeCounter += Time.deltaTime * speed;

        float x = Mathf.Cos(_timeCounter) * width;
        float z = Mathf.Sin(_timeCounter) * height;
        float y = transform.position.y;

        transform.position = new Vector3(x,y,z);
    }
}
