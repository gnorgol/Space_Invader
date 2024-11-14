using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int row = 5;
    public int column = 11;
    public float speed = 1.0f;

    private Vector3 _direction = Vector3.right;


    private void Awake()
    {
        for (int i = 0; i < row; i++)
        {
            float width = (column-1) * 2.0f;
            float height = (row - 1) * 2.0f;
            Vector3 centering = new Vector3(-width / 2.0f, -height / 2.0f, 0.0f);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (i* 2.0f), 0.0f);
            for (int j = 0; j < column; j++)
            {
                Invader invader = Instantiate(prefabs[i], transform);
                Vector3 position = rowPosition;
                position.x += j * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }
    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;

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
}
