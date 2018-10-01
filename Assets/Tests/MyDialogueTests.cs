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
        Assert.AreEqual("Test", dLine.text);
    }

    [Test]
    public void DLine_FromYarnLine_WordsWithColonsAndParens()
    {
        line.text = "Nora (Sad): Test Test (Test): More test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Sad", dLine.face);
        Assert.AreEqual("Test Test (Test): More test.", dLine.text);
    }

    [Test]
    public void DLine_FromYarnLine_LowerCaseFace()
    {
        line.text = "Nora (happy): Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
    }

    [Test]
    public void DLine_FromYarnLine_UpperCaseFace()
    {
        line.text = "Nora (HAPPY): Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
    }

    [Test]
    public void DLine_FromYarnLine_TitleCaseFace()
    {
        line.text = "Nora (Happy): Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
    }

    [Test]
    public void DLine_FromYarnLine_Speed()
    {
        line.text = "Nora (Happy) <speed=1.5>: Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
        Assert.AreEqual(1.5f, dLine.speed);
    }

    [Test]
    public void DLine_FromYarnLine_Jitter()
    {
        line.text = "Nora (Happy) <jitter=1.5>: Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
        Assert.AreEqual(1.5f, dLine.jitter);
    }

    [Test]
    public void DLine_FromYarnLine_Pause()
    {
        line.text = "Nora (Happy) <nopause>: Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
        Assert.AreEqual(false, dLine.pause);
    }

    [Test]
    public void DLine_FromYarnLine_ClearText()
    {
        line.text = "Nora (Happy) <nocleartext>: Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
        Assert.AreEqual(false, dLine.clear_text);
    }

    [Test]
    public void DLine_FromYarnLine_WaitAndClear()
    {
        line.text = "Nora (Happy) <nocleartext; wait=1.5>: Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
        Assert.AreEqual(false, dLine.clear_text);
        Assert.AreEqual(1.5f, dLine.wait);
    }

    [Test]
    public void DLine_FromYarnLine_Wait()
    {
        line.text = "Nora (Happy) <wait=1.5>: Test.";
        DLine dLine = DLine.FromYarnLine(line);

        Assert.AreEqual("Nora", dLine.name);
        Assert.AreEqual("Happy", dLine.face);
        Assert.AreEqual("Test.", dLine.text);
        Assert.AreEqual(1.5f, dLine.wait);
    }
}
