using UnityEngine;

public class zoomInCamera : MonoBehaviour
{
    /*
     * Zooms camera by changing its FOV
     */

    public float speed;
    public float zoomTo;
    public bool shrink;

    private float _startingZoom;
    private float _resetPoint;

    void Start()
    {
        _startingZoom = Camera.main.fieldOfView;
        _resetPoint = _startingZoom;
    }

    void Update()
    {
        if (zoomTo < Camera.main.fieldOfView && shrink)
        {
            _startingZoom -= 1 * speed;
            Camera.main.fieldOfView = _startingZoom;
            transform.position = new Vector3(transform.position.x - (1 * speed), transform.position.y, transform.position.z);
        }
        else if (zoomTo >= Camera.main.fieldOfView && shrink)
        {
            var temp = _resetPoint;
            _resetPoint = zoomTo;
            zoomTo = temp;
            shrink = false;
        }

        if (zoomTo > Camera.main.fieldOfView && !shrink)
        {
            _startingZoom += 1 * speed;
            Camera.main.fieldOfView = _startingZoom;
            transform.position = new Vector3(transform.position.x + (1 * speed), transform.position.y, transform.position.z);
        }
        else if (zoomTo <= Camera.main.fieldOfView && !shrink)
        {
            shrink = true;
            var temp = _resetPoint;
            _resetPoint = zoomTo;
            zoomTo = temp;
        }
    }
}
