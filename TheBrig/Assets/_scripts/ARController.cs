namespace GoogleARCore {
    using System.Collections.Generic;
    using System.Collections;
    using GoogleARCore.HelloAR;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.Rendering;
    using UnityEngine.UI;

#if UNITY_EDITOR
    using Input = InstantPreviewInput;
#endif

    public class ARController : MonoBehaviour {

        //ARCore stuff
        //
        //UI objects
        public GameObject SearchingForPlaneUI, StartingUI, PlayingUI, EndingUI, QuitUI, DebugUI;

        public Camera FirstPersonCamera;

        // Tracking components
        public GameObject TrackedPlanePrefab;

        /// A list to hold NEW planes ARCore began tracking in the current frame.       
        private List<TrackedPlane> m_NewPlanes = new List<TrackedPlane>();
        /// A list to hold ALL planes ARCore is tracking in the current frame.          
        private List<TrackedPlane> m_AllPlanes = new List<TrackedPlane>();

        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.             
        private bool m_IsQuitting = false;

        //
        //Game  
        //
        public Transform m_PlayerSpawn, m_EnemySpawn;
        //Stage
        public GameObject StagePrefab;
        [HideInInspector] public GameObject StageInstance;
        //Players
        public GameObject PlayerPrefab, EnemyPrefab;
        //private GameObject mettlePlayerInstance;

        public BoolReference allPlayersSpawned;
        bool placeModel = false;


        // Game States
        [SerializeField]
        GAME_STATE currentstate = GAME_STATE.STARTING;

        public enum GAME_STATE { STARTING, PLAYING, ENDING };

        public GAME_STATE CurrentState {

            get { return currentstate; }

            set {
                currentstate = value;

                StopAllCoroutines();

                switch (currentstate) {

                    case GAME_STATE.STARTING:
                        StartCoroutine(RoundStarting());
                        break;

                    case GAME_STATE.PLAYING:
                        StartCoroutine(RoundPlaying());
                        break;

                    case GAME_STATE.ENDING:
                        StartCoroutine(RoundEnding());
                        break;

                }
            }
        }

        //Body and Methods
        private void Start() {

            CurrentState = GAME_STATE.STARTING;

        }

        public void QuitButton() {

            //StartCoroutine(RoundEnding());
            CurrentState = GAME_STATE.ENDING;
            m_IsQuitting = true;

        }

        public void Update() {

            if (Input.GetKey(KeyCode.Escape)) {
                Application.Quit();
            }

            _QuitOnConnectionErrors();

            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking) {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
                if (!m_IsQuitting && Session.Status.IsValid()) {
                    SearchingForPlaneUI.SetActive(true);
                }

                return;
            }

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            Session.GetTrackables<TrackedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++) {
                // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
                // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
                // coordinates.
                GameObject planePrefab = Instantiate(TrackedPlanePrefab, Vector3.zero, Quaternion.identity,
                    transform);
                planePrefab.GetComponent<TrackedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
            }

            // Disable the ARUI when no planes are valid.
            Session.GetTrackables<TrackedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++) {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking) {
                    showSearchingUI = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);

        }


        //COROUTINES
        IEnumerator RoundStarting() {

            while (currentstate == GAME_STATE.STARTING) {

                StartingUI.SetActive(false);
                //CombatUI.SetActive(false);
                SearchingForPlaneUI.SetActive(true);

                Touch touch;

                while (!placeModel && Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) {
                    yield return null;
                }

                // Raycast against the location the player touched to search for planes.
                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                    TrackableHitFlags.FeaturePointWithSurfaceNormal;

                if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && !placeModel) {

                    var StageInstance = Instantiate(StagePrefab, hit.Pose.position, hit.Pose.rotation);

                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    if ((hit.Flags & TrackableHitFlags.PlaneWithinPolygon) != TrackableHitFlags.None) {

                        Vector3 cameraPositionSameY = FirstPersonCamera.transform.position;
                        cameraPositionSameY.y = hit.Pose.position.y;

                        StageInstance.transform.LookAt(cameraPositionSameY, StageInstance.transform.up);
                        placeModel = true;

                    }

                    StageInstance.transform.parent = anchor.transform;

                    StartingUI.SetActive(true);

                    yield return new WaitForSeconds(3);
                    
                    StartCoroutine(SpawnPlayers());

                    if(allPlayersSpawned.Value == true)  {
   
                        StartingUI.SetActive(false);              
                        CurrentState = GAME_STATE.PLAYING;

                        yield break;

                    }

                    yield return null;
                
                }
            }
        }

        IEnumerator RoundPlaying() {

            while (currentstate == GAME_STATE.PLAYING) {

                SearchingForPlaneUI.SetActive(false);
                StartingUI.SetActive(false);
                PlayingUI.SetActive(true);
                QuitUI.SetActive(false);
      
                yield return null;

             }
        }



        IEnumerator RoundEnding() {

            while (currentstate == GAME_STATE.ENDING) {

                PlayingUI.SetActive(false);
                EndingUI.SetActive(true);
                QuitUI.SetActive(false);

                yield return new WaitForSeconds(3);

                EndingUI.SetActive(false);
                SceneManager.LoadScene("Start");

                CurrentState = GAME_STATE.STARTING;

            }

        }

        // Supporting Coroutines
        IEnumerator SpawnPlayers() {

                m_PlayerSpawn = GameObject.FindGameObjectWithTag("playerSpawn").transform;
                m_EnemySpawn = GameObject.FindGameObjectWithTag("enemySpawn").transform;

                Vector3 cameraPositionSameY = FirstPersonCamera.transform.position;

            //var playerInstance = Instantiate(PlayerPrefab, m_PlayerSpawn.transform.position, Quaternion.LookRotation(this.transform.position - m_EnemySpawn.transform.position));        
            var playerInstance = Instantiate(PlayerPrefab, m_PlayerSpawn.transform.position, Quaternion.identity);

            var enemyInstance = Instantiate(EnemyPrefab, m_EnemySpawn.transform.position, Quaternion.identity);
  
                allPlayersSpawned.Value = true;

                yield break;

        }


        /// <summary>
        /// Quit the application if there was a connection error for the ARCore session.
        /// </summary>
        private void _QuitOnConnectionErrors() {
            if (m_IsQuitting) {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted) {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            } else if (Session.Status.IsError()) {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit() {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message) {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null) {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }

    }

}
