using UnityEngine;

public interface IWeapon
{
    public void Attack();
}


public class WeaponLauncher
{
    IWeapon currentWeapon;

    public void Launch(IWeapon weapon)
    {
        currentWeapon = weapon;
    }


    public void Attack()
    {
        currentWeapon.Attack();
    }
}
