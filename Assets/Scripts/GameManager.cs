using System.Collections;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _main { get; set; }
    public static GameManager Main => _main;

    [Header("Text Meshes")]
    public TextMeshProUGUI text;
    public TextMeshProUGUI[] scoresText;
    
    [Header("Bounds")]
    public float YBounds = 3.3f;
    public float XBounds = 8f;
    
    [Header("Ball Props")]
    public GameObject ballPrefab;
    public float ballSpeed = 5f;

    private WaitForSeconds _oneSec;
    private int[] _scores;

    #region EntityRelated

    private EntityManager _entityManager;
    private Entity _entityBallPrefab;
    private BlobAssetStore _blob;
    
    #endregion EntityRelated

    private void Awake()
    {
        if (_main != null && _main != this)
        {
            Destroy(gameObject);
            return;
        }

        _main = this;
    }

    private void Start()
    {
        _oneSec = new WaitForSeconds(1f);
        _scores = new []{0, 0};
        
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        _blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, 
                                                              _blob);
        _entityBallPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, settings);

        StartCoroutine(nameof(StartRound));
    }

    private IEnumerator StartRound()
    {
        text.text = "Get Ready...";
        yield return _oneSec;

        text.text = "3";
        yield return _oneSec;

        text.text = "2";
        yield return _oneSec;

        text.text = "1";
        yield return _oneSec;
        
        text.text = "";
        // Start the game by spawning the ball
         SpawnBall();
    }

    private void SpawnBall()
    {
        var entity = _entityManager.Instantiate(_entityBallPrefab);
        
        //Add a random linear velocity when the ball is instantiated.
        var direction = Random.Range(0, 2) == 1 ? 1 : -1;
        var physicsVel = new PhysicsVelocity
        {
            Angular = new float3(0.0f),
            Linear = new float3(ballSpeed * direction, Random.Range(0.0f, 2.0f), 0.0f)
        };
        _entityManager.AddComponentData(entity, physicsVel);
    }

    public void PlayerScored(int index)
    {
        _scores[index] += 1;
        scoresText[index].text = _scores[index].ToString();

        StartCoroutine(nameof(StartRound));
    }

    private void OnDestroy()
    {
        _blob.Dispose();
    }
}
