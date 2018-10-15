using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine;

namespace MyDialogue
{
    public class Faces : MonoBehaviour
    {
        public static Sprite BelfryDefault;
        public static Sprite BelfryHappy;
        public static Sprite BelfrySad;

        public static Sprite HenryDefault;
        public static Sprite HenryHappy;
        public static Sprite HenrySad;

        public static Sprite OakewoodDefault;
        public static Sprite OakewoodHappy;
        public static Sprite OakewoodSad;

        public static Sprite ParsleyDefault;
        public static Sprite ParsleyHappy;
        public static Sprite ParsleySad;

        public static void InitFaces()
        {
            BelfryDefault = Resources.Load<Sprite>("Faces/BelfryDefault");
            BelfryHappy = Resources.Load<Sprite>("Faces/BelfryHappy");
            BelfrySad = Resources.Load<Sprite>("Faces/BelfrySad");

            HenryDefault = Resources.Load<Sprite>("Faces/HenryDefault");
            HenryHappy = Resources.Load<Sprite>("Faces/HenryHappy");
            HenrySad = Resources.Load<Sprite>("Faces/HenrySad");
        }

        public void Start()
        {
            FieldInfo[] fieldInfos = this.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                Sprite sprite = Resources.Load<Sprite>("Faces/" + fieldInfo.Name);
                Debug.Log(sprite.ToString());
                fieldInfo.SetValue(this, sprite);
            }
        }
    }

    public class DLine
    {

        public string text;
        public string name;
        public string face;
        public float speed;
        public float jitter;
        public bool pause;
        public bool clear_text;
        public float wait;

        private static Regex re = new Regex(@"(.*?)\s*(\((.*?)\))?\s*(\<(.*?)\>)?\s*:(.*)");
        private static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the NPC to go in the name plate, and which portrait to load. If null, Dialogue manager will use the last name provided.</param>
        /// <param name="face">Emotion to show a portrait for, e.g. Happy, Sad. If null, will use Default portrait.</param>
        /// <param name="line">The dialogue string to show up on the screen.</param>
        /// <param name="speed">Speed to display the text at.</param>
        /// <param name="jitter">Makes the letters jitter, if the character needs to be frightened.</param>
        /// <param name="pause">Whether to pause at the end of a line and wait for user input.</param>
        /// <param name="clear_text">Whether the previous text is cleared when the next line is played.</param>
        /// <param name="wait">Number of seconds to wait after playing the text before continuing. Only valid when pause=false.</param>
        public DLine(string name, string face, string line, float speed=0.025f, float jitter=0, bool pause=true, bool clear_text=true, float wait=0)
        {
            this.name = name;
            this.face = face;
            this.text = line;
            this.speed = speed;
            this.jitter = jitter;
            this.pause = pause;
            this.clear_text = clear_text;
            this.wait = wait;
        }

        public Sprite GetFace()
        {
            string path = string.Format("Faces/{0} - {1}", name, face);
            Debug.Log(path);
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>("Faces/OopsyWoopsy");
            }
            return sprite;
        }

        public override string ToString()
        {
            return string.Format("{0}(name={1}, face={2}, line={3})", this.GetType(), name, face.ToString(), text);
        }

        public static DLine FromYarnLine(Yarn.Line line)
        {
            float speed = 0.025f;
            float jitter = 0;
            bool pause = true;
            bool clear_text = true;
            float wait = 0;

            Match m = re.Match(line.text);
            string face = m.Groups[3].Value;
            if (face.Equals(""))
                face = "Neutral";
            // Convert text to title case
            face = textInfo.ToTitleCase(face.Trim().ToLower());

            // Convert flags section to dictionary
            string flags_string = m.Groups[5].Value;
            if (!flags_string.Equals(""))
            {
                Match flags_match;
                // speed
                flags_match = new Regex(@"speed\s*=\s*([\d\.]+)", RegexOptions.IgnoreCase).Match(flags_string);
                if (flags_match.Success)
                    speed = float.Parse(flags_match.Groups[1].Value);
                // jitter
                flags_match = new Regex(@"jitter\s*=\s*([\d\.]+)", RegexOptions.IgnoreCase).Match(flags_string);
                if (flags_match.Success)
                    jitter = float.Parse(flags_match.Groups[1].Value);
                // wait
                flags_match = new Regex(@"wait\s*=\s*([\d\.]+)", RegexOptions.IgnoreCase).Match(flags_string);
                if (flags_match.Success)
                    wait = float.Parse(flags_match.Groups[1].Value);

                // pause
                flags_match = new Regex(@"nopause", RegexOptions.IgnoreCase).Match(flags_string);
                if (flags_match.Success)
                    pause = false;
                // clear_text
                flags_match = new Regex(@"nocleartext", RegexOptions.IgnoreCase).Match(flags_string);
                if (flags_match.Success)
                    clear_text = false;
            }

            return new DLine(m.Groups[1].Value.Trim(), face, m.Groups[6].Value.Trim(), 
                wait: wait, speed: speed, jitter: jitter, pause: pause, clear_text: clear_text);
        }
    }
}
