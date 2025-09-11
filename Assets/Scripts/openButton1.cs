using JetBrains.Rider.Unity.Editor;
using UnityEngine;
public class openButton1 : MonoBehaviour
{
    public GameObject screen0;
    private bool iAmAktive = false;   
    public void changeThat()
    {
        if (iAmAktive == false)
        {
            screen0.SetActive(true);
            iAmAktive = true;
        }
        else if (iAmAktive == true)
        {
            screen0.SetActive(false);
            iAmAktive = false;
        }
    }
}
