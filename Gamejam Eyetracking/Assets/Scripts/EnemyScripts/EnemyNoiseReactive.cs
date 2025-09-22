using UnityEngine;

public class EnemyNoiseReactive : MonoBehaviour
{
    [SerializeField] private Transform player;
    private EnemyPathfinding pathfinding;


    [Header("Enemy Settings")]
    public EnemyState currentState;


    [Header("Mic Settings")]
    public MicInputLevel micInputLevel;
    public float detectionThreshold = 5.0f;
    void Start()
    {
        currentState = EnemyState.Idle;
        pathfinding = GetComponent<EnemyPathfinding>();
    }
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                EnemyIdle();
                break;
            case EnemyState.Attack:

                break;
            case EnemyState.Run:

                break;
        }
    }

    void EnemyIdle()
        {
            
        }
}
