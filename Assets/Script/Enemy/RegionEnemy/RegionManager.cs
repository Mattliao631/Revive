using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected float teleportTime = 1f;
    [SerializeField] protected bool canRespawn = false;
    
    public void respawnMonster(RegionEnemy monster) {
        if (canRespawn) {
            Coroutine wait = StartCoroutine(waitForRespawn(monster));
        }
    }

    private IEnumerator waitForRespawn(RegionEnemy monster) {
        yield return new WaitForSeconds(monster.respawnTime);
        monster.respawn();
    }

    public void recallMonster(RegionEnemy monster) {
        monster.gameObject.SetActive(false);
        Coroutine teleport = StartCoroutine(teleportMonster(monster));
    }

    private IEnumerator teleportMonster(RegionEnemy monster) {

        GameObject soul = monster.soul;
        soul.transform.position = monster.gameObject.transform.position;
        soul.GetComponent<SpriteRenderer>().enabled = true;
        Vector3 startPosition = monster.transform.position;
        Vector3 endPosition = monster.respawnPoint.position;

        float startX = startPosition.x;
        float startY = startPosition.y;
        float endX = endPosition.x;
        float endY = endPosition.y;

        float elapsedTime = 0;

        while (elapsedTime < teleportTime) {
            elapsedTime += Time.deltaTime;
            
            float x = Mathf.Lerp(startX, endX, elapsedTime / teleportTime);
            float y = Mathf.Lerp(startY, endY, elapsedTime / teleportTime);
            soul.transform.position = new Vector3(x, y, 0);
            yield return null;
        }

        soul.GetComponent<SpriteRenderer>().enabled = false;
        monster.respawn();
        monster.outOfRegion = false;
    }
    
}
