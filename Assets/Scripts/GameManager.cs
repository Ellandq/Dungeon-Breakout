using System.Collections;
using GameStates;
using Unity.Collections;
using UnityEngine;

public class GameManager : ManagerBase<GameManager>
{
    private IGameState _currentState;

    #if UNITY_EDITOR
        [Header("State Information")]
        [SerializeField, ReadOnly] private string currentState;
        [SerializeField, ReadOnly] private GameStateStatus stateStatus;
    #endif

    private void Start()
    {
        // TODO Rework this to use main menu as the default or work based on the given scene
        // ChangeState(new PlayingState());
    }

    private void Update()
    {
    #if UNITY_EDITOR
            currentState = _currentState.Name;
            stateStatus = _currentState.Status;
    #endif
        _currentState?.Update();
    }

    public void ChangeState(IGameState nextState)
    {
        StartCoroutine(TransitionTo(nextState));
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