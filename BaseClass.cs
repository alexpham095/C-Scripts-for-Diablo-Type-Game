using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseClass
{
    public enum genders
    {
        MALE,
        FEMALE
    }

    [Header("Infos")]
    public string myName;
    public int currentLevel;
    public int currentXP;

    [Header("Health & Mana")]
    public int cur_health;
    public int max_health;
    public int cur_mana;
    public int max_mana;

    [Header("Gender")]
    public genders gender;

    [Header("Stats")]
    public int strength;
    public int endurance;
    public int agility;
    public int wisdom;
    public int intelligence;

    [Header("Skills")]
    public int statPoints;
    public int skillPoints;
    //CURRENT SKILLS

}
