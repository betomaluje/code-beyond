using UnityEngine;

public class AttackModel
{
    public GameObject enemy;
    public Transform position;

    public AttackModel(GameObject gameObject, Transform transform)
    {
        this.enemy = gameObject;
        this.position = transform;
    }
}
