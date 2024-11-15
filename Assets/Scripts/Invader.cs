using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public System.Action killed;

    public int score = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            GameManager.Instance.OnInvaderKilled(this);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            GameManager.Instance.OnBoundaryReached();
        }
    }
}
