using UnityEngine;
using UnityEngine.UIElements;

public class ButtonDropDown : MonoBehaviour
{
    private UIDocument _uiDocument;

    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();

        _uiDocument.rootVisualElement.Q<Button>("DropdownButton").clicked += () =>
        {
            var menu = _uiDocument.rootVisualElement.Q<VisualElement>("DropdownMenu");
            menu.style.display = menu.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
        };
    }
}
