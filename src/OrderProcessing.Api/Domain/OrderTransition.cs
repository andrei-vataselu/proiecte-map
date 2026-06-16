namespace OrderProcessing.Api.Domain;

public record OrderTransition(string FromState, string ToState, DateTime At);
