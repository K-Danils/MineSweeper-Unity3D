using System.Collections.Generic;
using UnityEngine;

public class MakeBloodSplatter : MonoBehaviour
{
    /*
     * Creates bloodsplatters where npcs are standing
     */
    public GameObject blood1;
    public GameObject blood2;
    public GameObject blood3;
    public GameObject objectToTrack;

    private List<GameObject> _blood;
    private int bleed = 0;

    private void Start()
    {
        bleed = 1;
        _blood = new List<GameObject>();
        _blood.Add(blood1);
        _blood.Add(blood2);
        _blood.Add(blood3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bleed == 1)
        {
            GameObject currentBlood = _blood[Random.Range(0, 3)];
            Instantiate(currentBlood, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), currentBlood.transform.rotation);
            bleed++;
        }
    }
}
