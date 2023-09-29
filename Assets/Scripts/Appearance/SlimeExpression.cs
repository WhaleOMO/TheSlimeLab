using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeExpression : MonoBehaviour
{
    public SlimeExpressionPreset expressions;

    private Material _mat;
    private Animator _animator;
    
    private void OnEnable()
    {
        _mat = GetComponent<MeshRenderer>().material;
        _animator = GetComponent<Animator>();
    }
    
    public void DisplaySingleExpression(string expressionName)
    {
        if (expressions.GetExpressionTexture(expressionName, out var targetTexture))
        {
            _mat.SetTexture(SlimeShaderProperties.ExpressionTex, targetTexture);
        }
    }

    public void PlayAnimationState(string stateName)
    {
        _animator.Play("Base Layer." + stateName);
    }
}
