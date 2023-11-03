using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime
{
    /// <summary>
    /// ????: Slime
    /// ????: ?洢Slime???????????????????м???
    /// ???????ú?????????????(Slime)??????????CombinationCheck??????????
    /// </summary>
    
    // initialize function
    public Slime() { }

    // ??????????
    public Slime(int ID,int level, Color color)
    {
        this.ID = ID;
        this.level = level;
        this.color = color;
    }

    // ???????????
    public Slime(int ID, int level, Color color, int decorationIndex)
    {
        this.level = level;
        this.color = color;
        this.decorationIndex = decorationIndex;
    }
    // ???????????
    public Slime(int ID, int level, Color color, int decorationIndex, int attributeIndex)
    {
        this.level = level;
        this.color = color;
        this.decorationIndex = decorationIndex;
        this.attributeIndex= attributeIndex;
    }
    // ??????????????????
    public Slime(Slime slime1, Slime slime2) //need to specify id for new slime
    {
        if (slime1.GetSlimeLevel() == 1 || slime2.GetSlimeLevel() == 1) this.level = 2;
        else this.level = 3;
        
        //if (Random.Range(0f, 1f) > 0.9f)
        //{
        //    this.color = new Color(Random.Range(0, 256), Random.Range(0, 256), Random.Range(0, 256));
        //}
        //else
        //{
        //    this.color = (slime1.GetSlimeColor() + slime2.GetSlimeColor()) / 2;
        //}
        //a rare bug may happen when the color is set to white
        this.color = (slime1.GetSlimeColor() + slime2.GetSlimeColor()) / 2;
        this.decorationIndex = 0;
        this.attributeIndex = 0;
        if (this.level == 2)
        {
            this.decorationIndex = Random.Range(0, decorationSum);
            if (slime1.GetSlimeLevel() == 2)
            {
                this.decorationIndex = slime1.GetSlimeDecorationIndex();
            }
            if (slime2.GetSlimeLevel() == 2)
            {
                this.decorationIndex = slime2.GetSlimeDecorationIndex();
            }
        }
        if (this.level == 3)
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                this.decorationIndex = slime1.decorationIndex;
            }
            else
            {
                this.decorationIndex = slime2.decorationIndex;
            }
            this.attributeIndex = Random.Range(0, attributeSum);
        }
    }

    // private
    private int ID;
    private int level;                  // ????????
    private Color color;              // ?????????????RGBA??????д洢
    private int decorationIndex;        // ??????????????к?
    private int attributeIndex;         // ?????????????к?

    // public
    public int decorationSum = 9;   // ???????????????????
    public int attributeSum = 1;    // ??????????????????


    // function
    
    // Gets
    public int GetSlimeID()
    {
        return ID;
    }
    
    public int GetSlimeLevel()
    {
        return level;
    }



    public Color GetSlimeColor()
    {
        return color;
    }
    public int GetSlimeDecorationIndex()
    {
        return decorationIndex;
    }

    // Logic
    public bool CombinationCheck(Slime slime)
    {
        // ?????????
        if (this.level == 3 || slime.GetSlimeLevel() == 3)
        {
            return false;
        }
        return true;
    }

    public void UpdateDecorationIndex(int newDecorationIndex)
    {
        this.decorationIndex = newDecorationIndex;
    }
}
