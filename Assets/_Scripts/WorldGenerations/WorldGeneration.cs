using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGeneration : MonoBehaviour
{
    //Gameplay
    private float chunkSpawnZ;
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    //Configurable field
    [SerializeField] private int firstChunkSpawnPosition = -10;
    [SerializeField] private int chunksOnScreen = 3;
    [SerializeField] private float despawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;

    #region TO DELETE $$
    private void Awake()
    {
        ResetWorld();
    }
    #endregion

    private void Start()
    {
        //Check if we have an empty chunkPrefab list
        if (chunkPrefab.Count == 0)
        {
            print("prefabList is empty, asign some chunks to the list");
            return;
        }
        //try to asign cameraTransform here
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            print("the cameraTransform is asigned to the Camera.main");
        }
    }

    private void Update()
    {
        ScanPosition();
    }

    private void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek();
        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + despawnDistance)
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }

    }

    private void SpawnNewChunk()
    {
        //Get a random index for which chunk to spawn

        int randomIndex = Random.Range(0, chunkPrefab.Count);

        //Does already exist within our pool
         Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name + "(Clone)"));

        //Create a chunk if were no able to find one to reuse
        if (!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        //place the object and show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLength;

        //Store the value to reuse in our pool
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }

    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue();
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }

    public void ResetWorld()
    {
        //Reset the ChunkSpawn Z
        chunkSpawnZ = firstChunkSpawnPosition;
        for (int i = activeChunks.Count; i != 0; i--)
        {
            DeleteLastChunk();
        }

        for (int i = 0; i < chunksOnScreen; i++)
        {
            SpawnNewChunk();
        }
    }
}
