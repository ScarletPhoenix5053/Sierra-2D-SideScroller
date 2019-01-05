using Sierra;
using System.Collections;
using Tutorial.NahuelG_Fighter;
using UnityEngine;

public class AttackManager : MonoBehaviour, IHitboxResponder
{
    public bool Attacking = false;
    public Hitbox Hitbox;
    public AttackData AttackData;

    protected AttackType _atkType = AttackType.Hit;

    public enum AttackType { Hit, Throw }

    protected void Update()
    {
        //GetActionInput();
    }
    protected void FixedUpdate()
    {
        Hitbox.UpdateHitbox();
    }

    // IHitboxResponder
    public void Hit(Collider hurtbox)
    {
        // Check hurtbox blockstate
        bool? success = false;
        if (_atkType == AttackType.Hit)
            success = hurtbox.GetComponent<Hurtbox>()?.
                CheckHit(AttackData.BlockStun, AttackData.HitStun);
        if (_atkType == AttackType.Throw)
            success = hurtbox.GetComponent<Hurtbox>()?.CheckThrow();

        // On successful hit, deal damage and other effects to the character attatched to the hurtbox
        if ((bool)success)
        {
            hurtbox.GetComponent<Hurtbox>().Health.Damage(AttackData);
            hurtbox.GetComponent<Hurtbox>().Health.LogHp();
            Hitbox.SetInactive();
        }
    }

    public void LightAttack()
    {
        if (!Attacking)
        {
            _atkType = AttackType.Hit;
            StopCoroutine(DoLightAttack());
            StartCoroutine(DoLightAttack());
        }
    }
    public void Throw()
    {
        if (!Attacking)
        {
            _atkType = AttackType.Throw;
            StopCoroutine(DoLightAttack());
            StartCoroutine(DoLightAttack());
        }
    }


    protected IEnumerator DoLightAttack()
    {
        // Startup
        Attacking = true;
        Hitbox.SetResponder(this);
        yield return new WaitForSeconds(Utility.FramesToSeconds(AttackData.Startup));

        // Active
        Hitbox.SetActive();
        yield return new WaitForSeconds(Utility.FramesToSeconds(AttackData.Active));

        // Recovery
        Hitbox.SetInactive();
        yield return new WaitForSeconds(Utility.FramesToSeconds(AttackData.Recovery));

        // End
        Attacking = false;
    }
}
