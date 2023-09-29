using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public Slider R_color;
    public Slider G_color;
    public Slider B_color;

    public InputField R_InputField;
    public InputField G_InputField;
    public InputField B_InputField;

    public void Update()
    {
        R_InputField.text = R_color.value.ToString();
        G_InputField.text = G_color.value.ToString();
        B_InputField.text = B_color.value.ToString();
    }
}
