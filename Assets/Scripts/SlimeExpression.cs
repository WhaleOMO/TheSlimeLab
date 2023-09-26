using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeExpressionPreset : ScriptableObject
{
    public struct ExpressionPreset
    {
        public string Name;
        public Texture2D ExpressionTexture;
    }

    public ExpressionPreset[] presets;
}

public class SlimeExpression : MonoBehaviour
{
    
}
