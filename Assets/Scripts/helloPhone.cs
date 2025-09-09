using UnityEngine;
using UnityEngine.InputSystem;

public class helloPhone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("hello");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey(KeyCode.E))
        {
            Debug.Log("you pressed E");
        }
        */

        if (Input.GetKeyDown("e"))
        {
            Debug.Log("e key was pressed");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space key was pressed");
        }
    }
}
