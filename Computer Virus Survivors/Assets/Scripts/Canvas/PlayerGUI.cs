using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour, IPlayerStatObserver
{

    [Serializable]
    public class HPGroup
    {
        public Slider hpSlider;
        public TextMeshProUGUI hpText;

        private int maxHP;
        private int currentHP;

        public void SetMaxHP(int maxHP)
        {
            this.maxHP = maxHP;
            UpdateHPbar();
        }

        public void SetCurrentHP(int currentHP)
        {
            this.currentHP = currentHP;
            UpdateHPbar();
        }

        private void UpdateHPbar()
        {
            hpText.text = $"{currentHP} / {maxHP}";
            hpSlider.value = currentHP / (float) maxHP;
        }
    }

    [Serializable]
    public class EXPGroup
    {
        public Slider expCurrent;

        private int maxEXP;
        private int currentEXP;

        public void SetMaxEXP(int maxEXP)
        {
            this.maxEXP = maxEXP;
            UpdateEXPbar();
        }

        public void SetCurrentEXP(int currentEXP)
        {
            this.currentEXP = currentEXP;
            UpdateEXPbar();
        }

        private void UpdateEXPbar()
        {
            expCurrent.value = currentEXP / (float) maxEXP;
        }
    }

    [SerializeField] private PlayerStatEventCaller playerStatEventCaller;
    [SerializeField] private HPGroup hpGroup;
    [SerializeField] private EXPGroup expGroup;

    public void Initialize()
    {
        playerStatEventCaller.StatChangedHandler += OnStatChanged;
    }


    public void OnStatChanged(object sender, StatChangedEventArgs e)
    {
        if (e.StatName == nameof(PlayerStat.CurrentHP))
        {
            hpGroup.SetCurrentHP((int) e.NewValue);
        }
        else if (e.StatName == nameof(PlayerStat.MaxHP))
        {
            hpGroup.SetMaxHP((int) e.NewValue);
        }
        else if (e.StatName == nameof(PlayerStat.CurrentExp))
        {
            expGroup.SetCurrentEXP((int) e.NewValue);
        }
        else if (e.StatName == nameof(PlayerStat.MaxExp))
        {
            expGroup.SetMaxEXP((int) e.NewValue);
        }
    }

}
