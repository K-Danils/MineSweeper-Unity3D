using UnityEngine;

public class HeadLineAppear : MonoBehaviour
{
    /*
     * Makes head line appear and move into its position during game over or victory screen
     */
    public float destination;
    public float speed;
    public float movementSpeed;
    CanvasGroup cg;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    void Update()
    {
        if (cg.alpha < 1f)
        {
            cg.alpha += speed;
        }

        if (transform.position.y > destination)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1 * movementSpeed, transform.position.z);
        }
    }
}
