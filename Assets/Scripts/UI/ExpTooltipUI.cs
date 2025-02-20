using MyRPG.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpTooltipUI : MonoBehaviour
{
    #region Variables
    public PlayerParams playerParams;
    public GameObject tooltip; // 툴팁 UI
    public TextMeshProUGUI tooltipText; // EXP 표시 텍스트
    public RectTransform tooltipBackground; // 툴팁 배경 (Image)

    private RectTransform tooltipRect; // 툴팁의 RectTransform
    private bool isHovering = false; // 마우스가 EXP 바 위에 있는지 여부
    #endregion

    private void Start()
    {
        tooltip.SetActive(false); // 시작 시 툴팁 숨기기
        tooltipRect = tooltip.GetComponent<RectTransform>(); // 툴팁의 RectTransform 가져오기
    }

    private void Update()
    {
        if (isHovering)
        {
            // ✅ 마우스 위치를 따라 툴팁 이동
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltip.transform.parent as RectTransform,
                Input.mousePosition,
                null,
                out mousePos
            );

            tooltipRect.anchoredPosition = mousePos + new Vector2(10f, -10f); // 마우스 위치 조정
        }
    }

    // ✅ 마우스를 EXP 바 위에 올리면 툴팁 표시
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        tooltip.SetActive(true);
        UpdateTooltip();
    }

    // ✅ 마우스를 벗어나면 툴팁 숨기기
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        tooltip.SetActive(false);
    }

    // ✅ 툴팁 텍스트 업데이트 및 배경 크기 조정
    private void UpdateTooltip()
    {
        tooltipText.text = playerParams.stat.exp + "/" + playerParams.stat.expToNextLevel; 

        // 텍스트 길이에 맞춰 배경 크기 자동 조정
        Vector2 textSize = new Vector2(tooltipText.preferredWidth + 20f, tooltipText.preferredHeight + 10f);
        tooltipBackground.sizeDelta = textSize;
    }
}
