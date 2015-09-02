using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

    private List<GameCloud> clouds;
    private int activePlayerIndex;
    private List<Player> players;
    public GameScreen screen;
    private int endGamePosition;
    private int startGamePosition;

    private int calculationPlayerIndex;

    private GameCloud addRandomCloud(Transform cloudPoint, int number, CloudTypes type)
    {
        string resource = "prefabs/Cloud_" + Random.Range(1, 4).ToString();
        GameObject instance= Instantiate(Resources.Load(resource, typeof(GameObject))) as GameObject;
        GameCloud cloud = instance.GetComponent<GameCloud>();
        cloud.setup(cloudPoint.transform, type, number);
        return cloud;
    }

    public void createClouds()
    {
        clouds = new List<GameCloud>();
        Transform cloudsRoot = transform.FindChild("Clouds");
        Transform points = cloudsRoot.FindChild("points");
        int cloudNumber = 1;
        for (int i = 0; i < points.childCount; i++ )
        {
            Transform cloudPoint = points.GetChild(i);
            GameCloud cloud = addRandomCloud(cloudPoint, cloudNumber, CloudTypes.Default);
            clouds.Add(cloud);
            cloudNumber++;
        }
        endGamePosition = cloudNumber - 1;
        startGamePosition = 1;
    }

    public GameCloud getStart()
    {
        return clouds[0];
    }

    public GameCloud getFinish()
    {
        return clouds[clouds.Count - 1];
    }

    public Player selectFirstPlayer()
    {
        int index = Random.Range(0, players.Count-1);
        activePlayerIndex = index;
        return players[index];
    }

    public GameCloud getCloud(int position)
    {
        GameCloud cloud = null;
        if (position > 0 && position <= clouds.Count)
        {
            cloud = clouds[position - 1];
        }        
        return cloud;
    }

    private void createPlayer(PlayerData data)
    {
        GameObject instance = Instantiate(Resources.Load("prefabs/player", typeof(GameObject))) as GameObject;
        Player player = instance.GetComponent<Player>();
        GameCloud playerCloud = getCloud(data.position);
        if (playerCloud == null)
        {
            Debug.LogError("Fail to get cloud for :" + data.ToString());
            instance.SetActive(false);
            GameObject.DestroyObject(instance);
        }
        else
        {
            player.setup(data.name, data.color, data.id, data.position);
            playerCloud.addPlayer(player);
        }
        players.Add(player);
    }

    public void StartGame(List<PlayerData> data)
    {
        gameObject.SetActive(true);
        screen.gameObject.SetActive(true);
        createClouds();
        players = new List<Player>();
        foreach (PlayerData player in data)
        {
            createPlayer(player);
        }
        Player firstPlayer = selectFirstPlayer();
        screen.setMaxCounter(players.Count-2);
        screen.ShowCurrentPlayer(firstPlayer);
    }

    

    public void moveComplete()
    {
        calculationPlayerIndex = 0;
        if (calculationPlayerIndex == activePlayerIndex)
        {
            calculationPlayerIndex++;
            if (calculationPlayerIndex > players.Count)
            {
                calculationPlayerIndex = 0;
            }
        }

        screen.ShowEndMoveForPlayer(players[calculationPlayerIndex]);
    }

    public void playerEndMove(bool isGuessed, int counter)
    {
        Player player = players[calculationPlayerIndex];
        player.preTotal = counter;
        player.preGuessed = isGuessed;
        calculationPlayerIndex++;

        if (calculationPlayerIndex == activePlayerIndex)
        {
            calculationPlayerIndex++;
        }

        if (calculationPlayerIndex >= players.Count)
        {
            calcEndMoveResults();
        }
        else
        {
            screen.ShowEndMoveForPlayer(players[calculationPlayerIndex]);
        }
    }

    private void calcEndMoveResults()
    {
        screen.showResultsScreen();
        int totalActiveGuessed = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (i != activePlayerIndex)
            {
                if (players[i].preGuessed)
                {
                    totalActiveGuessed++;
                }
            }
        }

        if (totalActiveGuessed > 0)
        {
            if (totalActiveGuessed == players.Count - 1)
            {
                allGuessed();
            }
            else
            {
                guessedNotAll(totalActiveGuessed);
            }
        }
        else
        {
            noOneNotGuessed();
        }
    }

    private void noOneNotGuessed()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (i == activePlayerIndex)
            {
                screen.addResultsItem(player, true, "Ни кто не угадал", "", "-2");
                player.preTotal = -2;
            }
            else
            {
                if (player.preTotal > 0)
                {
                    screen.addResultsItem(player, false, "Не угадал.\nВыбрали: " + player.preTotal, "0 + " + player.preTotal + " =", "+" + player.preTotal);
                }
                else
                {
                    if (player.preTotal < 0)
                    {
                        //log error
                    }
                    else
                    {
                        screen.addResultsItem(player, false, "Не угадал.\nВыбрали: " + player.preTotal, "", "0");
                    }
                }
            }
        }
    }

    private void allGuessed()
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (i == activePlayerIndex)
            {
                screen.addResultsItem(player, true, "Все угадали", "", "-3");
                player.preTotal = -3;
            }
            else
            {
                screen.addResultsItem(player, false, "Все угадали", "", "0");
                player.preTotal = 0;
            }
        }
    }

    public void guessedNotAll(int guessed)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            if (i == activePlayerIndex)
            {
                player.preTotal = 3 + guessed;
                screen.addResultsItem(player, true, "Угадали : " + guessed, "3 + " + guessed + " =", "+" + player.preTotal);                
            }
            else
            {
                if (player.preGuessed)
                {
                    if (player.preTotal > 0)
                    {
                        int result = 3 + player.preTotal;
                        screen.addResultsItem(player, false, "Угадал.\nВыбрали: " + player.preTotal, "3 + " + player.preTotal + " =", "+" + result);
                        player.preTotal = result;
                    }
                    else
                    {
                        screen.addResultsItem(player, false, "Угадал.\nНе выбрали", "", "+3");
                        player.preTotal = 3;
                    }
                }
                else
                {
                    if (player.preTotal > 0)
                    {
                        screen.addResultsItem(player, false, "Не угадал.\nВыбрали: " + player.preTotal, "", "+" + player.preTotal);
                    }
                    else
                    {
                        screen.addResultsItem(player, false, "Не угадал.\nНе выбрали", "", "0");
                    }
                }
    
            }
        }
    }

    public void finishMove()
    {
        List<Player> winners = null;
        bool endGame = false;
        foreach (Player player in players)
        {
            int shift = player.preTotal;
            int nextPosition = player.Position + shift;
            if (nextPosition >= endGamePosition  ) {
                endGame = true;
                if (winners == null)
                    winners = new List<Player>();
                winners.Add(player);
                nextPosition = endGamePosition;
            } else if (nextPosition < startGamePosition)
            {
                nextPosition = startGamePosition;
            }
            if (player.Position != nextPosition)
            {
                getCloud(player.Position).removePlayer(player);
                getCloud(nextPosition).addPlayer(player);
                player.Position = nextPosition;
            }
            player.preGuessed = false;
            player.preTotal = 0;
        }

        if (endGame)
        {
            screen.showWinnersScreen();
            foreach (Player player in winners)
            {
                screen.addWinner(player);
            }
            
        }
        else
        {
            activePlayerIndex++;
            if (activePlayerIndex >= players.Count)
                activePlayerIndex = 0;

            screen.ShowCurrentPlayer(players[activePlayerIndex]);
        }
    }

    public void endGame()
    {
        Application.Quit();
    }
}
