using System.Collections;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ItemProbability
{
    public string itemName; // Nazwa prefabrykaty w folderze Resources
    public float probability; // Prawdopodobieñstwo wylosowania tego przedmiotu
}
public class BlockHit : MonoBehaviour
{
    public GameObject item;
    public Sprite emptyBlock;
    public int maxHits = -1;
    public float coinPropability = 0.5f;
    public float magicMushroomPropability = 0.1f;
    public float oneUpMushroomPropability = 0.1f;
    public float dinoPropability = 0.1f;
    public float bearPropability = 0.1f;
    public float koopaPropability = 0.1f;
    private bool animating;
    private bool isItemNull = false;
    private ItemProbability[] itemsWithProbabilities = new ItemProbability[6];

    void Start()
    {
        if(item == null) isItemNull = true;
        itemsWithProbabilities[0] = new ItemProbability { itemName = "BlockCoin", probability = coinPropability };
        itemsWithProbabilities[1] = new ItemProbability { itemName = "MagicMushroom", probability = magicMushroomPropability };
        itemsWithProbabilities[2] = new ItemProbability { itemName = "1upMushroom", probability = oneUpMushroomPropability };
        itemsWithProbabilities[3] = new ItemProbability { itemName = "DinoMystery", probability = dinoPropability };
        itemsWithProbabilities[4] = new ItemProbability { itemName = "bearMystery", probability = bearPropability };
        itemsWithProbabilities[5] = new ItemProbability { itemName = "KoopaMystery", probability = koopaPropability };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!animating && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                Hit(player);
            }
        }
    }

    private void Hit(Player player)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // show if hidden

        maxHits--;

        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }
        if (item == null && maxHits >= 0)
        {
            item = GetRandomItem();
        }
        if (item != null && maxHits >= 0)
        {
            Instantiate(item, transform.position, Quaternion.identity);
            if(isItemNull) item = null;
        }

        StartCoroutine(Animate(player));
    }

    private IEnumerator Animate(Player player)
    {
        animating = true;

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        animating = false;
        if (maxHits < 0 && player.isBig())
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }

    private GameObject GetRandomItem()
    {
        float totalProbability = 0f;
        foreach (var itemProbability in itemsWithProbabilities)
        {
            totalProbability += itemProbability.probability;
        }
        Debug.Log(totalProbability);
        float randomPoint = Random.value * totalProbability;
        Debug.Log(randomPoint);
        foreach (var itemProbability in itemsWithProbabilities)
        {
            if (randomPoint < itemProbability.probability)
            {
                return LoadPrefab(itemProbability.itemName);
            }
            else
            {
                randomPoint -= itemProbability.probability;
            }
        }

        return null; // W przypadku gdy coœ pójdzie nie tak
    }

    // Funkcja ³adowania prefabu z Resources
    private GameObject LoadPrefab(string itemName)
    {
        GameObject prefab = Resources.Load<GameObject>(itemName);
        if (prefab == null)
        {
            Debug.LogError("Prefab not found in Resources: " + itemName);
        }
        return prefab;
    }

}