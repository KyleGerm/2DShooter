using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour ,IGunStatistics
{
    private string description = "";
    protected int damage;
    protected float fireSpeed;
    protected float range;
    protected float accuracy;
    protected int clip;
    [SerializeField] protected Transform firePoint;
    protected float reloadTime;
    protected float cooldown;
    protected bool isReloaded = true;

    protected int magSize;
    protected int currentClip;
    protected PlayerController player;

    public string Description
    {
        get => description;
        protected set => description = value;
    }
     public int Projectiles {get; protected set; }
    public float ProjectileSpeed{get; protected set; }
    public int CurrentClip { get { return clip; } protected set => clip = value; }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    public float Range
    {
        get => range;
        set => range = value;
    }
    public int MagSize
    {
        get => magSize;
        set => magSize = value;
    }
    public float FireRate
    {
        get => fireSpeed;
        set => fireSpeed = value;
    }
    public float ReloadTime
    {
        get => reloadTime;
        set => reloadTime = value;
    }
    public float Accuracy
    {
        get => accuracy;
        set
        {
            accuracy = value;
        }
    }
    public bool IsReloaded
    {
        get => isReloaded;
    }
     public WeaponSelect WeaponType { get; protected set; }
 

    /// <summary>
    /// Fires a projectile
    /// </summary>
    public void Shoot()
    {

        if (cooldown <= 0f && CurrentClip > 0)
        {
            float deviation;
            Vector3 aimPos;
            Quaternion newRot;
            if (Projectiles == 1)
            {
                deviation = Random.Range(-accuracy, accuracy);
                aimPos = new Vector3(0, 0, deviation) + firePoint.rotation.eulerAngles;
                newRot = Quaternion.Euler(aimPos);
                GameObject bullet = GameManager.Instance.Pool.GetNewObject(PrefabList.BULLET, true);

                if (!bullet)
                {
                    return;
                }
                
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;
                bullet.SetActive(true);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                BulletBehaviour behaviour = bullet.GetComponent<BulletBehaviour>();
                rb.transform.rotation = newRot;
                rb.AddForce(rb.transform.up * ProjectileSpeed, ForceMode2D.Impulse);
                behaviour.damage = this.damage;
                behaviour.range = this.range;

            }
            else if (Projectiles > 1)
            {
                float spread = (accuracy * 2) / (float)Projectiles;
                deviation = -accuracy;
                GameObject[] bullets = GameManager.Instance.Pool.RequestMultiple(Projectiles, PrefabList.BULLET, true);
                if (bullets == null)
                {
                    return;
                }

                for (int i = 0; i < Projectiles; i++)
                {
                    aimPos = new Vector3(0, 0, deviation) + firePoint.rotation.eulerAngles;
                    newRot = Quaternion.Euler(aimPos);

                    bullets[i].transform.position = firePoint.position;
                    bullets[i].transform.rotation = firePoint.rotation;
                    bullets[i].SetActive(true);
                    Rigidbody2D rb = bullets[i].GetComponent<Rigidbody2D>();
                    BulletBehaviour behaviour = bullets[i].GetComponent<BulletBehaviour>();
                    rb.transform.rotation = newRot;
                    rb.AddForce(rb.transform.up * ProjectileSpeed, ForceMode2D.Impulse);
                    behaviour.damage = (int)Mathf.Ceil((float)this.damage / Projectiles);
                    behaviour.range = this.range;
                    deviation += spread;
                }

            }
            cooldown = 1 / FireRate;
            CurrentClip--;
            StartCoroutine(Cooldown());

            CheckForPlayer();

        }
        else if (CurrentClip <= 0 && isReloaded)
        {
            BeginReload();
        }
    }

    /// <summary>
    /// Time before the next shot can be fired
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cooldown()
    {
        while (cooldown >= 0f)
        {
            cooldown -= Time.deltaTime;
            if (cooldown > 0f) 
            {
                yield return null;
            }
        }
        StopCoroutine("Cooldown");
    }
    /// <summary>
    /// Unloads the weapon and begins the reload process
    /// </summary>
    public void BeginReload()
    {
        CurrentClip = 0;
        isReloaded = false;
        StartCoroutine(Reload(reloadTime));
    }

    private IEnumerator Reload(float timeLeft)
    {
        while (timeLeft >= 0f)
        {
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        clip = MagSize; ;
        isReloaded = true;
       CheckForPlayer() ;
        StopCoroutine("Reload");
    }

    /// <summary>
    /// Checks for a playercontroller attatched to this
    /// </summary>
    protected void CheckForPlayer()
    {
        if (player != null)
        {
            GameManager.Instance.EventManager.ChangeAmmoCount(CurrentClip, MagSize);
        }
    }

}
