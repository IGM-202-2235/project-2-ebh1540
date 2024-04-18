using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : Singleton<AgentManager>
{

    [SerializeField]
    Agent agentPref;

    [SerializeField]
    int spawnCount = 100;

    public List<Obstacle> obstacles = new List<Obstacle>();

    public List<Agent> agents = new List<Agent>();

    Vector2 screenSize = Vector2.zero;

    public Vector2 ScreenSize { get { return screenSize; } }

    // (Optional) Prevent non-singleton constructor use.
    protected AgentManager() { }

    private void Start()
    {
        screenSize.y = Camera.main.orthographicSize;
        screenSize.x = screenSize.y * Camera.main.aspect;

        // Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            agents.Add(Instantiate(agentPref, PickRandomPoint(), Quaternion.identity));
        }
    }

    Vector2 PickRandomPoint()
    {
        Vector2 randPoint = Vector2.zero;

        randPoint.x = Random.Range(-ScreenSize.x, ScreenSize.x);
        randPoint.y = Random.Range(-ScreenSize.y, ScreenSize.y);

        return randPoint;
    }
}
