using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource))]
public class Invaders : MonoBehaviour
{
    public static Invaders instance;
    public Invader[] prefabs;
    public int row = 5;
    public int column = 11;
    public AnimationCurve speed;
    public Projectile missilePrefab;
    public float missileAttackRate = 1.0f;
    public AudioClip invaderKilledSound;

    public int amountKilled { get; private set; }
    public int totalInvaders => row * column;
    public float progressInvadersKilled => (float)amountKilled / (float)totalInvaders;

    public int amountAlive => totalInvaders - amountKilled;


    private Vector3 _direction = Vector3.right;
    private Vector3 initialPosition;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        initialPosition = transform.position;
        CreateInvaders();
    }

    private void CreateInvaders()
    {
        for (int i = 0; i < row; i++)
        {
            float width = (column - 1) * 2.0f;
            float height = (row - 1) * 2.0f;
            Vector3 centering = new Vector3(-width / 2.0f, -height / 2.0f, 0.0f);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (i * 2.0f), 0.0f);
            for (int j = 0; j < column; j++)
            {
                Invader invader = Instantiate(prefabs[i], transform);
                invader.killed += InverderKilled;
                Vector3 position = rowPosition;
                position.x += j * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileAttackRate, missileAttackRate);
    }
    private void Update()
    {
        // Calculate the percentage of invaders killed
        int totalCount = row * column;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = amountKilled / (float)totalCount;
        transform.position += _direction * speed.Evaluate(progressInvadersKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (_direction == Vector3.right && invader.position.x >=  rightEdge.x - 1.0f)
            {
                AdvanceRow();
            }
            else if (_direction == Vector3.left && invader.position.x <= leftEdge.x + 1.0f)
            {
                AdvanceRow();
                
            }
        }
    }
    private void AdvanceRow()
    {
        _direction.x *= -1.0f;


        Vector3 position = transform.position;
        position.y -= 1.0f;
        transform.position = position;
    }
    private void InverderKilled()
    {
        amountKilled++;
        AudioSource.PlayClipAtPoint(invaderKilledSound, transform.position);
        if (amountKilled >= totalInvaders)
        {
            Debug.Log("You win!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void ResetInvaders()
    {
        _direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform)
        {
            invader.gameObject.SetActive(true);
        }
    }
    private void MissileAttack()
    {
        if(GetAliveCount() == 0)
        {
            return;
        }
        foreach (Transform invader in transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }
            if (Random.value < (1.0f / (float)amountAlive))
            {
                Projectile missile = Instantiate(missilePrefab, invader.position, Quaternion.identity);
                missile.direction = Vector3.down;
                break;
            }
        }
    }
    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
