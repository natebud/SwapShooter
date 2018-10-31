using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon {

    void Fire();

    void Reload();

    IEnumerator ShotEffect(float x, float y);

}
