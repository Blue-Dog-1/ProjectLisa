using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField]
    Vector3 StarPosition;
    [SerializeField]
    List<ObjectsPrefab> Objects;
    [SerializeField]
    int Quantity;
    public int Level;
    [SerializeField] int DistanceToNextSpawn;
    public int DistansSpawn;
    float delay;

    [Space]
    [Header("Ground")]
    [SerializeField] GameObject groundCarent;

    public void Start()
    {
        delay = 0.05f * (DistansSpawn / MobailControl.Speed);
        StartCoroutine(loop(Quantity));
        Events.BrakeSpawn += delegate () { StopAllCoroutines();  };
    }

    int RandomeIndex(int oldindex, int min, int max)
    {
        int i = Random.Range(min, max);
        if (oldindex == i) return RandomeIndex(oldindex, min, max);
        else return i;
    }
    IEnumerator loop (int quantity)
    {
        float indent = 0f;
        int Index = int.MaxValue;

        Vector3 position = groundCarent.transform.position; 
        while (quantity >= 1)
        {
            var o = new GameObject();
            var bop = o.AddComponent<BakeObjectPrefab>();
            Index = RandomeIndex(Index, 0, Objects.Count);
            bop.Objects = Objects[Index];
            indent += bop.Objects.indent;
            
            StarPosition.z = indent;
            StarPosition.x = Random.Range(-1, 1);
            o.transform.position = StarPosition;

            indent += bop.Objects.indent;

            quantity--;

            bop.Build();
        }
        yield return new WaitForSeconds(delay * quantity);
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            #region Ground generation
            if ((position.z - transform.position.z) < 250f)
            {
                position = groundCarent.transform.position;
                position.z += groundCarent.transform.localScale.y;

                var newgroundCarent = Instantiate(groundCarent, position, groundCarent.transform.rotation);
                groundCarent = newgroundCarent;
            }
            #endregion


            if ((transform.position - StarPosition).magnitude > DistanceToNextSpawn) continue;
            var o = new GameObject();
            var bop = o.AddComponent<BakeObjectPrefab>();
            Index = RandomeIndex(Index, 0, Objects.Count);

            bop.Objects = Objects[Index];

            indent += bop.Objects.indent;
            
            StarPosition.z = indent;
            StarPosition.x = Random.Range(-1, 1);

            indent += bop.Objects.indent;

            o.transform.position = StarPosition;

            quantity--;
            bop.Build();


           
        }
    }

    private void Update()
    {
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, StarPosition);
        Gizmos.DrawLine(transform.position, groundCarent.transform.position);
    }
#endif

}
