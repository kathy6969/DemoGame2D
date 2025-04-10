using UnityEngine;

public class AttackTestTrigger : MonoBehaviour
{
    public Attack4_FireOrbStraight attack;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attack.ExecuteAttack4();
        }
    }
}
