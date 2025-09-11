using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class StartPanelController : MonoBehaviour
{
    public Animator _backround;
    public Animator _aauLogo;
    public Animator _medialogyLogo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayAnimation()
    {
        _backround.Play("StartPanelAnimation");
        _aauLogo.Play("StartPaneAnimation");
        _medialogyLogo.Play("StartPaneAnimation");
    }
}
