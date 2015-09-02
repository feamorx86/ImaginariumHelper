using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NewGameScreen : MonoBehaviour {

    private Slider slider;
    private Text playersCounter;
    private List<GameObject> playersList;
    private int maxPlayers;
    private Button buttonPlay;
    private Button buttonCancel;

    public Game game;

    public void Awake()
    {
        slider = transform.FindChild("Slider").GetComponent<Slider>();
        playersCounter = transform.FindChild("PlyersCounter").GetComponent<Text>();
        GameObject playersPanel = transform.FindChild("Players").gameObject;
        playersList = new List<GameObject>(maxPlayers);
        maxPlayers = playersPanel.transform.childCount;
        for (int i = 0; i < maxPlayers; i++)
        {
            playersList.Add(playersPanel.transform.GetChild(i).gameObject);
        }

        slider.onValueChanged.AddListener(changePlayersCount);

        buttonPlay = transform.FindChild("ButtonStart").GetComponent<Button>();
        buttonPlay.onClick.AddListener(startGame);
        buttonCancel = transform.FindChild("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.AddListener(cancelGame);
	}

    private void startGame()
    {
        List<PlayerData> data = new List<PlayerData>();
        int idGenerator = 0;
        for (int i = 0; i < playersList.Count; i++)
        {
            if (playersList[i].activeSelf)
            {
                int id = idGenerator;
                idGenerator++;
                Color color = playersList[i].transform.FindChild("Image").GetComponent<Image>().color;
                string name = playersList[i].transform.FindChild("InputField").GetComponent<InputField>().text;

                PlayerData player = new PlayerData(id, name, color, 1);
                data.Add(player);
            }
        }

        game.StartGame(data);
        gameObject.SetActive(false);
        
    }

    private void cancelGame()
    {
        Application.Quit();
    }

    private void changePlayersCount(float float_to)
    {
        int to = (int)float_to;
        for (int i = 0; i < maxPlayers; i++)
        {
            if (i < to)
            {
                playersList[i].SetActive(true);
            }
            else
            {
                playersList[i].SetActive(false);
            }
        }
        playersCounter.text = to.ToString();
    }
}
