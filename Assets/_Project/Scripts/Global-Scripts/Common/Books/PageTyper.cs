using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DaftAppleGames.Common.Books
{
    public class PageTyper : MonoBehaviour
    {
        [Header("Typer Settings")]
        public PauseInfo pauseInfo;

        private int index;
        private TextMeshProUGUI _tmpText;
        private string _textToType;
        private string _currentText = string.Empty;

        private void Start()
        {
            _tmpText= GetComponent<TextMeshProUGUI>();
            _textToType = _tmpText.text;
            _tmpText.text = string.Empty;
            ReproduceText();
        }

        private void ReproduceText()
        {
            //if not readied all letters
            if (index < _textToType.Length)
            {
                //get one letter
                char letter = _textToType[index];

                //Actualize on screen
                _tmpText.text = Write(letter);

                //set to go to the next
                index += 1;
                StartCoroutine(PauseBetweenChars(letter));
            }
        }

        private string Write(char letter)
        {
            _currentText += letter;
            return _currentText;
        }

        private IEnumerator PauseBetweenChars(char letter)
        {
            switch (letter)
            {
                case '.':
                    yield return new WaitForSeconds(pauseInfo.dotPause);
                    ReproduceText();
                    yield break;
                case ',':
                    yield return new WaitForSeconds(pauseInfo.commaPause);
                    ReproduceText();
                    yield break;
                case ' ':
                    yield return new WaitForSeconds(pauseInfo.spacePause);
                    ReproduceText();
                    yield break;
                default:
                    yield return new WaitForSeconds(pauseInfo.normalPause);
                    ReproduceText();
                    yield break;
            }
        }
    }

    [Serializable]
    public class PauseInfo
    {
        public float dotPause;
        public float commaPause;
        public float spacePause;
        public float normalPause;
    }
}