using UnityEngine;

public class Utilities : MonoBehaviour
{

     public enum GameState
    {
        Play, Pause, Main
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public bool RandomBool()
    {
        System.Random random = new System.Random();
        return random.NextDouble() >= 0.5;
    }

    public static void DamageEnemy(GameObject enemyObject, int damage)
    {
        EnemySlimeBehavior slime = enemyObject.GetComponent<EnemySlimeBehavior>();
        if (enemyObject != null)
            slime.TakeDamage(damage);

    }

    public static void DamagePlayer(GameObject enemyObject, int damage)
    {
        EnemySlimeBehavior slime = enemyObject.GetComponent<EnemySlimeBehavior>();
        if (slime.isAlive)
            GameBehavior.Instance.RestartLevel();
        else
            return;
        
    }

}



