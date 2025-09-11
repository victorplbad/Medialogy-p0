using UnityEngine;

public class openButton : MonoBehaviour
{

    public string screenName0;
    public string screenName1;
   /* public string screenName2;
    public string screenName3;
    public string screenName4;
    public string screenName5;
    public string screenName6;
   */
    
    public GameObject screen0;
    public GameObject screen1;
    /*public GameObject screen2;
    public GameObject screen3;
    public GameObject screen4;
    public GameObject screen5;
    public GameObject screen6;*/
    public void changeThat(string nameOfPanel)
    {
        screen0.SetActive(nameOfPanel == screenName0);
        screen1.SetActive(nameOfPanel == screenName1);
        /*screen2.SetActive(nameOfPanel == screenName2);
        screen3.SetActive(nameOfPanel == screenName3);
        screen4.SetActive(nameOfPanel == screenName4);
        screen5.SetActive(nameOfPanel == screenName5);
        screen6.SetActive(nameOfPanel == screenName6);
        */
       
       

    }




}
