using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class mathButton : MonoBehaviour
{

    public TextMeshProUGUI voresText;

    public int startNr = 10;
    private int currentNr;
    
    //public button button2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentNr = startNr;
        updateNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
        

    }


    public void minusNumber(int numberToMinus)
    {
        currentNr = currentNr - numberToMinus;
        updateNumber() ;
    }

    public void plusNumber(int numberToPlus)
    {
        currentNr = currentNr + numberToPlus;
        updateNumber();
    }



    public void updateNumber()
    {

        voresText.text = currentNr.ToString();

    }

}
