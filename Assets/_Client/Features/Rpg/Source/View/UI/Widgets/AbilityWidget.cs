﻿using UnityEngine;
using UnityEngine.UI;

namespace Client.Rpg
{
    public class AbilityWidget : MonoBehaviour
    {
        [SerializeField] private Image _artImage;

        public void SetArt(Sprite art)
        {
            _artImage.sprite = art;
        }
    }
}
