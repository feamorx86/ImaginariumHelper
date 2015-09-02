using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private string playerName;
    private Color playerColor;

    private int id;
    private Image image;

    private int position;

    public int preTotal;
    public bool preGuessed;

    public void Awake()
    {
        image = transform.FindChild("Image").GetComponent<Image>();
    }

    public void setup(string name, Color color, int id, int position)
    {
        playerName = name;
        image.color = color;
        playerColor = color;
        this.id = id;
        this.position = position;
    }

    public string PlayerName { 
        get
        {
            return playerName;
        }
    }

    public Color PlayerColor
    {
        get
        {
            return playerColor;
        }
    }

    public int PlayerId
    {
        get 
        {
            return id;
        }
    }

    public int Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }
}
