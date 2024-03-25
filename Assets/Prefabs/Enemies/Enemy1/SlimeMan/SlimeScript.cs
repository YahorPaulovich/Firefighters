using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SlimeScript : Unit
{
    private float timeCounter = 0.0f;
    public float oscillationSpeed = 1.0f;
   public float minSpeed = 20;
    public float maxSpeed = 500;
    public float minScale = 1.5f;
    public float maxScale = 2.5f;

    



    override public void FollowPathAction(Path path, int pathIndex)
    {
        Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * TurnSpeed);

        float sineValue = Mathf.Sin(timeCounter * oscillationSpeed);
        float CurrentSpeed = Mathf.Lerp(minSpeed, maxSpeed, (sineValue + 1.0f) / 2.0f);
        speed = CurrentSpeed;

        float scale = Mathf.Lerp(minScale, maxScale, (sineValue + 1.0f) / 2.0f);
        transform.localScale = new Vector3(Mathf.Clamp(scale, minScale, minScale + .5f), minScale, scale);
        rb.velocity = transform.forward * Time.deltaTime * CurrentSpeed * 200;
    
    timeCounter += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "StickmanContainer")
        {
            //ImpactReceiver.AddImpactOnGameObject(Player.transform.gameObject, (Player.transform.position - transform.position) * Player.KnockBack);
            //HeartScript.TakeDamage(2);
        }
    }
}
