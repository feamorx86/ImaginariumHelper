using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;

public class BaseDialog
{
    private GameObject root;
    public BaseDialog(GameObject gameObject)
    {
        this.root = gameObject;
    }

    public GameObject Root
    {
        get
        {
            return root;
        }
    }

    public void show()
    {
        root.SetActive(true);
    }

    public void hide()
    {
        root.SetActive(false);
    }
}

public class CurrenPlayerScreen : BaseDialog
{
    public Image playerImage;
    public Text playerLabel;
    public Button buttonNext;

    public CurrenPlayerScreen(GameObject gameObject, UnityAction onNextClick) : base(gameObject)
    {
        Transform player = Root.transform.FindChild("player");
        playerImage = player.FindChild("Image").GetComponent<Image>();
        playerLabel = player.FindChild("Text").GetComponent<Text>();
        buttonNext = Root.transform.FindChild("buttonEnd").GetComponent<Button>();
        buttonNext.onClick.AddListener(onNextClick);
    }

    public void showPlayer(Player player)
    {
        playerImage.color = player.PlayerColor;
        playerLabel.text = player.PlayerName;
    }
}

public class WinnersItem : BaseDialog
{
    public Image playerImage;
    public Text playerLabel;

    public WinnersItem(GameObject gameObject)
        : base(gameObject)
    {
        playerImage = Root.transform.FindChild("Icon").GetComponent<Image>();
        playerLabel = Root.transform.FindChild("Name").GetComponent<Text>();
    }

    public void setPlayer(Player player)
    {
        playerImage.color = player.PlayerColor;
        playerLabel.text = player.PlayerName;
    }
}

public class WinnersScreen : BaseDialog
{
    public List<WinnersItem> players;
    public Button endButton;
    private int lastAddedPlayerIndex;

    public WinnersScreen(GameObject gameObject, UnityAction onEndClick)
        : base(gameObject)
    {
        Transform playersRoot = Root.transform.FindChild("players");
        players = new List<WinnersItem>();
        for (int i = 0; i < playersRoot.childCount; i++)
        {
            WinnersItem item = new WinnersItem(playersRoot.GetChild(i).gameObject);
            players.Add(item);
            item.hide();
        }

        endButton = Root.transform.FindChild("buttonEnd").GetComponent<Button>();
        endButton.onClick.AddListener(onEndClick);
    }

    public void clear()
    {
        lastAddedPlayerIndex = 0;
        foreach (WinnersItem item in players)
        {
            item.hide();
        }
    }

    public void addPlayer(Player player)
    {
        if (lastAddedPlayerIndex < players.Count)
        {
            players[lastAddedPlayerIndex].setPlayer(player);
            players[lastAddedPlayerIndex].show();
            lastAddedPlayerIndex++;
        }
        else
        {
            //TODO: log error
        }
    }
}

public class ResultScreenItem : BaseDialog
{
    public Image playerImage;
    public Text playerLabel;
    public Text resultContext;
    public Text resultCalculation;
    public Text resultlabel;

    public ResultScreenItem(GameObject gameObject)
        : base(gameObject)
    {
        playerImage = Root.transform.FindChild("Icon").GetComponent<Image>();
        playerLabel = Root.transform.FindChild("Name").GetComponent<Text>();
        resultContext = Root.transform.FindChild("Context").GetComponent<Text>();
        resultCalculation = Root.transform.FindChild("Calc").GetComponent<Text>();
        resultlabel = Root.transform.FindChild("Result").GetComponent<Text>();
    }

    public void setPlayer(Player player, string context, string calculation, string result)
    {
        playerImage.color = player.PlayerColor;
        playerLabel.text = player.PlayerName;
        resultContext.text = context;
        resultCalculation.text = calculation;
        resultlabel.text = result;
    }

    public void clear()
    {
        playerImage.color = Color.white;
        playerLabel.text = "";
        resultContext.text = "";
        resultCalculation.text = "";
        resultlabel.text = "";
    }
}

public class ResultsScreen : BaseDialog
{
    public ResultScreenItem activePlayer;
    public List<ResultScreenItem> players;
    public Button nextButton;
    private int lastAddedPlayerIndex;

    public ResultsScreen(GameObject gameObject, UnityAction onNextClick)
        : base(gameObject)
    {
        activePlayer = new ResultScreenItem(Root.transform.FindChild("activePlayer").gameObject);

        Transform playersRoot = Root.transform.FindChild("players");
        players = new List<ResultScreenItem>();
        for (int i = 0; i < playersRoot.childCount; i++)
        {
            ResultScreenItem item = new ResultScreenItem(playersRoot.GetChild(i).gameObject);
            players.Add(item);
            item.hide();
        }

        nextButton = Root.transform.FindChild("buttonNext").GetComponent<Button>();
        nextButton.onClick.AddListener(onNextClick);
    }

    public void clear()
    {
        lastAddedPlayerIndex = 0;
        activePlayer.clear();
        foreach (ResultScreenItem item in players)
        {
            item.clear();
            item.hide();
        }
    }

