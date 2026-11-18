/**********************************************************************
 * Author/Editor:  Tre Hamilton (CS)                                                                                                              
 * Creation Date:  November 17, 2026
 * Last Modified:  November 17, 2026
 * Project:        Sums & Sorcery
 * Filename:       CharacterBase.cs
 * Purpose:        
 * Modifications:  Listed at the bottom of the file. 
 **********************************************************************/
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    protected int maxHealth;
    protected int currentHealth;
    protected int block;


    public void TakeDamage(int amount)
    {
        
    }

    public void Heal(int amount)
    {
        
    }

    public void Block(int amount) 
    {
        
    }
    
    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}

