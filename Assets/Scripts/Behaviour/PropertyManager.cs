using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SlimeShaderProperties
{
    // Slime Colors 
    public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    public static readonly int AmbientColor = Shader.PropertyToID("_AmbientColor");
    public static readonly int HighlightColor = Shader.PropertyToID("_HighlightColor");
    public static readonly int RimColor = Shader.PropertyToID("_RimColor");
    // Slime Expression
    public static readonly int ExpColorLayer1 = Shader.PropertyToID("_ExpColorLayer1");
    public static readonly int ExpColorLayer2 = Shader.PropertyToID("_ExpColorLayer2");
    public static readonly int ExpColorLayer3 = Shader.PropertyToID("_ExpColorLayer3");
    // Slime Outline
    public static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
    // Slime Expression
    public static readonly int ExpressionTex = Shader.PropertyToID("_ExpressionTex");
}

[ExecuteInEditMode]
public class PropertyManager : MonoBehaviour
{
    [SerializeField]
    private Color
        MaterialColor,
        AmbientColor,
        HighlightColor,
        EyeColor,
        MouthColor,
        OutlineColor;

    [ColorUsage(true, true)] private Color RimColor;
    public int ShakeSpeed;
    public Texture SlimeExpression;

    private void OnEnable()
    {
        //Get the renderer of the object
        Renderer renderer = GetComponentInChildren<Renderer>();

        //Get the material of the renderer
        Material mat = renderer.material;

        this.MaterialColor = mat.GetColor(SlimeShaderProperties.BaseColor);
        this.AmbientColor = mat.GetColor(SlimeShaderProperties.AmbientColor);
        this.HighlightColor = mat.GetColor(SlimeShaderProperties.HighlightColor);
        this.RimColor = mat.GetColor(SlimeShaderProperties.RimColor);
        this.EyeColor = mat.GetColor(SlimeShaderProperties.ExpColorLayer1);
        this.MouthColor = mat.GetColor(SlimeShaderProperties.ExpColorLayer2);
        this.OutlineColor = mat.GetColor(SlimeShaderProperties.OutlineColor);
        this.SlimeExpression = mat.GetTexture(SlimeShaderProperties.ExpressionTex);
    }

    private void Update()
    {
        //Get the renderer of the object
        Renderer renderer = GetComponentInChildren<Renderer>();

        //Get the material of the renderer
        Material mat = renderer.material;

        //Set the Color property
        mat.SetColor(SlimeShaderProperties.BaseColor, MaterialColor);
        mat.SetColor(SlimeShaderProperties.AmbientColor, AmbientColor);
        mat.SetColor(SlimeShaderProperties.HighlightColor, HighlightColor);
        mat.SetColor(SlimeShaderProperties.RimColor, RimColor);
        mat.SetColor(SlimeShaderProperties.ExpColorLayer1, EyeColor);
        mat.SetColor(SlimeShaderProperties.ExpColorLayer2, MouthColor);
        mat.SetColor(SlimeShaderProperties.OutlineColor, OutlineColor);
        mat.SetTexture(SlimeShaderProperties.ExpressionTex, SlimeExpression);
        
        //Reassign the material to the renderer
        renderer.material = mat;
    }
}
