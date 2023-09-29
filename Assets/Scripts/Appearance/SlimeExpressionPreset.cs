using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SlimeExpressionPreset", menuName = "ScriptableObjects/SlimeExpressionPreset")]
public class SlimeExpressionPreset : ScriptableObject
{
    [Serializable]
    public struct ExpressionPreset
    {
        public string name;
        public Texture2D expressionTexture;
    }

    public ExpressionPreset[] presets;

    public bool GetExpressionTexture(string expName, out Texture2D targetTexture)
    {
        BuildHashMap();
        return _expressionByName.TryGetValue(expName, out targetTexture);
    }
    
    /// <summary>
    /// Build the look up dictionary if it has not been built
    /// So that expression controller script could get the expression texture by their name 
    /// </summary>
    private void BuildHashMap()
    {
        if (_expressionByName == null)
        {
            _expressionByName = new Dictionary<string, Texture2D>();
            
            foreach (var preset in presets)
            {
                _expressionByName[preset.name] = preset.expressionTexture;
            }
        }
    }

    private Dictionary<string, Texture2D> _expressionByName;
}