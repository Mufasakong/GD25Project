using UnityEngine;
using UnityEngine.UI;

public class DEBUGSLIDER : MonoBehaviour
{
    public Slider attackSlider;

    void Start() {
        attackSlider = GetComponent<Slider>();
    }

    void Update()
    {
        Debug.Log("Slider value: " + attackSlider.value);
    }
}
