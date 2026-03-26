using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Daadab
{
    public class TypedText : MonoBehaviour
    {
        private RectTransform rectTransform;
        private TMP_Text text;
        public RectTransform GetRectTransform() => rectTransform;
        public TMP_Text GetText() => text;

        private string[] substrings;
        private char[] fakeCharArray;
        private char[] realCharArray;
        private char[] pauseCharArray;
        private int noOfSubstrings;
        public int GetNumberOfSubstrings() => noOfSubstrings;

        private string fullString;
        private float charRevealTimeout = 0.03f;
        private int charRevealIndex;
        private bool isRevealingText;
        public bool IsRevealingText => isRevealingText;

        public static Action HasFinishedRevealingText;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            Assert.IsNotNull(text);

            rectTransform = GetComponent<RectTransform>();

            pauseCharArray = new char[2];
            pauseCharArray[0] = ',';
            pauseCharArray[1] = '.';
            pauseCharArray[1] = '!';
        }

        public void FinishRevealingText()
        {
            // if we're in the middle of revealing the text, just show it all immediately
            if (isRevealingText)
            {
                isRevealingText = false;
                StopAllCoroutines();
                UpdateText(fullString);
            }
        }

        public void UpdateText(string str, bool letterByLetter = false)
        {
            text.text = "";
            fullString = str;

            substrings = str.Split('^');
            noOfSubstrings = substrings.Length;

            if (letterByLetter)
            {
                var substringLength = substrings.Length;

                for (int i = 0; i < substrings.Length; i++)
                {
                    substringLength += substrings[i].Length;
                }

                fakeCharArray = new char[substringLength];
                realCharArray = new char[substringLength];
                var counter = 0;

                for (int i = 0; i < substrings.Length; i++)
                {
                    // fill up each fake substring with empty space
                    for (int j = 0; j < substrings[i].Length + 1; j++)
                    {
                        if (j == substrings[i].Length)
                        {
                            fakeCharArray[counter] = '\n';
                            realCharArray[counter] = '\n';
                        }
                        else
                        {
                            fakeCharArray[counter] = ' ';
                            realCharArray[counter] = substrings[i][j];
                        }

                        counter++;
                    }
                }

                StopAllCoroutines();
                StartCoroutine(RevealStringCO());
            }
            else
            {
                for (int i = 0; i < substrings.Length; i++)
                {
                    if (i > 0) text.text += "\n";

                    text.text += substrings[i];
                }
                isRevealingText = false;
                HasFinishedRevealingText?.Invoke();
            }
        }

        private IEnumerator RevealStringCO()
        {
            charRevealIndex = 0;
            isRevealingText = true;
            bool addPause = false;

            while (charRevealIndex < realCharArray.Length)
            {
                text.text = "";
                addPause = false;

                for (int i = 0; i < realCharArray.Length; i++)
                {
                    if (i >= charRevealIndex)
                    {
                        text.text += fakeCharArray[i];
                    }
                    else
                    {
                        text.text += realCharArray[i];

                        // check if last revealed letter should cause pause
                        if (i == charRevealIndex - 1)
                        {
                            for (int j = 0; j < pauseCharArray.Length; j++)
                            {
                                if (realCharArray[i] == pauseCharArray[j])
                                {
                                    addPause = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                charRevealIndex++;
                yield return new WaitForSeconds(charRevealTimeout + (addPause ? 0.5f : 0));
            }

            isRevealingText = false;
            HasFinishedRevealingText?.Invoke();
        }
    }
}