using System.Diagnostics.CodeAnalysis;

namespace iBurguer.ShoppingCart.Infrastructure.Http;

[ExcludeFromCodeCoverage]
public abstract class ApiConfiguration
{
    public abstract string Tag();

    /// <summary>
    /// Menu API URL to which requests will be sent.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The maximum number of retry attempts in case of failure.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// The delay, in milliseconds, between retry attempts.
    /// </summary>
    public int RetryDelay { get; set; } = 500;

    /// <summary>
    /// The minimum amount of traffic required before the circuit breaker kicks in.
    /// </summary>
    public int CircuitBreakerMinimumThroughput { get; set; } = 25;

    /// <summary>
    /// The duration, in seconds, during which the circuit breaker will remain open after being tripped.
    /// </summary>
    public int CircuitBreakerBreakDuration { get; set; } = 30;

    /// <summary>
    /// The timeout, in seconds, to wait for a response before terminating the request.
    /// </summary>
    public int Timeout { get; set; } = 5;

    public void ThrowIfInvalid()
    {
        if (string.IsNullOrEmpty(Url))
        {
            throw new InvalidOperationException("Url is misconfigured.");
        }
        
        if (MaxRetryAttempts <= 0)
        {
            throw new InvalidOperationException("MaxRetryAttempts must be greater than zero.");
        }

        if (RetryDelay <= 0)
        {
            throw new InvalidOperationException("RetryDelay must be greater than zero.");
        }

        if (CircuitBreakerMinimumThroughput <= 0)
        {
            throw new InvalidOperationException("CircuitBreakerMinimumThroughput must be greater than zero.");
        }

        if (CircuitBreakerBreakDuration <= 0)
        {
            throw new InvalidOperationException("CircuitBreakerBreakDuration must be greater than zero.");
        }

        if (Timeout <= 0)
        {
            throw new InvalidOperationException("Timeout must be greater than zero.");
        }
    }
}