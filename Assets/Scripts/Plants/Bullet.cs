using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10;
    public Vector3 direction = new Vector3(1, 0, 0);
    public float speed = 200f;

    protected virtual void Start() { }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (transform.position.x > Values.BULLET_MAX_X || transform.position.x < Values.BULLET_MIN_X)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Zombie")
        {
            other.GetComponent<Zombie>().GetDamage(damage);
            DestroyBullet();
        }
    }

    protected virtual void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}
