using Gamification.Domain.Awards.Models;

namespace Gamification.Domain.Awards.Ports;

/// <summary>
/// Port for reading award-related data (Query side)
/// </summary>
public interface IAwardsReadStore
{
    /// <summary>
    /// Gets an award by its unique identifier
    /// </summary>
    Task<Award?> GetByIdAsync(Guid awardId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all awards for a specific user
    /// </summary>
    Task<IReadOnlyList<Award>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an award exists for the given criteria
    /// </summary>
    Task<bool> ExistsAsync(Guid awardId, CancellationToken cancellationToken = default);
} 