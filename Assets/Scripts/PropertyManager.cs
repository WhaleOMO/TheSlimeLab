using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    public Color
        MaterialColor,
        AmbientColor,
        HighlightColor,
        RimColor,
        EyeColor,
        MouthColor;
    public int ShakeSpeed;
    public Texture SlimeExpression;


    private void OnValidate()
    {
        //Get the renderer of the object
        Renderer renderer = GetComponentInChildren<Renderer>();

        //Get the material of the renderer
        Material mat = renderer.material;

        //Set the Color property
        mat.SetColor("_Color", MaterialColor);
        mat.SetColor("_AmbientColor", AmbientColor);
        mat.SetColor("_HighlightColor", HighlightColor);
        mat.SetColor("_RimColor", RimColor);
        mat.SetColor("_EyeColor", EyeColor);
        mat.SetColor("_MouthColor", MouthColor);
        mat.SetInt("_ShakeSpeed", ShakeSpeed);
        mat.SetTexture("_SlimeExpression", SlimeExpression);




        //Reassign the material to the renderer
        renderer.material = mat;
    }
}
