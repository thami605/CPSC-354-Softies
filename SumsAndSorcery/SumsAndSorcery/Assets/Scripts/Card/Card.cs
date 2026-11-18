/**********************************************************************
 * Author/Editor:  Tre Hamilton (CS)                                                                                                              
 * Creation Date:  November 17, 2026
 * Last Modified:  November 17, 2026
 * Project:        Sums & Sorcery
 * Filename:       CharacterBase.cs
 * Purpose:        
 * Modifications:  Listed at the bottom of the file. 
 **********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public List<CardType> cardType;
    public int cost;
    public int health;
    public int damageMin;
    public int damageMax;
    public Sprite cardSprite;
    public List<DamageType> damageType;

    public enum CardType
    {
        filler1,
        filler2,
        filler3,
        filler4
    }

    public enum DamageType
    {
        filler1,
        filler2,
        filler3
    }
}


/**********************************************************************/
/*
/* MODIFICATION HISTORY
/*
/**********************************************************************/
/* Author:     Tre Hamilton                                                   
/* Date:       November 17, 2026
/* Update:     Created class with basic structure from Class diagram
/* Time Spent: 10 minutes
/*
/**********************************************************************/