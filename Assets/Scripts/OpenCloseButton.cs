using UnityEngine;

public class OpenCloseButton : MonoBehaviour
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
