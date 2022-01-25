using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal Rigidbody2D rb;
    internal Vector2 launchDirection;
    internal float damage;
    internal float speed;
    internal DestroyAfterTime destroyingScript;
    public LayerMask platformsLayerMask;
    public LayerMask elevatorsLayerMask;
    public LayerMask destroyableLayerMask;
    public LayerMask enemyLayerMask;
    public LayerMask deadEnemyLayerMask;
    public GameObject bloodParticlesPrefab;
}
