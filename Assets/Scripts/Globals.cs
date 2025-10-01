using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;



[RequireComponent(typeof(GridSystem))]
public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }

    public Material beatVisualizer;

    public GridSystem gridSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    void Start()
    {
        gridSystem = GetComponent<GridSystem>();
    }


    public bool inGame = true;
    public double timeSinceLastTick { get; private set; } = 0.0;
    public double tickProgression { get; private set; } = 0.0;
    public int currenTick { get; private set; } = 0;
    public int currentTick8 { get; private set; } = 0;
    public bool tickFrame { get; private set; } = false;
    public double bpmDuration = 60.0 / 90.0;
    public const double easingRatio = 0.000001;
    public double currentEasingRatio { get; private set; } = 0.0;

    void Update()
    {
        if (inGame)
        {
            timeSinceLastTick += Time.deltaTime;
            if (timeSinceLastTick >= bpmDuration)
            {
                timeSinceLastTick -= bpmDuration;
                tickFrame = true;
                currenTick++;
                BroadcastMessage("TickSystem", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("TickPlayer", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("TickMonster", SendMessageOptions.DontRequireReceiver);
                BroadcastMessage("TickPlayerLate", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                tickFrame = false;
            }
            currentEasingRatio = math.pow(easingRatio, Time.deltaTime);
            tickProgression = timeSinceLastTick / bpmDuration;

            int actualTick8 = (int)(tickProgression * 8);
            if (currentTick8 != actualTick8)
            {
                currentTick8 = actualTick8;
                BroadcastMessage("Tick8", SendMessageOptions.DontRequireReceiver);
            }
            beatVisualizer.SetFloat("_TickProgression", (float)tickProgression);
        }
    }
}
