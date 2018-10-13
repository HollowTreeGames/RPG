using Enums;

public class QuestLoader
{
    private static Quest[] tutorialQuestList =
    {
        new Quest("Oakewood Stretch Legs", "Walk Around"),
        new Quest("Oakewood Run", "Run in a circle"),
        new Quest("Oakewood Pick Up Rock", "Pick up the rock"),
        new Quest("Oakewood Drop Rock", "Drop the rock"),
        new Quest("Oakewood Talk Rock", "Talk to the rock")
    };

    public static Quest[] GetQuestList(string levelName)
    {
        switch (levelName)
        {
            case "Anustart":
                return tutorialQuestList;
            default:
                throw new System.NotImplementedException(
                    string.Format("'{0}' is not a recognized level name.", levelName));
        }
    }
}
