using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {
    public static LevelManager instance = null;

    #region Private Variables

    [SerializeField]
    private GameObject m_pauseButton;
    [SerializeField]
    private GameObject m_startButton;
    [SerializeField]
    private Text m_currentScore;
    [SerializeField]
    private Text m_highestScoreTxt;
    [SerializeField]
    private GameObject m_gameOverTxt;

    // Reference to the state machine
    private StateManager m_stateMachine = null;

    [SerializeField]
    private float m_timeInterval = 1f;

    // Current score
    private int m_score = 0;   
    private int m_highestScore = 0;

    #endregion

    #region Unity APIs

    void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        } 
        else if (instance != this) 
        {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (this);

        // Make sure all UI Elements are assinged properly
        Debug.Assert(m_pauseButton!= null, "LevelManager : Please drag pause button to m_pauseButton!");
        Debug.Assert(m_startButton!= null, "LevelManager : Please drag start button to m_StartButton!");
        Debug.Assert(m_currentScore!= null, "LevelManager : Please drag score text to m_currentScore!");
        Debug.Assert(m_highestScoreTxt!= null, "LevelManager : Please drag highest score text to m_highestScoreTxt!");
        Debug.Assert(m_gameOverTxt != null, "LevelManager : Please drag game over text to m_GameOverTxt!");

        // Load High Score from playerpref
        m_highestScore = PlayerPrefs.GetInt("ScoreDatabase");
        m_highestScoreTxt.text = "Highest Score : " + m_highestScore.ToString();
    }

    void Start ()
    {
        // Create and register states and transitions
        m_stateMachine = new StateManager();
        StateGeneric IntroState = new IntroState();
        StateGeneric InGameState = new InGameState();
        StateGeneric GameOverState = new GameOverState();

        m_stateMachine.RegisterState(IntroState.ToString(), IntroState);
        m_stateMachine.RegisterState(InGameState.ToString(), InGameState);
        m_stateMachine.RegisterState(GameOverState.ToString(), GameOverState);

        m_stateMachine.AddTransition(IntroState, InGameState);
        m_stateMachine.AddTransition(InGameState, GameOverState);
        m_stateMachine.SetDefaultState(IntroState.ToString());
    }        
	
	void Update ()
    {      
        // Level Manager is driving the state machine 
        if (m_stateMachine != null)
        {
            m_stateMachine.Execute();
        }
    }

    #endregion

    #region Public Interface

    /// <summary>
    /// Transition to Game Over State, called when the game reaches game over condition
    /// </summary>
    public void GameOver()
    {
        m_stateMachine.SwitchState("GameOverState");
    }

    /// <summary>
    /// Transition to in game state, called when player starts the game
    /// </summary>
    public void StartTheGame()
    {
        m_stateMachine.SwitchState("InGameState");
    }

    /// <summary>
    /// Called when player hits pause button
    /// </summary>
    public void Pause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    /// <summary>
    /// Return the instance of pause button
    /// </summary>
    /// <returns></returns>
    public GameObject GetPauseButton()
    {
        return m_pauseButton;
    }

    /// <summary>
    /// Get the start button
    /// </summary>
    /// <returns></returns>
    public GameObject GetStartPauseButton()
    {
        return m_startButton;
    }

    /// <summary>
    /// Get the game over txt instance
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameOverTxtObject()
    {
        return m_gameOverTxt;
    }

    /// <summary>
    /// Add points to current score
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        m_score += score;
        m_currentScore.text = "Score : " + m_score.ToString();

        if (m_score > m_highestScore)
        {
            PlayerPrefs.SetInt("ScoreDatabase", m_score);
            m_highestScoreTxt.text = "Highest Score : " + m_highestScore.ToString();
        }
    }

    /// <summary>
    /// The time it takes before the game moves the piece downward for you.
    /// </summary>
    /// <returns></returns>
    public float GetTimeInterval()
    {
        return m_timeInterval;
    }

    #endregion
}
