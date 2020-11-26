using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class TerrainDeform : MonoBehaviour
{

    public Texture2D BrashHeights;
    public Texture2D BrashColor;
    public ComputeShader shader;
    public Vector3[] tex;
    [Space]
    public Terrain terr;

    int hmWidth; // heightmap width
    int hmHeight; // heightmap height

    int posXInTerrain; // position of the game object in terrain width (x axis)
    int posYInTerrain; // position of the game object in terrain height (z axis)
    
    [Range(10, 100)]
    public int size = 20; // the diameter of terrain portion that will raise under the game object
    public float force;
    public float depth;


    float t;
    private float[,] originalHeights;
    private float[,,] originalMap;

    // amount of absorbed mass
    [HideInInspector] public float AOAM;
    float t2; // 40 is constant max absorbed mass

    public ControlVFX vfx;
    public ParticleSystem Dust;
    public GameObject originalStone;
    

    void Awake()
    {
        t2 = 1f / 40f;
        hmWidth = terr.terrainData.heightmapResolution;
        hmHeight = terr.terrainData.heightmapResolution;

        originalHeights = terr.terrainData.GetHeights(
                 0, 0, terr.terrainData.heightmapResolution, terr.terrainData.heightmapResolution);
        originalMap = terr.terrainData.GetAlphamaps(0, 0, terr.terrainData.alphamapWidth, terr.terrainData.alphamapHeight);
        t = 1f / size;
    }
    
    private void Start()
    {
        StartCoroutine(loop());
        RunShader();

    }
    void RunShader()
    {
        int kernelHandle = shader.FindKernel("CSMain");

        

    }

    IEnumerator loop()
    {
        Vector3 tempCoord;
        Vector3 coord;

        int offset;

        float[,] heights;
        float[,,] maps;

        float buferHeigth;
        float buferAlphaMap;

        

        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            // get the normalized position of this game object relative to the terrain
            tempCoord = (transform.position - terr.gameObject.transform.position);
            
            coord.x = tempCoord.x / terr.terrainData.size.x;
            coord.y = tempCoord.y / terr.terrainData.size.y;
            coord.z = tempCoord.z / terr.terrainData.size.z;

            // get the position of the terrain heightmap where this game object is
            posXInTerrain = (int)(coord.x * hmWidth);
            posYInTerrain = (int)(coord.z * hmHeight);

            // we set an offset so that all the raising terrain is under this game object
            offset = size / 2;

            // get the heights of the terrain under this game object
            heights = terr.terrainData.GetHeights(posXInTerrain - offset, posYInTerrain - offset, size, size);

            maps = terr.terrainData.GetAlphamaps(posXInTerrain - offset, posYInTerrain - offset, size, size);

            // we set each sample of the terrain in the size to the desired height
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                {
                    buferHeigth = ((BrashHeights.GetPixelBilinear(t * x, t * y).r) + depth) * force;
                    if (buferHeigth < heights[x, y])
                    {
                        AOAM += heights[x, y] - buferHeigth;
                        heights[x, y] = buferHeigth;
                    }
                    //heights[x, y] = (buferHeigth < heights[x, y]) ? buferHeigth : heights[x, y];

                    buferAlphaMap = 1 - (BrashColor.GetPixelBilinear(t * x, t * y).r);
                    maps[x, y, 1] = (buferAlphaMap > maps[x, y, 1]) ? buferAlphaMap : maps[x, y, 1];
                    maps[x, y, 0] = 1 - maps[x, y, 1] * 1.2f; // invers maps[x, y, 1]
                }

            // set the new height
            terr.terrainData.SetHeights(posXInTerrain - offset, posYInTerrain - offset, heights);
            terr.terrainData.SetAlphamaps(posXInTerrain - offset, posYInTerrain - offset, maps);

            AOAM = t2 * Mathf.Round(AOAM);
            vfx.dust(AOAM);
            
            /*
            Vector3 pos;
            GameObject stone;
            while (AOAM > 0) 
            {
                pos.x = transform.position.x + Random.Range(-1.5f, 1.5f);
                pos.y = transform.position.y - 2f;
                pos.z = transform.position.z + Random.Range(-1.5f, 1.5f);

                stone = Instantiate(originalStone, pos, Quaternion.identity);
                stone.transform.localScale *= Random.Range(0.8f, 2f);
                stone.SetActive(true);
                AOAM -= 0.5f;
            }*/
            
            Dust.emissionRate = (AOAM + 0.1f) * 10;

            AOAM = 0f;

        }
    }

   
   

    void OnApplicationQuit()
    {
        terr.terrainData.SetHeights(0, 0, originalHeights);
        terr.terrainData.SetAlphamaps(0, 0, originalMap);
    }

}
