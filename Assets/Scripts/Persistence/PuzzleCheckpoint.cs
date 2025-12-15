using UnityEngine;

public class PuzzleCheckpoint : MonoBehaviour
{
    [SerializeField] private string checkpointID;
    [SerializeField] private Transform respawnPoint;

    public string ID => checkpointID;
    public Vector3 Position => respawnPoint.position;
    public Quaternion Rotation => respawnPoint.rotation;
}
