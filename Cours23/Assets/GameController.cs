using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameController : MonoBehaviour {
    public static GameController control;

    public int attack;
    public int defense;
    public int health;
    public List<Weapon> weapons;
    public int CurrentWeaponIndex;
	// Use this for initialization
	void Start () {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
            SetDefaultValue();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDefaultValue()
    {
        attack = 1;
        defense = 1;
        health = 1;
        weapons = new List<Weapon>();
        CurrentWeaponIndex = 0;
    }

    public void AddAttackToCurrentWeapon()
    {
        Weapon currentWeapon = weapons[CurrentWeaponIndex];
        currentWeapon.WeaponAttack++;
    }

    public void NextWeapon()
    {
        if(weapons.Count > CurrentWeaponIndex + 1)
        {
            CurrentWeaponIndex++;
        }
        else
        {
            CurrentWeaponIndex = 0;
        }
    }

    public void PreviousWeapon()
    {
        if (CurrentWeaponIndex > 0)
        {
            CurrentWeaponIndex--;
        }
        else
        {
            CurrentWeaponIndex = weapons.Count - 1;
        }
    }

    public void AddWeapon()
    {
        Weapon newWeapon = new Weapon();
        newWeapon.WeaponAttack = 1;
        weapons.Add(newWeapon);
    }

    public void AddAttack()
    {
        attack++;
    }

    public void AddDefense()
    {
        defense++;
    }

    public void AddHealth()
    {
        health++;
    }
	
	public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        if(!File.Exists(Application.persistentDataPath + "gameInfo.dat"))
        {
            throw new Exception("Game file doesn't exist");
        }
        FileStream file = File.Open(Application.persistentDataPath + "gameInfo.dat", FileMode.Open);
        PlayerData playerDataToLoad = (PlayerData)bf.Deserialize(file);
        file.Close();
        attack = playerDataToLoad.attack;
        defense = playerDataToLoad.defense;
        health = playerDataToLoad.health;
        weapons = playerDataToLoad.weapons;
        CurrentWeaponIndex = playerDataToLoad.gunIndexInFile;
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "gameInfo.dat", FileMode.Create);
        PlayerData playerDataToSave = new PlayerData();
        playerDataToSave.attack = attack;
        playerDataToSave.defense = defense;
        playerDataToSave.health = health;
        playerDataToSave.weapons = weapons;
        playerDataToSave.gunIndexInFile = CurrentWeaponIndex;
        bf.Serialize(file, playerDataToSave);
        file.Close();
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 32;
        GUI.Label(new Rect(10, 60, 180, 80), "Attack : " + attack, style);
        GUI.Label(new Rect(10, 110, 180, 80), "Defense : " + defense, style);
        GUI.Label(new Rect(10, 160, 180, 80), "Health : " + health, style);
        if (weapons.Count > 0)
        {
            GUI.Label(new Rect(10, 210, 180, 80), "Current Weapon index : " + CurrentWeaponIndex, style);
        }
        if(weapons.Count > 0)
        {
            GUI.Label(new Rect(10, 260, 180, 80), "Current Weapon attack : " + weapons[CurrentWeaponIndex].WeaponAttack, style);
        }
    }
}
[Serializable]
class PlayerData
{
    public int attack;
    public int defense;
    public int health;
    public int gunIndexInFile;
    public List<Weapon> weapons;
}
[Serializable]
public class Weapon
{
    public int WeaponAttack;
}
