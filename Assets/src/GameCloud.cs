using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum CloudTypes
{
    Default = 1
}

public class GameCloud : MonoBehaviour {

    private Image image;
    //private Image typeImage;
    private Text numberLabel;
    private List<Transform> playerPoints;
    private List<Player> players;
    
    private int number;
    private CloudTypes type;

	void Awake () {
        image = transform.FindChild("Image").GetComponent<Image>();
        //typeImage = transform.FindChild("Type").GetComponent<Image>();
        numberLabel = transform.FindChild("Number").GetComponent<Text>();

        Transform playersRoot = transform.FindChild("Players");
        int playersCount = playersRoot.childCount;
        playerPoints = new List<Transform>(playersCount);
        players = new List<Player>();
        for (int i = 0; i < playersCount; i++)
        {
            playerPoints.Add(playersRoot.GetChild(i));
        }
	}

    public void setup(Transform cloudPoint, CloudTypes type, int number)
    {
        this.type = type;
        this.number = number;
        transform.SetParent(cloudPoint, false);

        numberLabel.text = number.ToString();
    }

    public void addPlayer(Player player)
    {
        players.Add(player);
        bool userAdded = false;
        for (int i = 0; i < playerPoints.Count; i++)
        {
            bool isEmpty = playerPoints[i].childCount == 0;
            if (isEmpty)
            {
                player.transform.SetParent(playerPoints[i], false);
                userAdded = true;
                break;
            }
        }
        if (!userAdded)
        {
            Debug.LogError("Can`t add player :" + player + " to cloud : " + this);
        }
    }

    public void removePlayer(Player player)
    {
        bool userRemoved = players.Remove(player);
        //if (players.Remove(player))
        //{
        //    for (int i = 0; i < playerPoints.Count; i++)
        //    {
        //        bool isFound = playerPoints[i].gameObject == player.gameObject;
        //        if (isFound)
        //        {
        //            player.transform.SetParent(null, false);
        //            userRemoved = true;
        //            break;
        //        }
        //    }
        //}
        if (!userRemoved)
        {
            Debug.LogError("Can`t found or remove player :" + player + " from cloud : " + this);
        }
    }
}
