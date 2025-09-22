using System.ComponentModel.Design.Serialization;
using UnityEngine;

using UnityEngine.UIElements;


[CreateAssetMenu(menuName = "UI/Button Action", fileName = "NewButtonAction")]
public class button_behavior : ScriptableObject
{
    [SerializeField] private VisualTreeAsset SceneToGoTo;


    // Define a UnityEvent or virtual function to allow customization
    public virtual void Execute(UiController controller)
    {
        controller.ChangeSceneTo(SceneToGoTo);
    }
}
