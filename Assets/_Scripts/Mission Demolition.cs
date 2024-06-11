using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private SIngleton
    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start()
    {
        S = this;
        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        //Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //Destory old projectiles if they exist(Method is not yet written)
        Projectile.DESTROY_PROJECTILES();

        //Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        //Reset the goal

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        //Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

     
        void Update()
        {
        UpdateGUI();

        //Check for level end
        if((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;

            //Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
        }
    void NextLevel()
    {
        level++;
        if(level== levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }

    //Static method that allwos code antywhere to increment shotsTaken

    static public void SHOT_FIRED()
    {
        S.shotsTaken++;
    }
    //Static ethod that allows code anywhere to get a reference to S.castle

    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
    }
