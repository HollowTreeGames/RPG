using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using MyDialogue;

public class MyDialogueTests {

    Yarn.Line line;

    [SetUp]
    public void SetUp()
    {
        line = new Yarn.Line();
    }

    [Test]
    public void DLine_FromYarnLine_Default() {
        line.text = "Henry: Test";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Henry", dLine.name);
        Assert.AreEqual("Default", dLine.face);
        Assert.AreEqual("Test", dLine.line);
    }

    [Test]
    public void DLine_FromYarnLine_WordsWithColonsAndParens()
    {
        line.text = "Nora (Sad): Test Test (Test): More test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Sad", dLine.face);
        Assert.AreEqual("Test Test (Test): More test.", dLine.line);
    }

    [Test]
    public void DLine_FromYarnLine_LowerCaseFace()
    {
        line.text = "Nora (happy): Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.line);
    }

    [Test]
    public void DLine_FromYarnLine_UpperCaseFace()
    {
        line.text = "Nora (HAPPY): Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.line);
    }

    [Test]
    public void DLine_FromYarnLine_TitleCaseFace()
    {
        line.text = "Nora (Happy): Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.line);
    }
}
