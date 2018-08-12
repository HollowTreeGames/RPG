using System.Reflection;
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

        public string name;
        public string face;
        public string line;

        public DLine(string name, string face, string line)
        {
            this.name = name;
            this.face = face;
            this.line = line;
        }

        public Sprite getFace()
        {
            return Resources.Load<Sprite>("Faces/" + name + face);
        }

        public override string ToString()
        {
            return string.Format("{0}(name={1}, face={2}, line={3})", this.GetType(), name, face.ToString(), line);
        }
    }
}
