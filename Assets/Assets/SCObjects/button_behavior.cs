using System.ComponentModel.Design.Serialization;
using UnityEngine;

using UnityEngine.UIElements;


[CreateAssetMenu(menuName = "UI/Button Action", fileName = "NewButtonAction")]
public class button_behavior : ScriptableObject
{
    [SerializeField] private VisualTreeAsset SceneToGoTo;
    [SerializeField] private bool useLastScene;

    // Define a UnityEvent or virtual function to allow customization
    public virtual void Execute(UiController controller)
    {
        if (useLastScene)
        {
            controller.GoBack();
            return;
        }
        controller.ChangeSceneTo(SceneToGoTo);
    }
}
