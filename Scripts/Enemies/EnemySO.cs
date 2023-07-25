using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Characters/Enemy")]
public class EnemySO : ScriptableObject
{
    public int health = 10;
    public int attackPower = 1;
    public float fireRate = 1f;

    public float attackRadius = 5f;
}
