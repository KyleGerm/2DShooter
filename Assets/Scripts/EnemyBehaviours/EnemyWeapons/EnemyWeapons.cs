using UnityEngine;

public class EnemyWeapons : Weapon
{
    /// <summary>
    /// Takes in an enum, and a difficulty multiplier to be applied to weapons.
    /// Runs the appropriate method based on the enum given.
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="weaponType"></param>
    /// <param name="difficulty"></param>
    public void InitializeWeaponType(WeaponSelect weaponType, float difficulty)
    {
        switch (weaponType)
        {
            case WeaponSelect.PISTOL:
                EquipPistol(difficulty);
                break;
            case WeaponSelect.SNIPER:
                EquipSniper(difficulty);
                break;
            case WeaponSelect.SMG:
                EquipSMG(difficulty);
                break;
            case WeaponSelect.RIFLE:
                EquipRifle(difficulty);
                break;
            case WeaponSelect.SHOTGUN:
                EquipShotgun(difficulty);
                break;
        }
            WeaponType = weaponType;

    }

    private void EquipPistol(float multi)
    {
        damage = Mathf.CeilToInt(Random.Range(5, 12) * multi);
        range = Random.Range(3f, 5f);
        fireSpeed = 1.5f * multi;
        magSize = Mathf.CeilToInt(8 * multi);
        reloadTime = 3f;
        reloadTime -= reloadTime / 10 * multi * (multi - 1);
        accuracy = Random.Range(5f, 10f);
        accuracy -= accuracy / 20 * multi * (multi - 1);
        ProjectileSpeed = 8f;
        Projectiles = 1;
        currentClip = magSize;
        isReloaded = true;
        cooldown = 0;
    }

    private void EquipSMG(float multi)
    {
        damage = Mathf.CeilToInt(Random.Range(3, 8) * multi);
        range = Random.Range(4f, 7f);
        fireSpeed = 5f;
        fireSpeed += fireSpeed / 10 * multi * (multi - 1);
        magSize = Mathf.CeilToInt(20 * multi);
        reloadTime = 5f;
        reloadTime -= reloadTime / 10 * multi * (multi - 1);
        accuracy = Random.Range(8f, 15f);
        accuracy -= accuracy / 20 * multi * (multi - 1);
        ProjectileSpeed = 8f;
        Projectiles = 1;
        currentClip = magSize;
        isReloaded = true;
        cooldown = 0;

    }
    private void EquipSniper(float multi)
    {
        damage = Mathf.CeilToInt(Random.Range(30, 55) * multi);
        range = Random.Range(10f, 20f);
        fireSpeed = 0.4f;
        fireSpeed += fireSpeed / 10 * multi * (multi - 1);
        magSize = Mathf.CeilToInt(6 * multi);
        reloadTime = 7f;
        reloadTime -= reloadTime / 10 * multi * (multi - 1);
        accuracy = Random.Range(2f, 5f);
        accuracy -= accuracy / 20 * multi * (multi - 1);
        ProjectileSpeed = 20f;
        Projectiles = 1;
        currentClip = magSize;
        isReloaded = true;
        cooldown = 0;

    }
    private void EquipRifle(float multi)
    {
        damage = Mathf.CeilToInt(Random.Range(10, 20) * multi);
        range = Random.Range(8f, 15);
        fireSpeed = 2.5f;
        fireSpeed += fireSpeed / 10 * multi * (multi - 1);
        magSize = Mathf.CeilToInt(30 * multi);
        reloadTime = 6f;
        reloadTime -= reloadTime / 10 * multi * (multi - 1);
        accuracy = Random.Range(4f, 10f);
        accuracy -= accuracy / 20 * multi * (multi - 1);
        ProjectileSpeed = 10f;
        Projectiles = 1;
        currentClip = magSize;
        isReloaded = true;
        cooldown = 0;

    }

    private void EquipShotgun(float multi)
    {
        damage = Mathf.CeilToInt(Random.Range(25, 40) * multi);
        range = Random.Range(3f, 5f);
        fireSpeed = 0.3f;
        fireSpeed += fireSpeed / 10 * multi * (multi - 1);
        magSize = Mathf.CeilToInt(8 * multi);
        reloadTime = 8f;
        reloadTime -= reloadTime / 10 * multi * (multi - 1);
        accuracy = Random.Range(10f, 15f);
        accuracy -= accuracy / 20 * multi * (multi - 1);
        ProjectileSpeed = 6f;
        Projectiles = 6;
        currentClip = magSize;
        isReloaded = true;
        cooldown = 0;
    }
}
