using Gamification.Domain.Awards.Models;

namespace Gamification.Domain.Awards.Ports;

/// <summary>
/// Port for writing award-related data (Command side)
/// </summary>
public interface IAwardsWriteStore
{
    /// <summary>
    /// Creates a new award
    /// </summary>
    Task CreateAsync(Award award, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing award
    /// </summary>
    Task UpdateAsync(Award award, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an award by its identifier
    /// </summary>
    Task DeleteAsync(Guid awardId, CancellationToken cancellationToken = default);
}