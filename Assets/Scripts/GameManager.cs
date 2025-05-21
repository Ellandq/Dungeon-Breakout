using System.Collections;
using GameStates;
using Input;
using Unity.Collections;
using UnityEngine;

public class GameManager : ManagerBase<GameManager>
{
    private IGameState _currentState;
    private int _currentLevelIndex;

    [Header("Save Information")]
    [SerializeField] private int maxLevelReached;

    #if UNITY_EDITOR
        [Header("State Information")]
        [SerializeField, ReadOnly] private string currentState;
        [SerializeField, ReadOnly] private GameStateStatus stateStatus;
    #endif

    private void Start()
    {
        ChangeState(new MainMenuState());
        
        InputManager.GetKeyboardInputHandle().AddListenerOnInputAction((state) =>
        {
            if (_currentState.Name != "PlayState" 
                || _currentState.Name == "MainMenuState" 
                || _currentState.Name == "LoadToMenuState") return;
            ChangeState(new LoadToMenuState());
        }, "Escape");
    }

    private void Update()
    {
        #if UNITY_EDITOR
                currentState = _currentState.Name;
                stateStatus = _currentState.Status;
        #endif
        _currentState?.Update();
    }

    public static void ChangeState(IGameState nextState)
    {
        if (Instance._currentState != null && nextState.Name == Instance._currentState.Name) return;
        Instance.StartCoroutine(Instance.TransitionTo(nextState));
    }

    public static void SetLevelIndex(int index)
    {
        Instance.maxLevelReached = index;
        Instance._currentLevelIndex = index;
    }

    public static int GetLevelIndex()
    {
        return Instance._currentLevelIndex;
    }

    public static int GetCurrentMaxLevel()
    {
        return Instance.maxLevelReached;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator TransitionTo(IGameState nextState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
            while (!_currentState.CanExit())
            {
                yield return null;
            }
        }

        _currentState = nextState;
        _currentState.Enter();
    }
}