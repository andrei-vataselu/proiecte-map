namespace OrderProcessing.Api.States;

public class InvalidOrderTransitionException : InvalidOperationException
{
    public InvalidOrderTransitionException(string from, string action)
        : base($"Nu pot executa '{action}' in starea '{from}'") { }
}
