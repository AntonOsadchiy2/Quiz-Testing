using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AnimationManager //класс анимаций Dotween
{
    public static void SquareSpawnBounce(GameObject Square)
    {
        if (Square != null)
        {
            Square.transform
                .DOScale(1f, 2f)
                .SetEase(Ease.InBounce);
        }
    }
    public static void WrongAnswerBounce(GameObject Square)
    {
        Vector3 Scale = Square.transform.localScale;
        Square.transform
             .DOScale(0.7f, 0.4f)
             .SetEase(Ease.OutElastic)
         .OnComplete(() =>
         {
             Square.transform.DOScale(Scale, 0.4f)
             .SetEase(Ease.OutElastic);
         });
        Square.transform
           .DOShakePosition(3);
    }

    public static void TextAppear(GameObject text)
    {
        text.GetComponent<Text>()
            .DOFade(1f, 2f);
    }
    public static void FadeIn(GameObject screen)
    {
        screen.GetComponent<Image>()
            .DOFade(0.9f, 3f)
            .SetEase(Ease.OutCubic);
    }
    public static void Restart(GameObject obj)
    {
        if (obj.GetComponent<Text>())
        {
            var text = obj.GetComponent<Text>();
            text
                .DOFade(1f, 2f)
            .OnComplete(() =>
            {
                text
                .DOFade(0f, 2f);
            });
        }
        if (obj.GetComponent<Image>())
        {
            var image = obj.GetComponent<Image>();
            image
                .DOFade(1f, 2f)
            .OnComplete(() =>
            {
                image
                .DOFade(0f, 2f);
            });
        }
    }
}
