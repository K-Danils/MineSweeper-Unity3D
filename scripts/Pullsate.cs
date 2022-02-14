using UnityEngine;

public class Pullsate : MonoBehaviour
{
    /*
     * Create Pulsating effect by scalling object down and back to it's original scale
     */
    public float from;
    public float to;
    public float speed;

    private bool _shrinking;
    private float _resetPoint;

    void Start()
    {
        _resetPoint = from;
        _shrinking = false;
        transform.localScale = new Vector3(from, from, from);
    }

    void Update()
    {
        if (transform.localScale.x <= to && !_shrinking)
        {
            from += 1 * speed;
            transform.localScale = new Vector3(from, from, from );
        }
        else if(transform.localScale.x >= to && !_shrinking)
        {
            _shrinking = true;
            var temp = _resetPoint;
            _resetPoint = to;
            to = temp;
        }

        if (transform.localScale.x >= to && _shrinking)
        {
            from -= 1 * speed;
            transform.localScale = new Vector3(from, from, from);
        }
        else if (transform.localScale.x <= to && _shrinking)
        {
            _shrinking = false;
            var temp = _resetPoint;
            _resetPoint = to;
            to = temp;
        }
    }
}
