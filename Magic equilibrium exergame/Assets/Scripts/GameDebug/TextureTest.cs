using Assets.Scripts.Models.Path.Generation.Line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameDebug
{
    public class TextureTest : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _sprite;


        private void Start()
        {
            var texture = TextureUtils.GetBorderAsTexture(_sprite.texture);
            _image.sprite = Sprite.Create(texture, _image.sprite.rect, _image.sprite.pivot);
        }   
    }
}
