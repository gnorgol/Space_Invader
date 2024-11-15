using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public System.Action killed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            gameObject.SetActive(false);
            if (killed != null)
                killed.Invoke();
        

            Destroy(this.gameObject);
        }
    }
}
