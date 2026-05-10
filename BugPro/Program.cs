using Stateless;

namespace BugPro;

public sealed class Bug
{
    public enum State
    {
        New,
        Triaged,
        InProgress,
        Resolved,
        Verified,
        Reopened,
        Closed,
        Rejected
    }

    public enum Trigger
    {
        Triage,
        StartProgress,
        Resolve,
        Verify,
        Close,
        Reopen,
        Reject
    }

    private readonly StateMachine<State, Trigger> _machine;

    public Bug(State initialState = State.New)
    {
        _machine = new StateMachine<State, Trigger>(initialState);
        Configure();
    }

    public State CurrentState => _machine.State;

    public IReadOnlyCollection<Trigger> PermittedTriggers => _machine.PermittedTriggers.ToArray();

    public void Fire(Trigger trigger) => _machine.Fire(trigger);

    private void Configure()
    {
        _machine.Configure(State.New)
            .Permit(Trigger.Triage, State.Triaged)
            .Permit(Trigger.Reject, State.Rejected);

        _machine.Configure(State.Triaged)
            .Permit(Trigger.StartProgress, State.InProgress)
            .Permit(Trigger.Reject, State.Rejected);

        _machine.Configure(State.InProgress)
            .Permit(Trigger.Resolve, State.Resolved)
            .Permit(Trigger.Reject, State.Rejected);

        _machine.Configure(State.Resolved)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Reopened)
            .Permit(Trigger.StartProgress, State.InProgress)
            .Permit(Trigger.Reject, State.Rejected);

        _machine.Configure(State.Rejected)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);
    }
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Initial state: {bug.CurrentState}");

        bug.Fire(Bug.Trigger.Triage);
        bug.Fire(Bug.Trigger.StartProgress);
        bug.Fire(Bug.Trigger.Resolve);

        Console.WriteLine($"State after progress: {bug.CurrentState}");
        Console.WriteLine("Allowed actions now: " + string.Join(", ", bug.PermittedTriggers));
    }
}
