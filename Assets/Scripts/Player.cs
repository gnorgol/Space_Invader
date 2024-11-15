using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    private Projectile _laser;

    public float speed = 5.0f;

    public AudioClip laserSound;
    public AudioClip explosionSound;


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

        Vector3 direction = new Vector3(horizontal, 0.0f, 0.0f);
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
        if (_laser == null)
        {
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
            _laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            _laser.direction = Vector3.up;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader") || collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            GameManager.Instance.OnPlayerKilled(this);
        }
    }
}
