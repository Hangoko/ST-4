using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void NewBug_HasNewState()
    {
        var bug = new Bug();
        Assert.AreEqual(Bug.State.New, bug.CurrentState);
    }

    [TestMethod]
    public void NewBug_AllowsTriage()
    {
        var bug = new Bug();
        CollectionAssert.Contains(bug.PermittedTriggers.ToList(), Bug.Trigger.Triage);
    }

    [TestMethod]
    public void NewBug_AllowsReject()
    {
        var bug = new Bug();
        CollectionAssert.Contains(bug.PermittedTriggers.ToList(), Bug.Trigger.Reject);
    }

    [TestMethod]
    public void Triage_FromNew_GoesToTriaged()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Triage);
        Assert.AreEqual(Bug.State.Triaged, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromNew_GoesToRejected()
    {
        var bug = new Bug();
        bug.Fire(Bug.Trigger.Reject);
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void StartProgress_FromTriaged_GoesToInProgress()
    {
        var bug = new Bug(Bug.State.Triaged);
        bug.Fire(Bug.Trigger.StartProgress);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromTriaged_GoesToRejected()
    {
        var bug = new Bug(Bug.State.Triaged);
        bug.Fire(Bug.Trigger.Reject);
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void Resolve_FromInProgress_GoesToResolved()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Fire(Bug.Trigger.Resolve);
        Assert.AreEqual(Bug.State.Resolved, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromInProgress_GoesToRejected()
    {
        var bug = new Bug(Bug.State.InProgress);
        bug.Fire(Bug.Trigger.Reject);
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void Verify_FromResolved_GoesToVerified()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Fire(Bug.Trigger.Verify);
        Assert.AreEqual(Bug.State.Verified, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromResolved_GoesToReopened()
    {
        var bug = new Bug(Bug.State.Resolved);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Close_FromVerified_GoesToClosed()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Fire(Bug.Trigger.Close);
        Assert.AreEqual(Bug.State.Closed, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromVerified_GoesToReopened()
    {
        var bug = new Bug(Bug.State.Verified);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void StartProgress_FromReopened_GoesToInProgress()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Fire(Bug.Trigger.StartProgress);
        Assert.AreEqual(Bug.State.InProgress, bug.CurrentState);
    }

    [TestMethod]
    public void Reject_FromReopened_GoesToRejected()
    {
        var bug = new Bug(Bug.State.Reopened);
        bug.Fire(Bug.Trigger.Reject);
        Assert.AreEqual(Bug.State.Rejected, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromClosed_GoesToReopened()
    {
        var bug = new Bug(Bug.State.Closed);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void Reopen_FromRejected_GoesToReopened()
    {
        var bug = new Bug(Bug.State.Rejected);
        bug.Fire(Bug.Trigger.Reopen);
        Assert.AreEqual(Bug.State.Reopened, bug.CurrentState);
    }

    [TestMethod]
    public void InvalidTransition_FromNew_Throws()
    {
        var bug = new Bug();
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Close));
    }

    [TestMethod]
    public void InvalidTransition_FromClosed_Throws()
    {
        var bug = new Bug(Bug.State.Closed);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Resolve));
    }

    [TestMethod]
    public void InvalidTransition_FromRejected_Throws()
    {
        var bug = new Bug(Bug.State.Rejected);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Verify));
    }

    [TestMethod]
    public void InvalidTransition_FromTriaged_Throws()
    {
        var bug = new Bug(Bug.State.Triaged);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Close));
    }

    [TestMethod]
    public void InvalidTransition_FromInProgress_Throws()
    {
        var bug = new Bug(Bug.State.InProgress);
        Assert.ThrowsException<InvalidOperationException>(() => bug.Fire(Bug.Trigger.Verify));
    }
}
