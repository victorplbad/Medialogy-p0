using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class dropDownButton : MonoBehaviour
{

    Animator animator;
    //public bool isOpen;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Closed", 0, 1f);
    }


    private void Update()
    {
       // animator.SetBool(isOpen false);
    }


    
    

}
