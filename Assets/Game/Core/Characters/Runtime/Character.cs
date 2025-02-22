using DG.Tweening;
using System;
using UnityEngine;

namespace Core.Entities
{
    public enum CharacterType
    {
        Demon, 
        Human
    }

    public class Character : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CharacterType _characterType;

        private Sequence _moveSequence;

        public void Initialize()
        {
            transform.position = new Vector3(-15, transform.position.y, transform.position.z);
        }

        public Character SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;

            return this;
        }

        public void SetType(CharacterType type)
        {
            _characterType = type;
        }

        public void InviteInRoom(Action callback)
        {
            transform.position = new Vector3(-15, transform.position.y, transform.position.z);
            MoveOnPosX(0, callback);
        }

        public void Approved(Action callback)
        {
            MoveOnPosX(15, callback);
        }

        public void Denied(Action callback)
        {
            MoveOnPosX(-15, callback);
        }

        private void MoveOnPosX(float pos, Action callback)
        {
            _moveSequence?.Kill();
            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(transform.DOMoveX(pos, 1f)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                }));
        }
    }
}
