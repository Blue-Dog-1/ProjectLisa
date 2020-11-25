using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public Vector3 StarPosition;
    public List<ObjectsPrefab> Objects;
    public int Quantity;
    public int Level;

    
    public int DistansSpawn;
    float delay;
    public void Spawn(int quantity)
    {
        
        float indent = 0f;
        int Index = int.MaxValue;
        while (quantity >= 1)
        {
            var o = new GameObject();
            var bop = o.AddComponent<BakeObjectPrefab>();
            Index = RandomeIndex(Index, 0, Objects.Count);
            bop.Objects = Objects[Index];

            indent += bop.Objects.SizeObject.z;

            StarPosition.z = indent;
            StarPosition.x = Random.Range(-1, 1);

            o.transform.position = StarPosition;
            
            indent += bop.Objects.SizeObject.z / 2;


            quantity--;

            bop.Build();
        }

    }
    public void Start()
    {
        float speed = gameObject.GetComponent<MobailControl>().Speed;
        delay = 0.05f * (DistansSpawn / speed);
        //Spawn(Quantity);
        StartCoroutine(loop(Quantity));
    }

    int RandomeIndex(int oldindex, int min, int max)
    {
        var i = Random.Range(min, max);
        if (oldindex == i) return RandomeIndex(oldindex, min, max);
        else return i;
    }
    IEnumerator loop (int quantity)
    {
        float indent = 0f;
        int Index = int.MaxValue;
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
            if ((transform.position - StarPosition).magnitude > 40f) continue;
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, StarPosition);
    }
#endif

}
