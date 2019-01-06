using UnityEngine;
using System;

[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(MotionController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get { return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y); } }

    private Animator an;
    private AttackManager am;
    private MotionController mc;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0; 

    private enum AttackState
    {

    }
    

    private void Awake()
    {
        an = GetComponent<Animator>();
        am = GetComponent<AttackManager>();
        mc = GetComponent<MotionController>();
    }
    private void LateUpdate()
    {
        mc.ApplyMovement();
        mc.ResetVelocity();

        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        an.SetBool("Walking", false);
        if (IsGrounded)
        {
            an.SetBool("Jumping", false);
        }
        else
        {
            an.SetBool("Jumping", true);
        }
    }

    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        // Light attack button
        if (data.buttons[0])
        {
            if (am.LightAttack())
            {
                switch (am.AtkState)
                {
                    case AttackManager.AttackState.Light1:
                        an.SetTrigger("Attack 1");
                        break;
                    case AttackManager.AttackState.Light2:
                        an.SetTrigger("Attack 2");
                        break;
                    case AttackManager.AttackState.Light3:
                        an.SetTrigger("Attack 3");
                        break;
                    default:
                        throw new Exception("am.LightAttack should set Attack state to Light1, Light2, or Light3. It should not set it to: " + am.AtkState);
                }
            }
            else
            {
                Debug.LogWarning("Cannot chain from " + am.AtkState);            
            }
        }
        // Launcher attack button
        if (data.buttons[1])
        {
            if (am.LauncherAttack())
            {
                an.SetTrigger("Launcher");
            }
        }

        // Jump
        if (data.axes[0] > 0.5 && IsGrounded && jumpLimitTimer <= 0)
        {
            jumpLimitTimer = jumpLimitSeconds;
            mc.UpdateVelocity(new Vector3(mc.XVel, mc.Motion.JumpHeight, 0));
            an.SetBool("Jumping", true);
        }
        // Walk
        if (data.axes[1] !=  0)
        {            
            mc.UpdateVelocity(
                new Vector3(
                    data.axes[1] * mc.Motion.Speed,
                    mc.YVel, 0));
            an.SetBool("Walking", true);

            // Works fine when not flipping character around. will need to update later!
            an.SetInteger("Direction", Convert.ToInt32(data.axes[1]));
        }
    }

   
}