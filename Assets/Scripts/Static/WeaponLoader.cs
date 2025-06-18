using UnityEngine;

public static class WeaponLoader
{
    public static void LoadWeaponData(this Transform weaponHolder,
                                      GameObject newWeapon,
                                      out Weapon newWeaponCore)
    {
        //for syntax
        newWeaponCore = null;

        //clear pre weapon gameobjects
        weaponHolder.KillAllChilds();

        //load new weapon
        GameObject _weaponInstance = Object.Instantiate(newWeapon, weaponHolder);
        SpriteRenderer[] spriteRenderers = _weaponInstance.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) spriteRenderer.sortingOrder += 50;
        newWeaponCore = _weaponInstance.GetComponentEx<Weapon>();

        //set tag
        if (newWeaponCore != null)
        {
            newWeaponCore.weaponTag = weaponHolder.tag;
            newWeaponCore.gameObject.tag = weaponHolder.tag;
        }
        else
        {
            Debug.LogWarning("WeaponLoader: Weapon core is NULL");
        }

    }
}
