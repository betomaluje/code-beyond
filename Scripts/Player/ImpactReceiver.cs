﻿using UnityEngine;

public class ImpactReceiver : MonoBehaviour
{
    float mass = 6.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        // normalize force vector to get direction only and trim magnitude
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
}
