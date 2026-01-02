using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform player;

    IEnumerator Start()
    {
        yield return null;

        if (!SaveManager.I.HasPlayerTransform())
            yield break;

        var agent = player.GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.Warp(SaveManager.I.GetPlayerPosition());
            player.rotation = SaveManager.I.GetPlayerRotation();
        }
        else
        {
            player.SetPositionAndRotation(
                SaveManager.I.GetPlayerPosition(),
                SaveManager.I.GetPlayerRotation()
            );
        }
    }
}
