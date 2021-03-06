﻿/*

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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CsvHelper;

namespace Yarn.Unity
{

    [System.Serializable]
    public class LocalisedStringGroup {
        public SystemLanguage language;
        public TextAsset[] stringFiles;
    }

    /// DialogueRunners act as the interface between your game and YarnSpinner.
    /** Make our menu item slightly nicer looking */
    [AddComponentMenu("Scripts/Yarn Spinner/Dialogue Runner")]
    public class DialogueRunner : MonoBehaviour
    {
        /// The JSON files to load the conversation from
        public TextAsset[] sourceText;

        /// The group of JSON files to be used for this language
        public LocalisedStringGroup[] stringGroups;

        /// Language debugging options
        public bool shouldOverrideLanguage = false;

        public SystemLanguage overrideLanguage = SystemLanguage.English;

        /// Our variable storage
        public Yarn.Unity.VariableStorageBehaviour variableStorage;

        /// The object that will handle the actual display and user input
        public Yarn.Unity.DialogueUIBehaviour dialogueUI;

        /// Which node to start from
        public string startNode = Yarn.Dialogue.DEFAULT_START;

        /// Whether we should start dialogue when the scene starts
        public bool startAutomatically = false;

        /// Tests to see if the dialogue is running
        public bool isDialogueRunning { get; private set; }

        public bool automaticCommands = true;

        /// Our conversation engine
        /** Automatically created on first access
         */
        private Dialogue _dialogue;
        public Dialogue dialogue {
            get {
                if (_dialogue == null) {
                    // Create the main Dialogue runner, and pass our variableStorage to it
                    _dialogue = new Yarn.Dialogue (variableStorage);

                    // Set up the logging system.
                    _dialogue.LogDebugMessage = delegate (string message) {
                        Debug.Log (message);
                    };
                    _dialogue.LogErrorMessage = delegate (string message) {
                        Debug.LogError (message);
                    };
                }
                return _dialogue;
            }
        }

        private QuestManager questManager;
        private GameState gameState;
        private InventoryManager inventoryManager;
        private SceneLoader sceneLoader;

        private System.Random random;

        /// Start the dialogue
        void Start ()
        {
            // Ensure that we have our Implementation object
            if (dialogueUI == null) {
                Debug.LogError ("Implementation was not set! Can't run the dialogue!");
                return;
            }

            // And that we have our variable storage object
            if (variableStorage == null) {
                Debug.LogError ("Variable storage was not set! Can't run the dialogue!");
                return;
            }

            // And that we have our other objects
            gameState = FindObjectOfType<GameState>();
            questManager = FindObjectOfType<QuestManager>();
            inventoryManager = FindObjectOfType<InventoryManager>();
            sceneLoader = FindObjectOfType<SceneLoader>();

            // Set additional functions at runtime
            AddCustomFunctions();

            // Seed RNG
            random = new System.Random();

            // Ensure that the variable storage has the right stuff in it
            variableStorage.ResetToDefaults ();

            // Load all scripts
            if (sourceText != null) {
                foreach (var source in sourceText) {
                    // load and compile the text
                    dialogue.LoadString (source.text, source.name);
                }
            }

            if (startAutomatically) {
                StartDialogue();
            }

            if (stringGroups != null) {
                // Load the string table for this language, if appropriate
                var stringsGroup = new List<LocalisedStringGroup>(stringGroups).Find(
                    entry => entry.language == (shouldOverrideLanguage ? overrideLanguage : Application.systemLanguage)
                );

                if (stringsGroup != null) {
                    foreach (var table in stringsGroup.stringFiles) {
                        this.AddStringTable(table.text);
                    }
                }
            }
        }

        /// Add a string of text to a script
        public void AddScript(string text)
        {
            try
            {
                dialogue.LoadString(text);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// Add a TextAsset to a script
        public void AddScript(TextAsset asset)
        {
            AddScript(asset.text);
        }

        /// Loads a string table, replacing any existing strings with the same
        /// key.
        public void AddStringTable(Dictionary<string,string> stringTable) {
            dialogue.AddStringTable(stringTable);
        }

        /// Add a string of text to a table
        public void AddStringTable(string text) {

            // Create the dictionary that will contain these values
            var stringTable = new Dictionary<string,string>();

            using (var reader = new System.IO.StringReader(text)) {
                using (var csv = new CsvHelper.CsvReader(reader)) {

                    var records = csv.GetRecords<Yarn.LocalisedLine>();

                    foreach (var record in records) {
                        stringTable[record.LineCode] = record.LineText;
                    }

                }
            }

            AddStringTable(stringTable);

        }

        /// Add a TextAsset to a table
        public void AddStringTable(TextAsset text) {
            AddStringTable(text.text);
        }

        /// Destroy the variable store and start again
        public void ResetDialogue ()
        {
            variableStorage.ResetToDefaults ();
            StartDialogue ();
        }

        /// Start the dialogue
        public void StartDialogue () {
            StartDialogue(startNode);
        }

        /// Start the dialogue from a given node
        public void StartDialogue (string startNode)
        {

            // Stop any processes that might be running already
            StopAllCoroutines ();
            dialogueUI.StopAllCoroutines ();

            // Get it going
            StartCoroutine (RunDialogue (startNode));
        }

        IEnumerator RunDialogue (string startNode = "Start")
        {
            // Mark that we're in conversation.
            isDialogueRunning = true;

            // Signal that we're starting up.
            yield return StartCoroutine(this.dialogueUI.DialogueStarted());

            // Get lines, options and commands from the Dialogue object,
            // one at a time.
            foreach (Yarn.Dialogue.RunnerResult step in dialogue.Run(startNode)) {

                if (step is Yarn.Dialogue.LineResult) {

                    // Wait for line to finish displaying
                    var lineResult = step as Yarn.Dialogue.LineResult;
                    yield return StartCoroutine (this.dialogueUI.RunLine (lineResult.line));

                }
                //else if (step is Yarn.Dialogue.OptionSetResult)
                //{
                //    // Wait for user to finish picking an option
                //    var optionSetResult = step as Yarn.Dialogue.OptionSetResult;
                //    yield return StartCoroutine (
                //        this.dialogueUI.RunOptions (
                //        optionSetResult.options,
                //        optionSetResult.setSelectedOptionDelegate
                //    ));

                //}
                else if (step is Yarn.Dialogue.CommandResult)
                {
                    // Wait for command to finish running
                    var commandResult = step as Yarn.Dialogue.CommandResult;

                    if (DispatchCommand(commandResult.command.text) == true) {
                        // command was dispatched
                    } else {
                        yield return StartCoroutine (this.dialogueUI.RunCommand (commandResult.command));
                    }


                } else if(step is Yarn.Dialogue.NodeCompleteResult) {

                    // Wait for post-node action
                    var nodeResult = step as Yarn.Dialogue.NodeCompleteResult;
                    yield return StartCoroutine (this.dialogueUI.NodeComplete (nodeResult.nextNode));
                }
            }

            // No more results! The dialogue is done.
            yield return StartCoroutine (this.dialogueUI.DialogueComplete ());

            // Clear the 'is running' flag. We do this after DialogueComplete returns,
            // to allow time for any animations that might run while transitioning
            // out of a conversation (ie letterboxing going away, etc)
            isDialogueRunning = false;
        }

        /// Clear the dialogue system
        public void Clear() {

            if (isDialogueRunning) {
                throw new System.InvalidOperationException("You cannot clear the dialogue system while a dialogue is running.");
            }

            dialogue.UnloadAll();
        }

        /// Stop the dialogue
        public void Stop() {
            isDialogueRunning = false;
            dialogue.Stop();
        }

        /// Test to see if a node name exists
        public bool NodeExists(string nodeName) {
            return dialogue.NodeExists(nodeName);
        }

        /// Return the current node name
        public string currentNodeName {
            get {
                return dialogue.currentNode;
            }
        }


        /// commands that can be automatically dispatched look like this:
        /// COMMANDNAME OBJECTNAME <param> <param> <param> ...
        /** We can dispatch this command if:
         * 1. it has at least 2 words
         * 2. the second word is the name of an object
         * 3. that object has components that have methods with the YarnCommand attribute that have the correct commandString set
         */
        public bool DispatchCommand(string command) {

            var words = command.Split(' ');

            // need 2 parameters in order to have both a command name
            // and the name of an object to find
            if (words.Length < 2)
                return false;

            var commandName = words[0];

            var objectName = words[1];

            var sceneObject = GameObject.Find(objectName);

            // If we can't find an object, we can't dispatch a command
            if (sceneObject == null)
                return false;

            Debug.Log("Dispatch command: " + command);

            int numberOfMethodsFound = 0;
            List<string[]> errorValues = new List<string[]>();

            List<string> parameters;

            if (words.Length > 2) {
                parameters = new List<string>(words);
                parameters.RemoveRange(0, 2);
            } else {
                parameters = new List<string>();
            }

            // Find every MonoBehaviour (or subclass) on the object
            foreach (var component in sceneObject.GetComponents<MonoBehaviour>()) {
                var type = component.GetType();

                // Find every method in this component
                foreach (var method in type.GetMethods()) {

                    // Find the YarnCommand attributes on this method
                    var attributes = (YarnCommandAttribute[]) method.GetCustomAttributes(typeof(YarnCommandAttribute), true);

                    // Find the YarnCommand whose commandString is equal to the command name
                    foreach (var attribute in attributes) {
                        if (attribute.commandString == commandName) {


                            var methodParameters = method.GetParameters();
                            bool paramsMatch = false;
                            // Check if this is a params array
                            if (methodParameters.Length == 1 && methodParameters[0].ParameterType.IsAssignableFrom(typeof(string[])))
                                {
                                    // Cool, we can send the command!
                                    string[][] paramWrapper = new string[1][];
                                    paramWrapper[0] = parameters.ToArray();
                                    method.Invoke(component, paramWrapper);
                                    numberOfMethodsFound++;
                                    paramsMatch = true;

                            }
                            // Otherwise, verify that this method has the right number of parameters
                            else if (methodParameters.Length == parameters.Count)
                            {
                                paramsMatch = true;
                                foreach (var paramInfo in methodParameters)
                                {
                                    if (!paramInfo.ParameterType.IsAssignableFrom(typeof(string)))
                                    {
                                        Debug.LogErrorFormat(sceneObject, "Method \"{0}\" wants to respond to Yarn command \"{1}\", but not all of its parameters are strings!", method.Name, commandName);
                                        paramsMatch = false;
                                        break;
                                    }
                                }
                                if (paramsMatch)
                                {
                                    // Cool, we can send the command!
                                    method.Invoke(component, parameters.ToArray());
                                    numberOfMethodsFound++;
                                }
                            }
                            //parameters are invalid, but name matches.
                            if (!paramsMatch)
                            {
                                //save this error in case a matching command is never found.
                                errorValues.Add(new string[] { method.Name, commandName, methodParameters.Length.ToString(), parameters.Count.ToString() });
                            }
                        }
                    }
                }
            }

            // Warn if we found multiple things that could respond
            // to this command.
            if (numberOfMethodsFound > 1) {
                Debug.LogWarningFormat(sceneObject, "The command \"{0}\" found {1} targets. " +
                    "You should only have one - check your scripts.", command, numberOfMethodsFound);
            } else if (numberOfMethodsFound == 0) {
                //list all of the near-miss methods only if a proper match is not found, but correctly-named methods are.
                foreach (string[] errorVal in errorValues) {
                    Debug.LogErrorFormat(sceneObject, "Method \"{0}\" wants to respond to Yarn command \"{1}\", but it has a different number of parameters ({2}) to those provided ({3}), or is not a string array!", errorVal[0], errorVal[1], errorVal[2], errorVal[3]);
                }
            }

            return numberOfMethodsFound > 0;
        }


        /// Add additional functions to the Dialogue object, mostly for quest handling
        private void AddCustomFunctions()
        {
            // Check State
            dialogue.library.RegisterFunction("IsUnavailable", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsUnavailable();
            });
            dialogue.library.RegisterFunction("IsAvailable", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsAvailable();
            });
            dialogue.library.RegisterFunction("IsInProgress", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsInProgress();
            });
            dialogue.library.RegisterFunction("IsDone", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsDone();
            });
            dialogue.library.RegisterFunction("IsDisabled", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsDisabled();
            });

            // IsActive / IsInactive
            dialogue.library.RegisterFunction("IsActive", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsActive();
            });
            dialogue.library.RegisterFunction("IsInactive", 1, delegate (Value[] parameters) {
                return questManager.FindQuest(parameters[0].AsString).IsInactive();
            });

            // Set State
            dialogue.library.RegisterFunction("SetUnavailable", 1, delegate (Value[] parameters) {
                questManager.FindQuest(parameters[0].AsString).SetUnavailable();
            });
            dialogue.library.RegisterFunction("SetAvailable", 1, delegate (Value[] parameters) {
                questManager.FindQuest(parameters[0].AsString).SetAvailable();
            });
            dialogue.library.RegisterFunction("SetInProgress", 1, delegate (Value[] parameters) {
                questManager.FindQuest(parameters[0].AsString).SetInProgress();
            });
            dialogue.library.RegisterFunction("SetDone", 1, delegate (Value[] parameters) {
                questManager.FindQuest(parameters[0].AsString).SetDone();
            });
            dialogue.library.RegisterFunction("SetDisabled", 1, delegate (Value[] parameters) {
                questManager.FindQuest(parameters[0].AsString).SetDisabled();
            });

            // Start/Finish Quests
            dialogue.library.RegisterFunction("StartQuest", 1, delegate (Value[] parameters) {
                questManager.StartQuest(questManager.FindQuest(parameters[0].AsString));
            });
            dialogue.library.RegisterFunction("FinishQuest", 1, delegate (Value[] parameters) {
                questManager.FinishQuest(questManager.FindQuest(parameters[0].AsString));
            });

            // Get reps
            dialogue.library.RegisterFunction("GetReputation", 0, delegate (Value[] parameters) {
                return gameState.reputation;
            });
            dialogue.library.RegisterFunction("GetFriendship", 1, delegate (Value[] parameters) {
                return gameState.GetFriendship(parameters[0].AsString);
            });

            // Set reps
            dialogue.library.RegisterFunction("SetReputation", 1, delegate (Value[] parameters) {
                gameState.reputation = (int)parameters[0].AsNumber;
            });
            dialogue.library.RegisterFunction("AddReputation", 1, delegate (Value[] parameters) {
                gameState.reputation += (int)parameters[0].AsNumber;
            });
            dialogue.library.RegisterFunction("SubtractReputation", 1, delegate (Value[] parameters) {
                gameState.reputation -= (int)parameters[0].AsNumber;
            });
            dialogue.library.RegisterFunction("SetFriendship", 2, delegate (Value[] parameters) {
                gameState.SetFriendship(parameters[0].AsString, (int)parameters[1].AsNumber);
            });
            dialogue.library.RegisterFunction("AddFriendship", 2, delegate (Value[] parameters) {
                gameState.AddFriendship(parameters[0].AsString, (int)parameters[1].AsNumber);
            });
            dialogue.library.RegisterFunction("SubtractFriendship", 2, delegate (Value[] parameters) {
                gameState.AddFriendship(parameters[0].AsString, -1 * (int)parameters[1].AsNumber);
            });

            // Inventory
            dialogue.library.RegisterFunction("GetInventory", 0, delegate (Value[] parameters) {
                return inventoryManager.GetInventory();
            });
            dialogue.library.RegisterFunction("ClearInventory", 0, delegate (Value[] parameters) {
                inventoryManager.ClearInventory();
            });
            dialogue.library.RegisterFunction("DropItem", 0, delegate (Value[] parameters) {
                inventoryManager.DropItem();
            });

            // Misc
            dialogue.library.RegisterFunction("Random", 2, delegate (Value[] parameters) {
                return random.Next((int)parameters[0].AsNumber, (int)parameters[1].AsNumber);
            });
            dialogue.library.RegisterFunction("Log", 1, delegate (Value[] parameters) {
                Debug.Log(parameters[0].AsString);
            });
            dialogue.library.RegisterFunction("CinematicMode", 0, delegate (Value[] parameters) {
                dialogueUI.cinematic_mode = true;
            });

            // LoadScene
            dialogue.library.RegisterFunction("LoadScene", 3, delegate (Value[] parameters)
            {
                Debug.LogFormat("LoadScene({0}, {1}, {2})", parameters[0].AsString, parameters[1].AsString, 
                    parameters[2].AsString);
                sceneLoader.StartLoad(parameters[0].AsString, (float)parameters[1].AsNumber,
                    (float)parameters[2].AsNumber);
            });
            dialogue.library.RegisterFunction("LoadScene", 4, delegate (Value[] parameters)
            {
                Debug.LogFormat("LoadScene({0}, {1}, {2}, {3})", parameters[0].AsString, parameters[1].AsString, 
                    parameters[2].AsString, parameters[3].AsString);
                sceneLoader.StartLoad(parameters[0].AsString, (float)parameters[1].AsNumber,
                    (float)parameters[2].AsNumber, (float)parameters[3].AsNumber);
            });
            dialogue.library.RegisterFunction("LoadChapter", 3, delegate (Value[] parameters)
            {
                Debug.LogFormat("LoadChapter({0}, {1}, {2})", parameters[0].AsString, parameters[1].AsString,
                    parameters[2].AsString);
                sceneLoader.StartLoad(parameters[0].AsString, (float)parameters[1].AsNumber,
                    (float)parameters[2].AsNumber, loadNewChapter: true);
            });
            dialogue.library.RegisterFunction("LoadChapter", 4, delegate (Value[] parameters)
            {
                Debug.LogFormat("LoadChapter({0}, {1}, {2}, {3})", parameters[0].AsString, parameters[1].AsString,
                    parameters[2].AsString, parameters[3].AsString);
                sceneLoader.StartLoad(parameters[0].AsString, (float)parameters[1].AsNumber,
                    (float)parameters[2].AsNumber, (float)parameters[3].AsNumber, loadNewChapter: true);
            });
        }

    }

    /// Apply this attribute to methods in your scripts to expose
    /// them to Yarn.

    /** For example:
     *  [YarnCommand("dosomething")]
     *      void Foo() {
     *         do something!
     *      }
     */
    public class YarnCommandAttribute : System.Attribute
    {
        public string commandString { get; private set; }

        public YarnCommandAttribute(string commandString) {
            this.commandString = commandString;
        }
    }

    /// Scripts that can act as the UI for the conversation should subclass this
    public abstract class DialogueUIBehaviour : MonoBehaviour
    {
        /// When true, ignores dialogue skips from user and doesn't pause 
        /// at the end of lines.
        /// Also sets skip to allow skipping whole cinematics (TBC)
        public bool cinematic_mode;

        /// A conversation has started.
        public virtual IEnumerator DialogueStarted() {
            // Default implementation does nothing.
            yield break;
        }

        /// Display a line.
        public abstract IEnumerator RunLine (Yarn.Line line);

        /// Display the options, and call the optionChooser when done.
        public abstract IEnumerator RunOptions (Yarn.Options optionsCollection,
                                                Yarn.OptionChooser optionChooser);

        /// Perform some game-specific command.
        public abstract IEnumerator RunCommand (Yarn.Command command);

        /// The node has ended.
        public virtual IEnumerator NodeComplete(string nextNode) {
            // Default implementation does nothing.
            yield break;
        }

        /// The conversation has ended.
        public virtual IEnumerator DialogueComplete () {
            // Default implementation does nothing.
            yield break;
        }
    }

    /// Scripts that can act as a variable storage should subclass this
    public abstract class VariableStorageBehaviour : MonoBehaviour, Yarn.VariableStorage
    {

        /// Not implemented here
        public virtual void SetNumber (string variableName, float number)
        {
            throw new System.NotImplementedException ();
        }

        /// Not implemented here
        public virtual float GetNumber (string variableName)
        {
            throw new System.NotImplementedException ();
        }

        /// Get a value
        public virtual Value GetValue(string variableName) {
            return new Yarn.Value(this.GetNumber(variableName));
        }

        /// Set a value
        public virtual void SetValue(string variableName, Value value) {
            this.SetNumber(variableName, value.AsNumber);
        }

        /// Not implemented here
        public virtual void Clear ()
        {
            throw new System.NotImplementedException ();
        }

        public abstract void ResetToDefaults ();

    }

}
