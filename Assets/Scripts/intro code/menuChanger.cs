using UnityEngine;

public class menuChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject videoMenu;
    public GameObject mathMenu;
    public GameObject foodMenu;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void changeThat(string nameOfPanel)
    {
        videoMenu.SetActive(nameOfPanel == "video");
        mathMenu.SetActive(nameOfPanel == "math");
        foodMenu.SetActive(nameOfPanel == "food");

    }

}
