using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation.Examples;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Dictionary<GameObject, bool> _cantCollide = new();
    private void OnCollisionEnter(Collision other)
    {
        if (_cantCollide.TryGetValue(other.collider.gameObject, out var state))
        {
            if (state) return;
        }
        var player = other.collider.GetComponentInParent<PathFollower>();
        var ai = other.collider.GetComponentInParent<AIPathFollower>();
        if (player)
        {
            player.Destroy();
            StartCoroutine(RemoveAfterSecs(other.collider.gameObject));
        }
        if (ai)
        {
            ai.Destroy();
            StartCoroutine(RemoveAfterSecs(other.collider.gameObject));
        }
    }

    private IEnumerator RemoveAfterSecs(GameObject gameObject)
    {
        _cantCollide.TryAdd(gameObject, true);
        yield return new WaitForSeconds(4.0f);
        if(_cantCollide.TryGetValue(gameObject, out var boo))
        {
            _cantCollide[gameObject] = false;
        }
    }
}
