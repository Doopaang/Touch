using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dial : MonoBehaviour
{
    public List<Text> answerText;

    [HideInInspector]
    public int[] answer;
    private int index;

    void Start()
    {
        answer = new int[answerText.Count];
        
        Clear();
    }

    public void One()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 1;
        
        Print();
    }

    public void Two()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 2;
        
        Print();
    }

    public void Three()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 3;
        
        Print();
    }

    public void Four()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 4;
        
        Print();
    }

    public void Five()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 5;
        
        Print();
    }

    public void Six()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 6;
        
        Print();
    }

    public void Seven()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 7;
        
        Print();
    }

    public void Eight()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 8;
        
        Print();
    }

    public void Nine()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 9;
        
        Print();
    }

    public void Zero()
    {
        if (index >= answerText.Count)
        {
            return;
        }
        answer[index++] = 0;
        
        Print();
    }

    public void Clear()
    {
        for (int count = 0; count < answer.Length; count++)
        {
            answer[count] = -1;
        }
        index = 0;
        
        Print();
    }

    public void Erase()
    {
        if (index <= 0)
        {
            return;
        }
        answer[--index] = -1;

        Print();
    }

    public void Print()
    {
        for (int count = 0; count < answerText.Count; count++)
        {
            answerText[count].text = "_";
            if (answer[count] >= 0)
            {
                answerText[count].text = answer[count].ToString();
            }
        }
        SfxManager.Instance.PlaySfx("Dial");
    }

    public void Cancel()
    {
        PuzzleManager.Instance.Fail();
    }
}
