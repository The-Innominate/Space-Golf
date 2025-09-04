using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine; // just to be explicit

public class MainMenuScript : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;   // MUST have GridLayoutGroup in Inspector (or we add it)
    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;    // MUST have GridLayoutGroup in Inspector (or we add it)

    public GameObject golfBallPrefab;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

    private void Start()
    {
        cameraTransform = Camera.main != null ? Camera.main.transform : null;

        //var levelGrid = EnsureGrid(levelButtonContainer, 6, new Vector2(160, 160), new Vector2(12, 12));
        //var shopGrid = EnsureGrid(shopButtonContainer, 6, new Vector2(120, 120), new Vector2(10, 10));

        // Optional during testing. If you want to keep editor-placed children, comment these out.
        ClearChildren(levelButtonContainer.transform);
        ClearChildren(shopButtonContainer.transform);

        //this is to move the container down by half the height of itself, that way button 1 starts top left
        //levelButtonContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -levelButtonContainer.GetComponent<RectTransform>().rect.height / 2);
        

        // LEVEL BUTTONS
        Sprite[] thumbnails = Resources.LoadAll<Sprite>("UpdatedLevelSprites");
        Debug.Log($"Loaded level thumbnails: {thumbnails.Length}");
        for (int i = 0; i < thumbnails.Length; i++)
        {
            var thumbnail = thumbnails[i];
            try
            {
                GameObject container = Instantiate(levelButtonPrefab);
                container.transform.SetParent(levelButtonContainer.transform, false);
                NormalizeChild(container);

                var img = container.GetComponent<Image>();
                if (img != null)
                {
                    img.type = Image.Type.Simple;
                    img.preserveAspect = true;
                    img.sprite = thumbnail;
                    img.enabled = true;
                }

                var shadow = container.GetComponent<Shadow>() ?? container.AddComponent<Shadow>();
                shadow.effectColor = new Color(0f, 0f, 0f, 0.35f);
                shadow.effectDistance = new Vector2(-6f, -6f);

                string sceneName = thumbnail.name;

                var btn = container.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName));
                }

                // High score should never crash the loop
                if (TryGetHighScore("Begin", sceneName, out int highScore))
                {
                    var scoreText = container.GetComponentInChildren<TextMeshProUGUI>(true);
                    if (scoreText != null)
                        scoreText.text = highScore <= 0 ? "No Score" : "Lowest Strokes: " + highScore.ToString();
                }

                Debug.Log($"Created level button {i}: {thumbnail.name}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Level button spawn failed for '{thumbnail?.name}' at index {i}: {ex}");
                continue;
            }
        }

        // SHOP BUTTONS
        Sprite[] textures = Resources.LoadAll<Sprite>("GolfBall");
        Debug.Log($"Loaded shop textures: {textures.Length}");
        for (int i = 0; i < textures.Length; i++)
        {
            var texture = textures[i];
            try
            {
                GameObject shopGO = Instantiate(shopButtonPrefab);
                shopGO.transform.SetParent(shopButtonContainer.transform, false);
                NormalizeChild(shopGO);

                var img = shopGO.GetComponent<Image>();
                if (img != null)
                {
                    img.type = Image.Type.Simple;
                    img.preserveAspect = true;
                    img.sprite = texture;
                    img.enabled = true;
                }

                var shadow = shopGO.GetComponent<Shadow>() ?? shopGO.AddComponent<Shadow>();
                shadow.effectColor = new Color(0f, 0f, 0f, 0.35f);
                shadow.effectDistance = new Vector2(-6f, -6f);

                shopGO.SetActive(true);

                string selectedName = texture.name;
                Sprite capturedTexture = texture;

                var btn = shopGO.GetComponent<Button>();
                if (btn != null)
                {
                    btn.interactable = true;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() =>
                    {
                        PlayerPrefs.SetString("SelectedBallSkin", selectedName);

                        // Make this tolerant. If not readable, we still save the name.
                        try
                        {
                            if (capturedTexture != null && capturedTexture.texture != null)
                            {
                                Color pixelColor = capturedTexture.texture.GetPixel(32, 32);
                                PlayerPrefs.SetFloat("SelectedBallSkinR", pixelColor.r);
                                PlayerPrefs.SetFloat("SelectedBallSkinG", pixelColor.g);
                                PlayerPrefs.SetFloat("SelectedBallSkinB", pixelColor.b);
                            }
                        }
                        catch (System.Exception pxEx)
                        {
                            Debug.LogWarning($"Pixel read failed for '{selectedName}': {pxEx.Message}");
                        }

                        PlayerPrefs.Save();
                        Debug.Log("Selected ball skin: " + selectedName);
                    });
                }

                Debug.Log($"Created shop button {i}: {texture.name}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Shop button spawn failed for '{texture?.name}' at index {i}: {ex}");
                continue;
            }
        }

        // MAKE SURE CONTAINERS ARE PROPERLY SIZED AND POSITIONED
        //FixContainerSizing(levelButtonContainer, levelGrid);
        //FixContainerSizing(shopButtonContainer, shopGrid);

        ResetAnchored(levelButtonContainer);
        ResetAnchored(shopButtonContainer);

        //Hardcoded 'starting position' of the level button container - this will just move the container down by -120 on the Y axis
        levelButtonContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);
        shopButtonContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);

        ForceRebuild(levelButtonContainer);
        ForceRebuild(shopButtonContainer);

        Time.timeScale = 1;

        Debug.Log("MainMenu setup complete!");
    }

    // NEW: Fix container sizing so buttons actually show up
    private static void FixContainerSizing(GameObject container, GridLayoutGroup grid)
    {
        var rt = container.GetComponent<RectTransform>();
        if (rt == null) return;

        // Calculate how much space the grid needs
        int rows = Mathf.CeilToInt((float)container.transform.childCount / grid.constraintCount);
        float totalWidth = (grid.cellSize.x * grid.constraintCount) + (grid.spacing.x * (grid.constraintCount - 1));
        float totalHeight = (grid.cellSize.y * rows) + (grid.spacing.y * (rows - 1));

        // Add padding
        totalWidth += grid.padding.left + grid.padding.right;
        totalHeight += grid.padding.top + grid.padding.bottom;

        // Set the container size
        rt.sizeDelta = new Vector2(totalWidth, totalHeight);

        // Make sure it's positioned properly
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Debug.Log($"Fixed sizing for {container.name}: {totalWidth}x{totalHeight} (children: {container.transform.childCount})");
    }

    // Safe high-score getter that never kills the loop
    private static bool TryGetHighScore(string category, string sceneName, out int score)
    {
        score = 0;
        try
        {
            var inst = LevelHighScores.Instance;
            if (inst == null)
            {
                Debug.LogWarning("LevelHighScores.Instance is null");
                return false;
            }

            score = inst.LoadFromJson(category, sceneName);
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"LoadFromJson failed for '{sceneName}': {ex.Message}");
            return false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (cameraDesiredLookAt != null && cameraTransform != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(
                cameraTransform.rotation,
                cameraDesiredLookAt.rotation,
                3 * Time.deltaTime
            );
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredLookAt = menuTransform;
    }

    private void ChangePlayerSkin()
    {
        golfBallPrefab = Resources.Load<GameObject>("GolfBall/" + golfBallPrefab.name);
    }

    // ---------- Helpers ----------

    private static GridLayoutGroup EnsureGrid(GameObject container, int columns, Vector2 defaultCell, Vector2 spacing)
    {
        var grid = container.GetComponent<GridLayoutGroup>();
        if (grid == null) grid = container.AddComponent<GridLayoutGroup>();

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        if (grid.cellSize == Vector2.zero) grid.cellSize = defaultCell; // keep existing if already set in Inspector
        grid.spacing = spacing;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperLeft;
        grid.padding = new RectOffset(10, 10, 10, 10); // Smaller padding

        Debug.Log($"Grid setup for {container.name}: {columns} columns, cell size {defaultCell}");
        return grid;
    }

    private static void NormalizeChild(GameObject child)
    {
        var rt = child.GetComponent<RectTransform>();
        if (rt != null)
        {
            // Let the Grid control size; we only ensure sane anchors/scale
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.localScale = Vector3.one;
            rt.anchoredPosition3D = Vector3.zero;
        }
    }

    private static void ResetAnchored(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        if (rt != null) rt.anchoredPosition = Vector2.zero;
    }

    private static void ForceRebuild(GameObject go)
    {
        // Reliable way to force layout to rebuild so all children show immediately in ScrollRect
        Canvas.ForceUpdateCanvases();
        var rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }
        Canvas.ForceUpdateCanvases();
    }

    private static void ClearChildren(Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
            Object.Destroy(t.GetChild(i).gameObject);
    }
}