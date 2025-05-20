using System.Collections;
using GameStates;
using Unity.Collections;
using UnityEngine;

public class GameManager : ManagerBase<GameManager>
{
    private IGameState _currentState;
    private int _currentLevelIndex;

    #if UNITY_EDITOR
        [Header("State Information")]
        [SerializeField, ReadOnly] private string currentState;
        [SerializeField, ReadOnly] private GameStateStatus stateStatus;
    #endif

    private void Start()
    {
        ChangeState(new MainMenuState());
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
        Instance.StartCoroutine(Instance.TransitionTo(nextState));
    }

    public static void SetLevelIndex(int index)
    {
        Instance._currentLevelIndex = index;
    }

    public static int GetLevelIndex()
    {
        return Instance._currentLevelIndex;
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