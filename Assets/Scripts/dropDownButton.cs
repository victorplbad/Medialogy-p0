using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class dropDownButton : MonoBehaviour
{
    //public string screenName0;
    public GameObject openBox;

    Animator animator;
     public bool isOpen;

    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.Play("Closed", 0, 1f);
    }

    public void OpenClose() 
    {

        if (isOpen == false)
        {
            animator.SetBool("isOpen", true);
            openBox.SetActive(openBox == true);
            isOpen = true;
        }
        else if (isOpen == true)
        {
            animator.SetBool("isOpen", false);
            openBox.SetActive(openBox == false);
            isOpen = false;
        }
    }

    
  

    



}
