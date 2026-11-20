using UnityEngine;

// base class for things the player can pick up
public class Collactable : Targetable
{
    // picking up = removing the object from the scene
    public virtual void Collect()
    {
        Destroy(this.gameObject);
    }
}
