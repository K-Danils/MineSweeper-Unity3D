using UnityEngine;

public class UIAppear : MonoBehaviour
{
    /*
     * Make UI elements appear by changing its Alpha attribute
     */
    public float speed = 0.1f;
    private CanvasGroup _image;

    void Start()
    {
        _image = transform.GetChild(0).gameObject.GetComponent<CanvasGroup>();
        _image.alpha = 0f;

        transform.GetChild(0).gameObject.SetActive(true);
        gameObject.SetActive(true);

        for (int i = 1; i < transform.childCount; i++)
        {
           transform.GetChild(i).gameObject.SetActive(false);
        } 
    }

    void Update()
    {
        if (_image.alpha < 1f)
        {
            _image.alpha += speed;
        }
        else if (_image.alpha >= 1f)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
