using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager
{
    /// <summary>
    /// 类名: SlimeManager
    /// 类功能: 存储Slime的各项数据并且针对合成进行计算
    /// 注意：在调用合成方法的初始化器(SlimeManager)之前请务必调用CombinationCheck进行属性检查
    /// </summary>
    
    // initialize function
    public SlimeManager() { }

    // 一级初始化器
    public SlimeManager(int level, Vector4 color)
    {
        this.level = level;
        this.color = color;
    }

    // 二级初始化器
    public SlimeManager(int level, Vector4 color, int decorationIndex)
    {
        this.level = level;
        this.color = color;
        this.decorationIndex = decorationIndex;
    }
    // 三级初始化器
    public SlimeManager(int level, Vector4 color, int decorationIndex, int attributeIndex)
    {
        this.level = level;
        this.color = color;
        this.decorationIndex = decorationIndex;
        this.attributeIndex= attributeIndex;
    }
    // 两只史莱姆的合并信息计算
    public SlimeManager(SlimeManager slime1, SlimeManager slime2)
    {
        this.level = ((slime1.GetSlimeLevel() - 1) & (slime2.GetSlimeLevel() - 1)) + 1;
        
        if (Random.Range(0f, 1f) > 0.5f)
        {
            this.color = new Vector4(Random.Range(0, 256), Random.Range(0, 256), Random.Range(0, 256), Random.Range(0, 256));
        }
        else
        {
            this.color = (slime1.GetSlimeColor() + slime2.GetSlimeColor()) / 2;
        }
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
    private int level;                  // 史莱姆的等级
    private Vector4 color;              // 史莱姆的颜色，以RGBA格式进行存储
    private int decorationIndex;        // 史莱姆的装饰物序列号
    private int attributeIndex;         // 史莱姆的特性序列号

    // public
    public int decorationSum = 1;   // 史莱姆的装饰物总种类数
    public int attributeSum = 1;    // 史莱姆的特征总种类数


    // function
    
    // Gets
    public int GetSlimeLevel()
    {
        return level;
    }

    public Vector4 GetSlimeColor()
    {
        return color;
    }
    public int GetSlimeDecorationIndex()
    {
        return decorationIndex;
    }

    // Logic
    public bool CombinationCheck(SlimeManager slime)
    {
        // 三级则不可合成
        if (this.level == 3 || slime.GetSlimeLevel() == 3)
        {
            return false;
        }
        return true;
    }
}
