using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    private TMPro.TextMeshProUGUI textComp;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        textComp = GetComponent<TMPro.TextMeshProUGUI>();
        SetText(slider.value);
        slider.onValueChanged.AddListener(SetText);
    }

    private void SetText(float value)
    {
        textComp.text = value + "";
    }
}
