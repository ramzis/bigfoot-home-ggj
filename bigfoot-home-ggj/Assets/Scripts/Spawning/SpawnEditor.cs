using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(SpawnContainer))]
public class SpawnEditor : Editor {

    private static bool EditorEnabled = false;
    private GameObject spawnPrefab;
    private GameObject spawnParent;

    private static int spawn_count = 0;
    //If Enable Editor is checked, select a GameObject with SpawnContainer component, then click somewhere in the Scene view
    //to spawn a spawn point

    public enum SpawnType{HOUSES, OBSTACLES};
    public SpawnType spawnType;

    void OnEnable()
    {
        spawnPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Spawn.prefab", typeof(GameObject));
        spawnParent = GameObject.Find("Spawns");
    }

    void OnSceneGUI()
    {
        if (EditorEnabled)
        {
            Selection.activeGameObject = GameObject.Find("Spawns");

            if (Event.current.type == EventType.MouseDown)
            {
                Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(r, out hitInfo))
                {
                    GameObject newPoint = GameObject.Instantiate(spawnPrefab);
                    newPoint.transform.position = hitInfo.point + new Vector3 (0f, 0.5f, 0f);
                    newPoint.transform.parent = spawnParent.transform;

                    //Setup new Spawn's information
                    Spawn spawn_data = newPoint.GetComponent<Spawn>();
                    spawn_data.state = Spawn.State.EMPTY;
                    spawn_data.id = spawn_count;
                    if (spawnType == SpawnType.HOUSES)
                        spawnParent.GetComponent<SpawnContainer>().SpawnList.Add(spawn_data);
                    else if (spawnType == SpawnType.OBSTACLES)
                        spawnParent.GetComponent<SpawnContainer>().ObstacleSpawns.Add(spawn_data);
                    spawn_count++;
                }
            }
        }
    }
	
    public override void OnInspectorGUI()
    {
        EditorEnabled = GUILayout.Toggle(EditorEnabled, "Enable Editor");
        spawnType = (SpawnType)EditorGUILayout.EnumPopup("Spawn Type: ", spawnType);
        if (GUILayout.Button("Clear spawn count")){
            spawn_count = 0;
            if (spawnParent != null)
            {
                SpawnContainer sC = spawnParent.GetComponent<SpawnContainer>();
                if (spawnType == SpawnType.HOUSES){
                    foreach (var spawn in sC.SpawnList)
                    {
                        if (spawn != null)
                            GameObject.DestroyImmediate(spawn.gameObject);
                    }
                    // GameObject[] leftoverClones = GameObject.FindGameObjectsWithTag("SpawnPoint");
                    // if (leftoverClones.Length != 0)
                    //     foreach (GameObject g in leftoverClones)
                    //         GameObject.DestroyImmediate(g);
                    sC.SpawnList.Clear();
                }
                else if (spawnType == SpawnType.OBSTACLES)
                {
                    foreach (var spawn in sC.ObstacleSpawns)
                    {
                        if (spawn != null)
                            GameObject.DestroyImmediate(spawn.gameObject);
                    }
                    sC.ObstacleSpawns.Clear();
                }
            }
        }
    }
}