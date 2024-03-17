using UnityEngine;
using UnityEngine.UI;

public class ScrollViewTest : MonoBehaviour
{
    public GameObject root;

    // Start is called before the first frame update
    void Start()
    {
        CreateScrollView(root);
    }

    public void CreateScrollView(GameObject targetUi)
    {
        Vector2 anchorSetting = new Vector2(0.5f, 0.5f);
        Vector3 scale = new Vector3(1, 1, 1);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        // Scroll view
        GameObject scrollViewParent = new GameObject("Scroll View");
        scrollViewParent.transform.SetParent(targetUi.transform);
        RectTransform scrollViewRectTransform = scrollViewParent.AddComponent<RectTransform>();
        scrollViewRectTransform.localRotation = rotation;
        scrollViewRectTransform.localScale = scale;
        scrollViewRectTransform.sizeDelta = new Vector2(360, 200);
        scrollViewRectTransform.localPosition = new Vector3(-145, -50, 0);

        // Viewport
        GameObject viewPort = new GameObject("View Port");
        viewPort.transform.SetParent(scrollViewParent.transform);
        RectTransform viewPortRectTransform = viewPort.AddComponent<RectTransform>();
        viewPortRectTransform.localScale = scale;
        viewPortRectTransform.localRotation = rotation;
        viewPortRectTransform.localPosition = new Vector3(-20, 0, 0);

        // Viewport Content
        GameObject content = new GameObject("Content");
        content.transform.SetParent(viewPort.transform);
        RectTransform contentRectTransform = content.AddComponent<RectTransform>();
        contentRectTransform.localScale = scale;
        contentRectTransform.localRotation = rotation;
        contentRectTransform.localPosition = new Vector3(0, 0, 0);

        // Scrollbar
        GameObject scrollBar = new GameObject("Scrollbar");
        scrollBar.transform.SetParent(scrollViewParent.transform);
        RectTransform scrollbarAreaRectTransform = scrollBar.AddComponent<RectTransform>();
        scrollbarAreaRectTransform.localScale = scale;
        scrollbarAreaRectTransform.localRotation = rotation;
        scrollbarAreaRectTransform.pivot = new Vector2(1, 1);
        scrollbarAreaRectTransform.sizeDelta = new Vector2(20, 200); ;
        scrollbarAreaRectTransform.localPosition = new Vector3(180, 100, 0);

        // Scroll Sliding area
        GameObject slidingArea = new GameObject("Sliding Area");
        slidingArea.transform.SetParent(scrollBar.transform);
        RectTransform slidingAreaRectTransform = slidingArea.AddComponent<RectTransform>();
        slidingAreaRectTransform.localScale = scale;
        slidingAreaRectTransform.localRotation = rotation;
        slidingAreaRectTransform.sizeDelta = new Vector2(10, 200);
        slidingAreaRectTransform.anchoredPosition = new Vector3(0, 0, 0);

        // Scroll handle
        GameObject handle = new GameObject("Handle");
        RectTransform handleRectTransform = handle.AddComponent<RectTransform>();
        handle.transform.SetParent(slidingArea.transform);
        handleRectTransform.localScale = scale;
        handleRectTransform.localRotation = rotation;
        SetLeft(handleRectTransform, -5);
        SetTop(handleRectTransform, 0);
        SetRight(handleRectTransform, -5);
        SetBottom(handleRectTransform, 0);

        // Scroll handle
        Image scrollHandleImage = handle.AddComponent<Image>();
        scrollHandleImage.color = Color.red;

        // Scrollbar
        Image scrollbarImage = scrollBar.AddComponent<Image>();
        scrollbarImage.color = Color.yellow;
        Scrollbar vertScrollBar = scrollBar.AddComponent<Scrollbar>();
        vertScrollBar.targetGraphic = scrollHandleImage;
        vertScrollBar.handleRect = handleRectTransform;
        vertScrollBar.direction = Scrollbar.Direction.BottomToTop;

        // Viewport
        Image viewPortImage = viewPort.AddComponent<Image>();
        viewPortImage.color = Color.blue;
        Mask viewPortMask = viewPort.AddComponent<Mask>();

        // Viewport Content
        VerticalLayoutGroup verticalLayoutGroup = content.AddComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.childControlWidth = false;
        verticalLayoutGroup.childControlHeight = false;
        verticalLayoutGroup.spacing = 10.0f;
        ContentSizeFitter sizeFitter = content.AddComponent<ContentSizeFitter>();
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Scrollview
        Image scrollViewImage = scrollViewParent.AddComponent<Image>();
        scrollViewImage.color = Color.white;
        ScrollRect scrollRect = scrollViewParent.AddComponent<ScrollRect>();
        scrollRect.content = contentRectTransform;
        scrollRect.viewport = viewPortRectTransform;
        scrollRect.verticalScrollbar = vertScrollBar;
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
    }

    public static void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}