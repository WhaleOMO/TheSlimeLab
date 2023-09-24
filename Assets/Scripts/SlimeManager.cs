using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeManager
{
    /// <summary>
    /// ����: SlimeManager
    /// �๦��: �洢Slime�ĸ������ݲ�����Ժϳɽ��м���
    /// ע�⣺�ڵ��úϳɷ����ĳ�ʼ����(SlimeManager)֮ǰ����ص���CombinationCheck�������Լ��
    /// </summary>
    
    // initialize function
    public SlimeManager() { }

    // һ����ʼ����
    public SlimeManager(int level, Vector4 color)
    {
        this.level = level;
        this.color = color;
    }

    // ������ʼ����
    public SlimeManager(int level, Vector4 color, int decorationIndex)
    {
        this.level = level;
        this.color = color;
        this.decorationIndex = decorationIndex;
    }
    // ������ʼ����
    public SlimeManager(int level, Vector4 color, int decorationIndex, int attributeIndex)
    {
        this.level = level;
        this.color = color;
        this.decorationIndex = decorationIndex;
        this.attributeIndex= attributeIndex;
    }
    // ��ֻʷ��ķ�ĺϲ���Ϣ����
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
    private int level;                  // ʷ��ķ�ĵȼ�
    private Vector4 color;              // ʷ��ķ����ɫ����RGBA��ʽ���д洢
    private int decorationIndex;        // ʷ��ķ��װ�������к�
    private int attributeIndex;         // ʷ��ķ���������к�

    // public
    public int decorationSum = 1;   // ʷ��ķ��װ������������
    public int attributeSum = 1;    // ʷ��ķ��������������


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
        // �����򲻿ɺϳ�
        if (this.level == 3 || slime.GetSlimeLevel() == 3)
        {
            return false;
        }
        return true;
    }
}
