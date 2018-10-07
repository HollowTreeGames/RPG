/*

The MIT License (MIT)

Copyright (c) 2015-2017 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;
using MyDialogue;

namespace Yarn.Unity {
    /// Displays dialogue lines to the player, and sends
    /// user choices back to the dialogue system.

    /** Note that this is just one way of presenting the
     * dialogue to the user. The only hard requirement
     * is that you provide the RunLine, RunOptions, RunCommand
     * and DialogueComplete coroutines; what they do is up to you.
     */
    public class DialogueUI : Yarn.Unity.DialogueUIBehaviour
    {

        /// The object that contains the dialogue and the options.
        /** This object will be enabled when conversation starts, and 
         * disabled when it ends.
         */
        public Canvas dialogueCanvas;

        /// The UI element that displays lines
        public Text lineText;

        /// The UI elements that display NPC name and portrait
        public Text nameText;
        public GameObject portraitPanel;
        private Image portraitImage;

        /// A UI element that appears after lines have finished appearing
        public GameObject continuePrompt;

        /// A delegate (ie a function-stored-in-a-variable) that
        /// we call to tell the dialogue system about what option
        /// the user selected
        private Yarn.OptionChooser SetSelectedOption;

        /// The buttons that let the user choose an option
        public List<Button> optionButtons;

        /// Make it possible to temporarily disable the controls when
        /// dialogue is active and to restore them when dialogue ends
        public RectTransform gameControlsContainer;

        public bool talkButtonPressed = false;

        private static bool instanceExists = false;

        private string last_text;

        private void Start()
        {
            if (instanceExists)
            {
                Destroy(gameObject);
                return;
            }

            instanceExists = true;
            DontDestroyOnLoad(transform.gameObject);

            portraitImage = portraitPanel.GetComponent<Image>();
        }

        void Awake ()
        {
            // Start by hiding the container, line and option buttons
            HideCanvas();

            lineText.gameObject.SetActive (false);

            foreach (var button in optionButtons) {
                button.gameObject.SetActive (false);
            }

            // Hide the continue prompt if it exists
            if (continuePrompt != null)
                continuePrompt.SetActive (false);
        }

        public void Update()
        {
            if (!cinematic_mode && Input.GetButtonDown("Jump"))
                talkButtonPressed = true;
        }

        /// Show a line of dialogue, gradually
        public override IEnumerator RunLine(Yarn.Line line)
        {
            // Convert line text to DLine
            DLine dLine = DLine.FromYarnLine(line);

            if (cinematic_mode)
            {
                dLine.pause = false;
                // Set default wait time if not defined
                if (dLine.wait <= 0)
                    dLine.wait = 1;
            }

            nameText.text = dLine.name;
            portraitImage.sprite = dLine.GetFace();

            // Show the text
            if (dLine.clear_text)
            {
                lineText.text = "";
            }
            else
            {
                last_text = lineText.text;
            }

            lineText.gameObject.SetActive(true);

            talkButtonPressed = false;

            ShowCanvas();

            if (dLine.speed > 0.0f)
            {
                // Display the line one character at a time
                var stringBuilder = new StringBuilder();

                foreach (char c in dLine.text)
                {
                    // Detect keypress to skip text animation
                    if (talkButtonPressed)
                    {
                        break;
                    }
                    stringBuilder.Append(c);
                    lineText.text = stringBuilder.ToString();
                    yield return new WaitForSeconds(dLine.speed);
                }
            }
            // Display the line immediately if textSpeed == 0
            if (dLine.clear_text)
            {
                lineText.text = dLine.text;
            }
            else
            {
                lineText.text = last_text + dLine.text;
            }

            if (dLine.pause)
            {
                talkButtonPressed = false;

                // Delay before showing continue prompt
                for (int i=0; i < 10; i++)
                {
                    if (talkButtonPressed == true)
                        break;
                    yield return new WaitForSeconds(0.1f);
                }

                // Show the 'press any key' prompt when done, if we have one
                if (!talkButtonPressed && continuePrompt != null)
                    continuePrompt.SetActive(true);

                // Wait for any user input
                while (!talkButtonPressed)
                {
                    yield return null;
                }

                if (continuePrompt != null)
                    continuePrompt.SetActive(false);
            }
            else
            {
                if (dLine.wait > 0)
                {
                    yield return new WaitForSeconds(dLine.wait);
                }
            }

            // Hide the text and prompt
            lineText.gameObject.SetActive(false);
        }

        /// Show a list of options, and wait for the player to make a selection.
        public override IEnumerator RunOptions(Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser)
        {
            yield break;
        }

        /// Called by buttons to make a selection.
        //public void SetOption (int selectedOption)
        //{

        //    // Call the delegate to tell the dialogue system that we've
        //    // selected an option.
        //    SetSelectedOption (selectedOption);

        //    // Now remove the delegate so that the loop in RunOptions will exit
        //    SetSelectedOption = null; 
        //}

        /// Run an internal command.
        public override IEnumerator RunCommand(Yarn.Command command)
        {
            Debug.Log("Command: " + command.text);
            // "Perform" the command
            var words = command.text.Split(' ');
            var commandText = words[0].ToLower();
            switch (commandText)
            {
                case "wait":
                    float fWait;
                    try
                    {
                        fWait = float.Parse(words[1]);
                    }
                    catch (System.FormatException)
                    {
                        Debug.LogErrorFormat("wait <num> must contain an integer ({0})", command.text);
                        break;
                    }
                    yield return new WaitForSeconds(fWait);
                    break;
                case "show":
                    ShowCanvas();
                    break;
                case "hide":
                    HideCanvas();
                    break;
                default:
                    Debug.LogError(Utils.Join("Unrecognized command:", commandText));
                    break;
            }

            yield break;
        }

        /// Called when the dialogue system has started running.
        public override IEnumerator DialogueStarted()
        {
            Debug.Log("Dialogue starting!");

            // Disable cinematic mode by default
            cinematic_mode = false;

            // Enable the dialogue controls.
            //ShowCanvas();

            // Hide the game controls.
            if (gameControlsContainer != null)
            {
                gameControlsContainer.gameObject.SetActive(false);
            }

            yield break;
        }

        private void ShowCanvas()
        {
            if (dialogueCanvas != null)
                dialogueCanvas.enabled = true;
        }

        /// Called when the dialogue system has finished running.
        public override IEnumerator DialogueComplete()
        {
            Debug.Log("Complete!");

            // Hide the dialogue interface.
            HideCanvas();

            // Show the game controls.
            if (gameControlsContainer != null)
            {
                gameControlsContainer.gameObject.SetActive(true);
            }

            yield break;
        }

        private void HideCanvas()
        {
            if (dialogueCanvas != null)
                dialogueCanvas.enabled = false;
        }

    }

}