    public void forActivePlayer(Player player, string context, string calculation, string result)
    {
        activePlayer.setPlayer(player, context, calculation, result);
        activePlayer.show();
    }

    public void addPlayer(Player player, string context, string calculation, string result)
    {
        if (lastAddedPlayerIndex < players.Count)
        {
            players[lastAddedPlayerIndex].setPlayer(player, context, calculation, result);
            players[lastAddedPlayerIndex].show();
            lastAddedPlayerIndex++;
        }
        else
        {
            //TODO: log error
        }
    }
}

public class EndMoveScreen : BaseDialog
{
    public Image playerImage;
    public Text playerLabel;
    public Toggle toggleGuessed;
    private Button buttonInc;
    private Button buttonDec;
    public Button buttonNext;
    public Text playerCounter;
    public int counter;
    private int maxCounter;

    public EndMoveScreen(GameObject gameObject, UnityAction onNextClick)
        : base(gameObject)
    {
        Transform player = Root.transform.FindChild("player");
        playerImage = player.FindChild("Image").GetComponent<Image>();
        playerLabel = player.FindChild("Text").GetComponent<Text>();
        buttonNext = Root.transform.FindChild("ButtonNext").GetComponent<Button>();
        buttonNext.onClick.AddListener(onNextClick);
        toggleGuessed = Root.transform.FindChild("ToggleGuessed").GetComponent<Toggle>();
        playerCounter = Root.transform.FindChild("Counter").GetComponent<Text>();
        buttonInc = Root.transform.FindChild("ButtonInc").GetComponent<Button>();
        buttonInc.onClick.AddListener(increment);
        buttonDec = Root.transform.FindChild("ButtonDec").GetComponent<Button>();
        buttonDec.onClick.AddListener(decrement);
    }

    public void setMaxCounter(int value)
    {
        maxCounter = value;
    }

    private void increment()
    {
        counter++;
        if (counter > maxCounter)
        {
            counter = maxCounter;
        }
        playerCounter.text = counter.ToString();
    }

    private void decrement()
    {
        counter--;
        if (counter < 0)
        {
            counter = 0;
        }
        playerCounter.text = counter.ToString();
    }

    public void forPlayer(Player player)
    {
        toggleGuessed.isOn = false;
        counter = 0;
        playerCounter.text = "0";
        playerImage.color = player.PlayerColor;
        playerLabel.text = player.PlayerName;
    }
}


public class GameScreen : MonoBehaviour {

    private CurrenPlayerScreen currentPlayer;
    private EndMoveScreen endMove;
    private ResultsScreen resultsScreen;
    private WinnersScreen winnersScreen;

    public Game game;

    public void Awake()
    {
        currentPlayer = new CurrenPlayerScreen(transform.FindChild("CurrentPlayer").gameObject, currentPlayerNext);
        endMove = new EndMoveScreen(transform.FindChild("EndMove").gameObject, endMoveNext);
        resultsScreen = new ResultsScreen(transform.FindChild("MoveResults").gameObject, сloseMoveResults);
        winnersScreen = new WinnersScreen(transform.FindChild("Winners").gameObject, finishGame);

        endMove.hide();
        currentPlayer.hide();
        resultsScreen.hide();
        winnersScreen.hide();
    }

    public void setMaxCounter(int maxCounter)
    {
        endMove.setMaxCounter(maxCounter);
    }

    public void ShowCurrentPlayer(Player player)
    {
        currentPlayer.show();
        endMove.hide();
        winnersScreen.hide();
        resultsScreen.hide();
        currentPlayer.showPlayer(player);
    }   
    
    public void ShowEndMoveForPlayer(Player player)
    {
        currentPlayer.hide();
        resultsScreen.hide();
        winnersScreen.hide();
        endMove.show();
        endMove.forPlayer(player);
    }

    public void showResultsScreen()
    {
        currentPlayer.hide();
        endMove.hide();
        winnersScreen.hide();
        resultsScreen.clear();
        resultsScreen.show();
    }

    public void showWinnersScreen()
    {
        currentPlayer.hide();
        endMove.hide();
        resultsScreen.hide();
        winnersScreen.clear();
        winnersScreen.show();
    }

    public void addResultsItem(Player player, bool isActivePlayer, string context, string calculation, string result)
    {
        if (isActivePlayer)
        {
            resultsScreen.forActivePlayer(player, context, calculation, result);
        }
        else
        {
            resultsScreen.addPlayer(player, context, calculation, result);
        }
    }

    public void addWinner(Player player)
    {
        winnersScreen.addPlayer(player);
    }

    private void currentPlayerNext()
    {
        game.moveComplete();
    }

    private void endMoveNext()
    {
        game.playerEndMove(endMove.toggleGuessed.isOn, endMove.counter);
    }
    
    private void сloseMoveResults()
    {
        game.finishMove();
    }

    private void finishGame()
    {
        game.endGame();
    }
}
