using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentManager : Singleton<AgentManager>
{

    [SerializeField]
    Hungry hungryPref;

    [SerializeField]
    Schooling schoolingPref;

    [SerializeField]
    Avoidant avoidantPref;

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

    public List<Agent> foodMotivateds = new List<Agent>();

    public List<Hungry> hungries = new List<Hungry>();

    public List<Schooling> schoolings = new List<Schooling>();

    public List<Avoidant> avoidants = new List<Avoidant>();

    public List<Food> foodList = new List<Food>();

    Vector2 screenSize = Vector2.zero;

    Vector3 schoolingPoint = Vector3.zero;

    public Vector2 ScreenSize { get { return screenSize; } }

    public Vector3 SchoolingPoint {get { return schoolingPoint; } }

    // (Optional) Prevent non-singleton constructor use.
    protected AgentManager() { }

    private void Start()
    {
        screenSize.y = Camera.main.orthographicSize;
        screenSize.x = screenSize.y * Camera.main.aspect;

        AgentPrefabs.Add(hungryPref);
        AgentPrefabs.Add(schoolingPref);
        AgentPrefabs.Add(avoidantPref);

        Spawn();
        SpawnRocks();
        schoolingPoint = calcSchoolingPoint();
    }

    private void Update(){
        if(hungries.Count == 0){
            SpawnHungry(); // guarantee that there's at least one hungry fish at all times, more can spawn by eating but they die by starving
        }
        schoolingPoint = calcSchoolingPoint();
    }

    void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int toSpawn = Random.Range(0, AgentPrefabs.Count);
            if(toSpawn == 0){
                SpawnHungry();
            }
            else if(toSpawn == 1){
                SpawnSchooling();
            }
            else{
                SpawnAvoidant();
            }
        }
    }

    void SpawnHungry(){
        Hungry newFish = Instantiate(hungryPref, PickRandomPoint(), Quaternion.identity);
        agents.Add(newFish);
        foodMotivateds.Add(newFish);
        hungries.Add(newFish);
    }

    void SpawnHungry(Vector3 position){
        Hungry newFish = Instantiate(hungryPref, position, Quaternion.identity);
        agents.Add(newFish);
        foodMotivateds.Add(newFish);
        hungries.Add(newFish);
    }

    void SpawnSchooling(){
        Schooling newFish = Instantiate(schoolingPref, PickRandomPoint(), Quaternion.identity);
        agents.Add(newFish);
        foodMotivateds.Add(newFish);
        schoolings.Add(newFish);
    }

    void SpawnAvoidant(){
        Avoidant newFish = Instantiate(avoidantPref, PickRandomPoint(), Quaternion.identity);
        agents.Add(newFish);
        avoidants.Add(newFish);
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

    public void EatFood(Food food, Hungry eater){
        foodList.Remove(food);
        Vector3 position = food.transform.position;
        Destroy(food.gameObject);
        eater.resetFoodWeight();
        if(Random.Range(0, 100) < 10){
            SpawnHungry(position);
        }
    }

    public void EatFood(Food food, Schooling eater){
        foodList.Remove(food);
        Vector3 position = food.transform.position;
        Destroy(food.gameObject);
    }

    public void Starve(Agent fish){
        foodMotivateds.Remove(fish);
        if(fish.GetType() == typeof(Hungry)){
            hungries.Remove((Hungry) fish);
        }
        agents.Remove(fish);
        Destroy(fish.gameObject);
    }

    Vector3 calcSchoolingPoint(){
        Vector3 position = Vector3.zero;
        foreach(Schooling fish in schoolings){
            position += fish.transform.position;
        }
        position /= schoolings.Count;

        return position;
    }

    Vector2 PickRandomPoint()
    {
        Vector2 randPoint = Vector2.zero;

        randPoint.x = Random.Range(-ScreenSize.x, ScreenSize.x);
        randPoint.y = Random.Range(-ScreenSize.y, ScreenSize.y);

        return randPoint;
    }
}
