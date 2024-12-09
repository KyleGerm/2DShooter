using UnityEngine;

public class BulletBehaviour : MonoBehaviour , IPoolable
{
   public int damage;
    public float range;
    private float distance;
    Vector2 startPoint;
    private float time;
  
    private void OnEnable()
    {
     startPoint = transform.position;
        time = 0f;
    }
    void Update()
    {
        time += Time.deltaTime;
        distance =  Vector2.Distance(startPoint,transform.position);

        //If bullet goes out of range, is active for too long, or does not move enough in the first half second, return to pool
       if(distance > range || time >= 3f || distance < 1 && time >= .5f) 
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision != null && collision.gameObject.TryGetComponent(out Health h))
        {
            h.TakeDamage(damage);
        }
        
        ReturnToPool(); 
    }
   
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
