using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionTree : ITreeNode
{
    Func<bool> _question;
    ITreeNode _trueNode;
    ITreeNode _falseNode;

    public QuestionTree(Func<bool> question, ITreeNode trueNode, ITreeNode falseNode)
    {
        _question = question;
        _trueNode = trueNode;
        _falseNode = falseNode;
    }

    public void Execute()
    {
        if (_question())
        {
            _trueNode.Execute();
        }

        else
        {
            _falseNode.Execute();
        }
    }
}