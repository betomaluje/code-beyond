﻿using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private AttackForwarder playerAttackForwarder;

    void OnTriggerEnter(Collider col)
    {
        if (LayerMaskUtils.LayerMatchesObject(attackLayer, col.gameObject))
        {
            playerAttackForwarder.AddEnemy(new AttackModel(col.gameObject, col.transform));
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (LayerMaskUtils.LayerMatchesObject(attackLayer, col.gameObject))
        {
            playerAttackForwarder.RemoveEnemy(new AttackModel(col.gameObject, col.transform));
        }
    }
}
