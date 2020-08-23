using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Logic : MonoBehaviour
{
    public float tickLength;
    public int mapSize;

    private int currentTick;
    private float lastTick;
    private bool isPaused = true;
    public bool tutorial = true;
    public bool isVictorious = false;

    public Tile[,] grid;

    public int waveLength = 20;
    public int waveSpawnDistance = 2;
    public int waveNumber = 0;
    public int waveClearingTreshold = 2;
    public int movesRemaining = 0;
    public int bossWave = 10;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Tile[mapSize, mapSize];

        for(int x = 0; x<mapSize; x++)
        {
            for(int y = 0; y<mapSize; y++)
            {
                grid[x, y] = Instantiate(Resources.Load("Prefabs/Tile") as GameObject, GameObject.Find("Arena/Grid").transform).GetComponent<Tile>();
                grid[x, y].gameObject.name = "Tile [" + x + ";" + y + "]";
                grid[x, y].transform.position = new Vector2(-mapSize/2+x, -mapSize/2+y);
                grid[x, y].x = x;
                grid[x, y].y = y;
                float newColor = Random.Range(0, 0.30f)+0.1f;
                grid[x, y].GetComponent<SpriteRenderer>().color = new Color(newColor, newColor, newColor, 1f);
            }
        }

        GameObject.Find("Arena/Entities/Player").GetComponent<Entity>().position = grid[mapSize / 2, mapSize / 2];
        grid[mapSize / 2, mapSize / 2].contester = GameObject.Find("Arena/Entities/Player").GetComponent<Entity>();
        grid[mapSize / 2, mapSize / 2].contester.transform.position = grid[mapSize / 2, mapSize / 2].transform.position;

        GameObject.Find("Arena/Entities/Player").GetComponent<Player>().currHP = 5;
        GameObject.Find("Arena/Entities/Player").GetComponent<Player>().maxHP = 5;
        GameObject.Find("Arena/Entities/Player").GetComponent<Player>().str = 1;



        ShrinkMap();

        for (int x = 0; x<9; x++)
        {
            for (int y = 0; y<9; y++)
            {
                if (x == 1 && y == 1 || x== 7 && y==7 || x==1 &&y==7 || x==7 &&y==1 || x == 1 && y == 4 || x == 4 && y == 1 || x == 7 && y == 4 || x == 4 && y == 7)
                {
                    grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].contester = Instantiate(Resources.Load("Prefabs/Enemies/Knight") as GameObject, GameObject.Find("Arena/Entities").transform).GetComponent<Entity>();
                    grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].contester.position = grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y];
                    grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].contester.transform.position = grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].transform.position;
                }
                grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[15];

                Color newColor = Color.white;
                if ((x+y%2)%2==0)
                {
                    newColor = Color.Lerp(newColor, Color.gray, 0.5f);
                }
                grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].GetComponent<SpriteRenderer>().color = newColor;

            }
        }
    }

    public void GameStart()
    {
        if (tutorial)
        {
            GameObject.Find("Canvas/Tutorial").GetComponent<SubMenu>().Enter();
            GameObject.Find("Canvas").GetComponent<UI>().ClickTutorial();
            isPaused = true;
        }
        else
        {
            StartingSpawn();
            isPaused = false;
        }
    }

    public void Restart()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy.gameObject);
        }
        foreach (GameObject pckp in GameObject.FindGameObjectsWithTag("Pickup"))
        {
            Destroy(pckp.gameObject);
        }
        for (int x = 0; x<grid.GetLength(0); x++)
        {
            for(int y = 0; y<grid.GetLength(1); y++)
            {
                Destroy(grid[x, y].gameObject);
            }
        }

        GameObject.FindWithTag("Player").GetComponent<Player>().isMoving = false;
        GameObject.FindWithTag("Player").GetComponent<Player>().isAttacking = false;
        GameObject.FindWithTag("Player").GetComponent<Player>().actionPerformed = false;
        GameObject.FindWithTag("Player").GetComponent<Player>().alive = true;


        Camera.main.GetComponent<CameraHandler>().following = true;

        waveNumber = 0;
        movesRemaining = 0;
        currentTick = 0;
        isPaused = false;
        tutorial = false;
        isVictorious = false;

        Start();
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().Start();
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().RefreshUI();
    }

    void Update()
    {
        if (!IsPaused())
        {
            if (Time.time - lastTick >= tickLength)
            {
                Tick();
            }
            UpdatePacer();
            UpdateCounter();
        }
        else
        {
            lastTick = Time.time;
        }
    }

    public void Tick()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Where((x) => x.GetComponent<Knight>() && !x.GetComponent<Knight>().invincible).Count()==0)
        {
            if (waveNumber == bossWave)
            {
                Camera.main.GetComponent<CameraHandler>().ChangeMusic("Menu");

                GameObject.Find("Canvas/VictoryScreen").GetComponent<SubMenu>().Enter();
                isPaused = true;
                isVictorious = true;
            }
        }

        currentTick++;
        if (waveNumber > 0 && waveNumber!=bossWave)
        {
            movesRemaining--;
        }
        lastTick = Time.time;

        GameObject.FindWithTag("Player").GetComponent<Entity>().TickAttacks();
        GameObject.FindWithTag("Player").GetComponent<Entity>().TickMoves();
        GameObject.FindWithTag("Player").GetComponent<Player>().isMoving = false;
        GameObject.FindWithTag("Player").GetComponent<Player>().isAttacking = false;
        GameObject.FindWithTag("Player").GetComponent<Player>().actionPerformed = false;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().Reroll();
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().RefreshUI();

        
        foreach (GameObject ent in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            ent.GetComponent<Entity>().TickAttacks();
        }
        foreach (GameObject ent in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            ent.GetComponent<Entity>().TickMoves();
        }
        

        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Submerging"))
        {
            tile.GetComponent<Tile>().Submerge((float) ((float)waveLength - movesRemaining) / waveLength);
        }

        if (movesRemaining<=0  && !tutorial)
        {
            waveNumber++;

            movesRemaining = waveLength;
            if (waveNumber == bossWave)
            {
                movesRemaining *= 2;
            }
            SpawnEnemies();
            ShrinkMap();
            GameObject.Find("Canvas").GetComponent<UI>().DisplayWave(waveNumber);
        }
    }

    private void ShrinkMap()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Submerged"))
        {
            obj.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[247];
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (y > waveNumber - 1 && y < grid.GetLength(1) - waveNumber)
                {
                    if (x == grid.GetLength(0) - waveNumber - 1)
                    {
                        grid[x, y].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[248];
                        grid[x, y].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                        grid[x, y].obstructed = true;
                        grid[x, y].gameObject.tag = "Submerged";

                        if (y > waveNumber && y < grid.GetLength(1) - waveNumber - 1 && grid[x, y].Left())
                        {
                            grid[x, y].Left().gameObject.tag = "Submerging";
                        }
                    }
                    else if (x == waveNumber)
                    {
                        grid[x, y].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[248];
                        grid[x, y].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                        grid[x, y].gameObject.GetComponent<SpriteRenderer>().flipX = true;
                        grid[x, y].obstructed = true;
                        grid[x, y].gameObject.tag = "Submerged";

                        if (y > waveNumber && y < grid.GetLength(1) - waveNumber - 1 && grid[x, y].Right())
                        {
                            grid[x, y].Right().gameObject.tag = "Submerging";
                        }
                    }
                }
                if(x > waveNumber - 1 && x < grid.GetLength(0) - waveNumber) 
                { 
                    if (y == grid.GetLength(0) - waveNumber - 1)
                    {
                        grid[x, y].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[248];
                        grid[x, y].transform.Rotate(Vector3.forward * 90);
                        grid[x, y].obstructed = true;
                        grid[x, y].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                        grid[x, y].gameObject.tag = "Submerged";
                        if( x>waveNumber && x<grid.GetLength(0)-waveNumber-1 && grid[x, y].Bottom())
                        {
                            grid[x, y].Bottom().gameObject.tag = "Submerging";
                        }
                    }
                    else if (y == waveNumber)
                    {
                        grid[x, y].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[248];
                        grid[x, y].transform.Rotate(Vector3.forward * -90);
                        grid[x, y].obstructed = true;
                        grid[x, y].gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                        grid[x, y].gameObject.tag = "Submerged";

                        if (x > waveNumber && x < grid.GetLength(0) - waveNumber - 1 && grid[x, y].Top())
                        {
                            grid[x, y].Top().gameObject.tag = "Submerging";
                        }
                    }
                }
            }
        }
        grid[waveNumber, grid.GetLength(1) - waveNumber-1].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[247];
        grid[grid.GetLength(0) - waveNumber-1, waveNumber].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[247];
        grid[waveNumber, waveNumber].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[247];
        grid[grid.GetLength(0) - waveNumber -1, grid.GetLength(1) - waveNumber -1].GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Sprites/spritesheet")[247];

        foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Submerged"))
        {
            tile.GetComponent<Tile>().Submerge(0f);
        }

        foreach (Transform trans in GameObject.Find("Arena/Entities/").transform)
        {
            Entity ent = trans.gameObject.GetComponent<Entity>();
            if (ent.position.gameObject.tag == "Submerged")
            {
                if (!ent.stationary && ent.position.Left() && !ent.position.Left().obstructed && !ent.position.Left().contester)
                {
                    ent.Move(ent.position.Left());
                    ent.Move(ent.position);
                    ent.Transition(0f);
                }
                else if (!ent.stationary && ent.position.Right() && !ent.position.Right().obstructed && !ent.position.Right().contester)
                {
                    ent.Move(ent.position.Right());
                    ent.Move(ent.position);
                    ent.Transition(0f);
                }
                else if (!ent.stationary && ent.position.Top() && !ent.position.Top().obstructed && !ent.position.Top().contester)
                {
                    ent.Move(ent.position.Top());
                    ent.Move(ent.position);
                    ent.Transition(0f);
                }
                else if (!ent.stationary && ent.position.Bottom() && !ent.position.Bottom().obstructed && !ent.position.Bottom().contester)
                {
                    ent.Move(ent.position.Bottom());
                    ent.Move(ent.position);
                    ent.Transition(0f);
                }
                else
                {
                    ent.Die("Drown");
                }
            }
        }
    }

    private List<string> GetEnemiesToSpawn(int wave)
    {
        List<string> spawnTable = new List<string>();


        spawnTable.Add("Chest 1");
        spawnTable.Add("Chest 2");
        spawnTable.Add("Chest 2");

        if (Random.Range(0, 1f) > 0.5f)
        {
            spawnTable.Add("Chest 1");
        }
        if (Random.Range(0, 1f) > 0.5f)
        {
            spawnTable.Add("Chest 1");
        }

        switch (wave)
        {
            case 1:
                for (int i = 0; i < 20; i++)
                {
                    spawnTable.Add("Rat");
                }
                break;
            case 2:
                for (int i = 0; i < 5; i++)
                {
                    spawnTable.Add("Snake");
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    spawnTable.Add("Croc");
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    spawnTable.Add("Crab");
                }
                break;
            case 5:
                for (int i = 0; i < 1; i++)
                {
                    spawnTable.Add("Rat");
                }
                for (int i = 0; i < 1; i++)
                {
                    spawnTable.Add("Snake");
                }
                for (int i = 0; i < 1; i++)
                {
                    spawnTable.Add("Crab");
                }
                for (int i = 0; i < 1; i++)
                {
                    spawnTable.Add("Croc");
                }
                break;
            case 6:
                spawnTable.Add("Necromancer");
                break;
            case 7:
                spawnTable.Add("Necromancer");
                for (int i = 0; i < 10; i++)
                {
                    spawnTable.Add("Skeleton");
                }
                break;
            case 8:
                for (int i = 0; i < 10; i++)
                {
                    spawnTable.Add("Skeleton");
                }
                break;
            case 9:
                for (int i = 0; i < 2; i++)
                {
                    spawnTable.Add("Necromancer");
                }
                break;
            default:
                break;
        }
        return spawnTable;
    }

    public void StartingSpawn()
    {
        int x = 4;
        int y = 6;

        grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].contester = Instantiate(Resources.Load("Prefabs/Enemies/Chest 3") as GameObject, GameObject.Find("Arena/Entities").transform).GetComponent<Entity>();
        grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].contester.position = grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y];
        grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].contester.transform.position = grid[mapSize / 2 - 4 + x, mapSize / 2 - 4 + y].transform.position;
    }

    private void SpawnEnemies()
    {
        if (waveNumber == bossWave)
        {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy").Where((x) => x.GetComponent<Knight>()))
            {
                obj.GetComponent<Knight>().WakeUp();
            }
            GameObject.Find("Canvas").GetComponent<UI>().Prompt("The knights awake!");
            return;
        }


        int maxAttempts = 333;

        List<string> spawnTable = GetEnemiesToSpawn(waveNumber);

        int playerX = GameObject.FindWithTag("Player").GetComponent<Player>().position.x;
        int playerY = GameObject.FindWithTag("Player").GetComponent<Player>().position.y;

        foreach(string str in spawnTable)
        {
            int x = 0;
            int y = 0;
            int attempts = 0;
            do
            {
                x = (int)Random.Range(waveNumber, (float)grid.GetLength(0) - 1 - waveNumber);
                y = (int)Random.Range(waveNumber, (float)grid.GetLength(1) - 1 - waveNumber);
                attempts++;
            }
            while (!(attempts < maxAttempts && grid[x, y].contester == null && !grid[x, y].obstructed && (Mathf.Abs(playerX - x) > waveSpawnDistance || Mathf.Abs(playerY - y) > waveSpawnDistance)));

            if (grid[x,y].contester==null && !grid[x,y].obstructed && (Mathf.Abs(playerX-x)>waveSpawnDistance || Mathf.Abs(playerY - y) > waveSpawnDistance))
            {
                grid[x, y].contester = Instantiate(Resources.Load("Prefabs/Enemies/" + str) as GameObject, GameObject.Find("Arena/Entities").transform).GetComponent<Entity>();
                grid[x, y].contester.position = grid[x, y];
                grid[x, y].contester.transform.position = grid[x, y].transform.position;
            }
            else
            {
                break;
            }
        }
    }

    public void UpdateCounter()
    {
        
        GameObject.Find("Canvas/Enemy Counter").GetComponent<TextMeshProUGUI>().text = "Enemies: " + GameObject.FindGameObjectsWithTag("Enemy").Where((x)=>!x.GetComponent<Chest>() && !(x.GetComponent<Knight>() && x.GetComponent<Knight>().invincible)).Count();
        if (waveNumber != bossWave)
        {
            GameObject.Find("Canvas/Enemy Counter").GetComponent<TextMeshProUGUI>().text += "\nNext wave in: " + movesRemaining;
        }
        else
        {
            GameObject.Find("Canvas/Enemy Counter").GetComponent<TextMeshProUGUI>().text += "\n";
        }
    }

    public void UpdatePacer()
    {
        float progress = (Time.time-lastTick)/tickLength;
        float newAnchor = progress/2;
        GameObject.Find("Canvas/Pacer/Foreground/Left").GetComponent<RectTransform>().anchorMin = new Vector2(newAnchor, 0f);
        GameObject.Find("Canvas/Pacer/Foreground/Right").GetComponent<RectTransform>().anchorMax = new Vector2(1f-newAnchor, 1f);

        GameObject.Find("Canvas/Pacer/Foreground/Left").GetComponent<Image>().color = Color.Lerp(Color.green, Color.black, progress);
        GameObject.Find("Canvas/Pacer/Foreground/Right").GetComponent<Image>().color = Color.Lerp(Color.green, Color.black, progress);
    }

    public void SetPaused(bool state)
    {
        isPaused = state;
    }
    public bool IsPaused()
    {
        return isPaused;
    }
}
