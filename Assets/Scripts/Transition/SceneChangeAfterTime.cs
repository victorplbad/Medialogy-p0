using TMPro;
using UnityEngine;

public class SceneChangeAfterTime : SceneChanger
{
    [Header("Scene Timer Settings")]
    [SerializeField] private float _timeBeforeChange = 3f;
    [SerializeField] private bool _skipOnTouch = true;

    [SerializeField] private TextMeshProUGUI _countdownText;

    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_countdownText != null)
        {
            float timeLeft = Mathf.Max(0f, _timeBeforeChange - _timer);
            _countdownText.text = timeLeft.ToString("F1");
        }

        if (_skipOnTouch && Input.touchCount > 0)
        {
            RequestSceneAction();
            enabled = false;
        }

        if (_timer >= _timeBeforeChange)
        {
            RequestSceneAction();
            enabled = false;
        }
    }
}
