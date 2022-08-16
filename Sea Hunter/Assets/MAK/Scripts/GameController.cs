using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Invector
{
    using UnityEngine.Events;
    using vCharacterController;
    [vClassHeader("Simple GameController Example", openClose = false)]
    public class GameController : vMonoBehaviour
    {
        [System.Serializable]
        public class OnRealoadGame : UnityEngine.Events.UnityEvent { }
        [vHelpBox("Assign your Character Prefab to be instantiate at the SpawnPoint, leave it unassigned to Restart the Scene instead")]
        public GameObject playerPrefab;
        [vHelpBox("Assign a empty transform to spawn the Player to a specific location")]
        public Transform spawnPoint;
        [vHelpBox("Time to wait until the scene restart or the player will be spawned again")]
        public float respawnTimer = 4f;
        [vHelpBox("Check this if you want to destroy the dead body after the respawn")]
        public bool destroyBodyAfterDead;
        [vHelpBox("Display a message using the FadeText UI")]
        public bool displayInfoInFadeText = true;

        [HideInInspector]
        public OnRealoadGame OnReloadGame = new OnRealoadGame();
        [HideInInspector]
        public GameObject currentPlayer;
        private vThirdPersonController currentController;
        public static GameController instance;
        private GameObject oldPlayer;
        public vThirdPersonController newPlayer;

        public UnityEvent onSpawn;
        public Animator animator1;
        public string OpeningAnimationName;
        public int treasureCount = 0;
        public GameObject levelCleared;
        public GameObject missionText;
        protected virtual void Start()
        {
            if (instance == null)
            {
                instance = this;
                //DontDestroyOnLoad(this.gameObject);
                this.gameObject.name = gameObject.name + " Instance";
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            StartCoroutine(Cinematic());

        }

        IEnumerator Cinematic()
        {
            missionText.SetActive(true);
            while (animator1.GetCurrentAnimatorStateInfo(0).IsName(OpeningAnimationName))
            {
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            animator1.gameObject.SetActive(false);
            animator1.gameObject.SetActive(false);
            missionText.SetActive(false);
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
            if (displayInfoInFadeText && vHUDController.instance)
            {
                vHUDController.instance.ShowText("Init Scene");
            }

            FindPlayer();
        }

        protected virtual void OnCharacterDead(GameObject _gameObject)
        {
            oldPlayer = _gameObject;

            if (playerPrefab != null)
            {
                StartCoroutine(RespawnRoutine());
            }
            else
            {
                if (displayInfoInFadeText && vHUDController.instance)
                {
                    vHUDController.instance.ShowText("Restarting Scene...");
                }

                Invoke("ResetScene", respawnTimer);
            }
        }

        protected virtual IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnTimer);

            if (playerPrefab != null && spawnPoint != null)
            {
                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    if (displayInfoInFadeText && vHUDController.instance)
                    {
                        vHUDController.instance.ShowText("Player destroyed: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    Destroy(oldPlayer);
                }
                else
                {
                    if (displayInfoInFadeText && vHUDController.instance)
                    {
                        vHUDController.instance.ShowText("Remove Player Components: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    DestroyPlayerComponents(oldPlayer);
                }

                yield return new WaitForEndOfFrame();

                currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
                currentController = currentPlayer.GetComponent<vThirdPersonController>();
                currentController.onDead.AddListener(OnCharacterDead);

                if (displayInfoInFadeText && vHUDController.instance)
                {
                    vHUDController.instance.ShowText("Respawn player: " + currentPlayer.name.Replace("(Clone)", ""));
                }

                OnReloadGame.Invoke();
                onSpawn.Invoke();
            }
        }

        protected virtual void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            if (currentController.currentHealth > 0)
            {
                if (displayInfoInFadeText && vHUDController.instance)
                {
                    vHUDController.instance.ShowText("Load Scene: " + scene.name);
                }

                return;
            }
            if (displayInfoInFadeText && vHUDController.instance)
            {
                vHUDController.instance.ShowText("Reload Scene");
            }

            OnReloadGame.Invoke();
            FindPlayer();

        }

        protected virtual void FindPlayer()
        {
            //; var player = GameObject.FindObjectOfType<vThirdPersonController>();
            var player = newPlayer;
            player.gameObject.SetActive(true);
            if (player)
            {
                currentPlayer = player.gameObject;
                currentController = player;
                player.onDead.AddListener(OnCharacterDead);
                if (displayInfoInFadeText && vHUDController.instance)
                {
                    vHUDController.instance.ShowText("Found player: " + currentPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                }
            }
            else if (currentPlayer == null && playerPrefab != null && spawnPoint != null)
            {
                SpawnAtPoint(spawnPoint);
            }
        }

        protected virtual void DestroyPlayerComponents(GameObject target)
        {
            if (!target)
            {
                return;
            }

            var comps = target.GetComponentsInChildren<MonoBehaviour>();
            for (int i = 0; i < comps.Length; i++)
            {
                Destroy(comps[i]);
            }
            var coll = target.GetComponent<Collider>();
            if (coll != null)
            {
                Destroy(coll);
            }

            var rigidbody = target.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }

            var animator = target.GetComponent<Animator>();
            if (animator != null)
            {
                Destroy(animator);
            }
        }

        /// <summary>
        /// Set a custom spawn point (or use it as checkpoint to your level) 
        /// </summary>
        /// <param name="newSpawnPoint"> new point to spawn</param>
        public virtual void SetSpawnSpoint(Transform newSpawnPoint)
        {
            spawnPoint = newSpawnPoint;
        }

        /// <summary>
        /// Spawn New Player at a specific point
        /// </summary>
        /// <param name="targetPoint"> Point to spawn player</param>
        public virtual void SpawnAtPoint(Transform targetPoint)
        {
            if (playerPrefab != null)
            {
                if (oldPlayer != null && destroyBodyAfterDead)
                {
                    if (displayInfoInFadeText && vHUDController.instance)
                    {
                        vHUDController.instance.ShowText("Player destroyed: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    Destroy(oldPlayer);
                }

                else if (oldPlayer != null)
                {
                    if (displayInfoInFadeText && vHUDController.instance)
                    {
                        vHUDController.instance.ShowText("Remove Player Components: " + oldPlayer.name.Replace("(Clone)", "").Replace("Instance", ""));
                    }

                    DestroyPlayerComponents(oldPlayer);
                }

                currentPlayer = Instantiate(playerPrefab, targetPoint.position, targetPoint.rotation);
                currentController = currentPlayer.GetComponent<vThirdPersonController>();
                currentController.onDead.AddListener(OnCharacterDead);
                OnReloadGame.Invoke();

                if (displayInfoInFadeText && vHUDController.instance)
                {
                    vHUDController.instance.ShowText("Spawn player: " + currentPlayer.name.Replace("(Clone)", ""));
                }
            }
        }

        /// <summary>
        /// Reload  current Scene and current Player
        /// </summary>
        public virtual void ResetScene()
        {
            SpawnPlayer();
            /*   if (oldPlayer)
               {
                   DestroyPlayerComponents(oldPlayer);
               }

               var scene = SceneManager.GetActiveScene();
               SceneManager.LoadScene(scene.name);

               if (oldPlayer && destroyBodyAfterDead)
               {
                   Destroy(oldPlayer);
               }*/
        }

        public void SetSpawnPosition(GameObject NewSpawnPosition)
        {
            spawnPoint = NewSpawnPosition.transform;
        }

        public void SpawnPlayer()
        {
            /*     currentController.AddHealth(currentController.maxHealth);
                 currentPlayer.transform.position = spawnPoint.transform.position;*/
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SpawnPlayer();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopAllCoroutines();
                animator1.gameObject.SetActive(false);
                animator1.gameObject.SetActive(false);
                missionText.SetActive(false);
                SceneManager.sceneLoaded += OnLevelFinishedLoading;
                if (displayInfoInFadeText && vHUDController.instance)
                {
                    vHUDController.instance.ShowText("Init Scene");
                }

                FindPlayer();
            }
        }
  
        public void CollectTreasure()
        {
            treasureCount++;
            if (treasureCount > 3)
            {
                levelCleared.SetActive(true);
                LevelClear();
            }
        }

        private void LevelClear()
        {
            Invoke("LoadScene", 4f);
        }

        public void LoadScene()
        {
            int count = SceneManager.sceneCountInBuildSettings;
            var scene = SceneManager.GetActiveScene();
            if (count > scene.buildIndex)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(0);
            }

        }

    }
}