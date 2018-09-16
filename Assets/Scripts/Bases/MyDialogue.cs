using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine;
using System.Text;

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

        public string line;
        public string name;
        public string face;
        public float wait;
        public float speed;
        public float jitter;
        public bool pause;

        private static Regex re = new Regex(@"(.*?)( \((.*?)\))?:\s+(.*)");
        private static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the NPC to go in the name plate, and which portrait to load. If null, Dialogue manager will use the last name provided.</param>
        /// <param name="face">Emotion to show a portrait for, e.g. Happy, Sad. If null, will use Default portrait.</param>
        /// <param name="line">The dialogue string to show up on the screen.</param>
        /// <param name="wait">Number of seconds to wait until playing the text.</param>
        /// <param name="speed">Speed to display the text at.</param>
        /// <param name="jitter">Makes the letters jitter, if the character needs to be frightened.</param>
        /// <param name="pause">Whether to pause at the end of a line and wait for user input.</param>
        public DLine(string name, string face, string line, float wait=0, float speed=1, float jitter=0, bool pause=true)
        {
            this.name = name;
            this.face = face;
            this.line = line;
            this.wait = wait;
            this.speed = speed;
            this.jitter = jitter;
            this.pause = pause;
        }

        public Sprite GetFace()
        {
            return Resources.Load<Sprite>("Faces/" + name + face);
        }

        public override string ToString()
        {
            return string.Format("{0}(name={1}, face={2}, line={3})", this.GetType(), name, face.ToString(), line);
        }

        public static DLine FromYarnLine(Yarn.Line line)
        {
            Match m = re.Match(line.text);
            string face = m.Groups[2].Value;
            if (face.Equals(""))
                face = "Default";
            // Convert text to title case
            face = textInfo.ToTitleCase(face.ToLower());

            foreach (Group group in m.Groups)
            {
                Debug.Log(group.Value);
            }

            return new DLine(m.Groups[0].Value, face, m.Groups[3].Value);
        }
    }
}
