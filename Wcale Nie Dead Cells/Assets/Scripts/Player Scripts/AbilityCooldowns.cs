using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldowns : MonoBehaviour
{
    [SerializeField]
    internal Player mainPlayerScript;
    private float currentDashCooldown;
    internal bool dashReady;
    public float dashCooldown;
    // Start is called before the first frame update
    void Start()
    {
        ApplyDashCooldown();
    }
    // Update is called once per frame
    void Update()
    {
        if(!dashReady)
        {
            currentDashCooldown += Time.deltaTime;
        }
        CheckCooldowns();
    }
    private void CheckCooldowns()
    {
        if(currentDashCooldown >= dashCooldown)
        {
            dashReady = true;
        }
    }
    internal void ApplyDashCooldown()
    {
        dashReady = false;
        currentDashCooldown = 0;
    }
}
