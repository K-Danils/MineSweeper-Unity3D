using UnityEngine;

public class moveCar : MonoBehaviour
{
    /*
     * A simple script to move cars in the scenes from one point to the other
     * The given object should have multiple children cars that are selected randomly
     * and enabled, then they are given trajectory and are again disabled once they have
     * reached their destination, repeating the process again, creating illusion of heavy traffic
     */
    public float distanceZ;
    public float speed;
    
    private int _num;
    private float _startLocation;

    void Start()
    {
        _startLocation = transform.position.z;
        _num = Random.Range(0, gameObject.transform.childCount);
        gameObject.transform.GetChild(_num).gameObject.SetActive(true);
    }

    void Update()
    {
        if (distanceZ <= transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1 * speed);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _startLocation);
            gameObject.transform.GetChild(_num).gameObject.SetActive(false);
            _num = Random.Range(0, gameObject.transform.childCount);
            gameObject.transform.GetChild(_num).gameObject.SetActive(true);
        }
    }
}
