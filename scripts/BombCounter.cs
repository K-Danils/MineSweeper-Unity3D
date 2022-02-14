using UnityEngine;
using TMPro;

public class BombCounter : MonoBehaviour
{
    /*
     * A simple counter that shows how many boms are still left 
     */
    public int toAddTakeAway;
    public GameObject gm;

    void Update()
    {
        gameObject.GetComponent<TMP_Text>().text = "Bombs left: " + (gm.GetComponent<GameLogic>().minesToUncover + toAddTakeAway).ToString();
    }
}
