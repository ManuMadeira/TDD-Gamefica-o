using System;
using System.Threading;
using System.Threading.Tasks;
using Gamification.Domain.Awards.Ports;

namespace Gamification.Domain.Tests.Fakes
{
    public class FakeUnitOfWork : IAwardsUnitOfWork
    {
        public bool BeginCalled { get; private set; }
        public bool CommitCalled { get; private set; }
        public bool RollbackCalled { get; private set; }
        public bool FailOnCommit { get; set; }

        public Task BeginTransactionAsync(CancellationToken ct = default)
        {
            BeginCalled = true;
            return Task.CompletedTask;
        }

        public Task CommitTransactionAsync(CancellationToken ct = default)
        {
            CommitCalled = true;
            if (FailOnCommit) throw new InvalidOperationException("Simulated commit failure");
            return Task.CompletedTask;
        }

        public Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            RollbackCalled = true;
            return Task.CompletedTask;
        }
    }
}
