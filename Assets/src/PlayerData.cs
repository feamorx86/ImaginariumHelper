using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerData
{
    public int id;
    public string name;
    public Color color;
    public int position;

    public PlayerData(int id, string name, Color color, int position)
    {
        this.id = id;
        this.name = name;
        this.color = color;
        this.position = position;
    }
}