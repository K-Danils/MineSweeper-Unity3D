using UnityEngine;

public class ButtonAppear : MonoBehaviour
{
    /*
     * Makes UI buttons appear during game over or victory scenes
     */
    public float speed;
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
    }
}
