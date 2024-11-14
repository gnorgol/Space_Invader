using UnityEngine;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;

    public float speed = 5.0f;

    private bool _laserActive;

    private void Update()
    {
        MovePlayer();
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();

        }
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, vertical, 0.0f);
        transform.position += direction * speed * Time.deltaTime;

        // Clamp the position of the character so they do not go out of bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftEdge.x, rightEdge.x);
        transform.position = clampedPosition;
    }
    private void Shoot()
    {
        if (!_laserActive)
        {
            Projectile laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.destroyed +=  LaserDestroyed;
            laser.direction = Vector3.up;
            _laserActive = true;
        }
    }
    private void LaserDestroyed() {
        _laserActive = false;
    }
}
