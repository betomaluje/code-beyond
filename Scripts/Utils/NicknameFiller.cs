﻿using UnityEngine;
using UnityEngine.UI;

public class NicknameFiller : MonoBehaviour
{
    [SerializeField] private NicknameGenerator nicknameGenerator;
    [SerializeField] private InputField nicknameText;

    private void OnEnable()
    {
        GenerateRandomNickName();
    }

    public void GenerateRandomNickName()
    {
        string nickname = nicknameGenerator.Generate(Random.Range(2, 8));
        nicknameText.text = nickname;
    }

}
