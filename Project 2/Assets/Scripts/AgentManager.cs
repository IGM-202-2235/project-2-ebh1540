using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : Singleton<AgentManager>
{

    [SerializeField]
    FoodMotivated foodMotivatedPref;

    [SerializeField]
    Obstacle obstaclePref;

    [SerializeField]
    Food foodPref;

    List<Agent> AgentPrefabs = new List<Agent>();

    [SerializeField]
    int minRocks, maxRocks;

    [SerializeField]
    int spawnCount = 100;

    public List<Obstacle> obstacles = new List<Obstacle>();

    public List<Agent> agents = new List<Agent>();

    public List<FoodMotivated> foodMotivateds = new List<FoodMotivated>();

    public List<Food> foodList = new List<Food>();

    Vector2 screenSize = Vector2.zero;

    public Vector2 ScreenSize { get { return screenSize; } }

    // (Optional) Prevent non-singleton constructor use.
    protected AgentManager() { }

    private void Start()
    {
        screenSize.y = Camera.main.orthographicSize;
        screenSize.x = screenSize.y * Camera.main.aspect;

        AgentPrefabs.Add(foodMotivatedPref);

        Spawn();
        SpawnRocks();
    }

    void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            if(Random.Range(0, AgentPrefabs.Count) == 0){
                FoodMotivated newFish = Instantiate(foodMotivatedPref, PickRandomPoint(), Quaternion.identity);
                agents.Add(newFish);
                foodMotivateds.Add(newFish);
            }
        }
    }

    void SpawnRocks(){
        int rocksToSpawn = Random.Range(minRocks, maxRocks);
        for (int i = 0; i < rocksToSpawn; i++){
            obstacles.Add(Instantiate(obstaclePref, PickRandomPoint(), Quaternion.identity));
        }
    }

    public void SpawnFood(Vector3 position){
        position.z = 0;
        foodList.Add(Instantiate(foodPref, position, Quaternion.identity));
    }

    public void eatFood(Food food, FoodMotivated eater){
        foodList.Remove(food);
        Destroy(food.gameObject);
        eater.resetFoodWeight();
    }

    Vector2 PickRandomPoint()
    {
        Vector2 randPoint = Vector2.zero;

        randPoint.x = Random.Range(-ScreenSize.x, ScreenSize.x);
        randPoint.y = Random.Range(-ScreenSize.y, ScreenSize.y);

        return randPoint;
    }
}
