using System.Data;
using FluentAssertions;
using Moq;
using Studweb.Infrastructure.Persistance;
using Studweb.Infrastructure.UnitTests.Persistance.TestUtils;
using Studweb.Infrastructure.Utilities;

namespace Studweb.Infrastructure.UnitTests.Persistance;

public class UnitOfWorkTests
{
    private readonly UnitOfWork _unitOfWork;
    private readonly Mock<IDbContext> _mockDbContext;
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly Mock<IDbTransaction> _mockDbTransaction;

    public UnitOfWorkTests()
    {
        _mockDbContext = new Mock<IDbContext>();
        _mockDbConnection = new Mock<IDbConnection>();
        _mockDbTransaction = new Mock<IDbTransaction>();

        _mockDbContext
            .SetupGet(d => d.Connection)
            .Returns(_mockDbConnection.Object);
        _mockDbContext
            .SetupGet(d => d.Transaction)
            .Returns(_mockDbTransaction.Object);

        _unitOfWork = new UnitOfWork(_mockDbContext.Object);
    }

    [Fact]
    public void
        BeginTransaction_ShouldOpenConnectionAndStartTransaction_WhenConnectionIsClosedAndTransactionIsNotStarted()
    {
        // Arrange

        _mockDbConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Closed);
        
        // Act
        
        _unitOfWork.BeginTransaction();
        
        // Assert
        
        _mockDbConnection.Verify(c => c.Open(), Times.Once);
        _mockDbConnection.Verify(c => c.BeginTransaction(), Times.Once);
        _mockDbTransaction.Object.Should().NotBeNull();

    }

    [Fact]
    public void BeginTransaction_ShouldThrowInvalidOperationException_WhenConnectionIsNull()
    {
        // Arrange

        _mockDbContext
            .SetupGet(dbContext => dbContext.Connection)
            .Returns(value: null);
        
        // Act
        
        Action action = () => _unitOfWork.BeginTransaction();
        
        // Assert

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("The database connection has not been initialized.");

    }

    [Fact]
    public void BeginTransaction_ShouldThrowInvalidOperationException_WhenTransactionIsAlreadyStarted()
    {
        // Arrange
        
        _mockDbConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Closed);
        
        UnitOfWorkUtils.SetPrivateFieldTransactionStartedOnTrue(_unitOfWork);
        
        // Act

        Action action = () => _unitOfWork.BeginTransaction();
        
        // Assert

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("A transaction has already been started.");
    }

    [Fact]
    public void Commit_ShouldCommitTransaction_WhenTransactionIsStarted()
    {
        // Arrange

        _mockDbConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Open);
        
        UnitOfWorkUtils.SetPrivateFieldTransactionStartedOnTrue(_unitOfWork);
        
        // Act
        
        _unitOfWork.Commit();
        
        // Assert

        _mockDbTransaction.Verify(t => t.Rollback(), Times.Never);
        _mockDbTransaction.Verify(t => t.Commit(), Times.Once);
        _mockDbTransaction.Verify(t => t.Dispose(), Times.Once);
        _mockDbContext.VerifySet(d => d.Transaction = null);
    }

    [Fact]
    public void Commit_ShouldThrowInvalidOperationException_WhenTransactionIsNotStarted()
    {
        // Arrange
        
        // Act

        Action action = () => _unitOfWork.Commit();
        
        // Assert

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("No transaction has been started to commit.");
    }

    [Fact]
    public void Commit_ShouldRollbackTransactionAndThrowInvalidOperationException_WhenTransactionIsNull()
    {
        // Arrange

        _mockDbConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Open);
        _mockDbTransaction
            .Setup(t => t.Commit())
            .Throws(new Exception());
        
        UnitOfWorkUtils.SetPrivateFieldTransactionStartedOnTrue(_unitOfWork);
        
        // Act

        Action action = () => _unitOfWork.Commit();
        
        // Assert

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("An error occurred during transaction commit. Transaction has been rolled back.");
        _mockDbTransaction.Verify(t => t.Rollback(), Times.Once);
        _mockDbTransaction.Verify(t => t.Dispose(), Times.Once);
        _mockDbContext.VerifySet(d => d.Transaction = null);
    }

    [Fact]
    public void CommitAndCloseConnection_ShouldCommitTransactionAndCloseConnection_WhenConnectionIsNotNull()
    {
        // Arrange
        
        _mockDbConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Open);
        
        UnitOfWorkUtils.SetPrivateFieldTransactionStartedOnTrue(_unitOfWork);
        
        // Act
        
        _unitOfWork.CommitAndCloseConnection();
        
        // Assert
        
        _mockDbConnection.Verify(c => c.Close(), Times.Once);
        _mockDbConnection.Verify(c => c.Dispose(), Times.Once);
    }

    [Fact]
    public void CommitAndCloseConnection_ShouldCommitAndReturn_WhenConnectionIsNull()
    {
        // Arrange
        
        _mockDbConnection
            .Setup(c => c.State)
            .Returns(ConnectionState.Open);
        _mockDbContext
            .SetupGet(d => d.Connection)
            .Returns(value: null);
        
        UnitOfWorkUtils.SetPrivateFieldTransactionStartedOnTrue(_unitOfWork);
        
        // Act
        
        _unitOfWork.CommitAndCloseConnection();
        
        // Assert
        
        _mockDbConnection.Verify(c => c.Close(), Times.Never);
        _mockDbConnection.Verify(c => c.Dispose(), Times.Never);
    }

    [Fact]
    public void Rollback_ShouldRollbackTransaction_WhenTransactionIsStarted()
    {
        // Arrange
        
        UnitOfWorkUtils.SetPrivateFieldTransactionStartedOnTrue(_unitOfWork);
        
        // Act
        
        _unitOfWork.Rollback();
        
        // Assert
        
        _mockDbTransaction.Verify(t => t.Rollback(), Times.Once);
        _mockDbTransaction.Verify(t => t.Dispose(), Times.Once);
        _mockDbContext.VerifySet(d => d.Transaction = null);
    }

    [Fact]
    public void Rollback_ShouldThrowInvalidOperationException_WhenTransactionIsNotStarted()
    {
        // Arrange
        
        // Act

        Action action = () => _unitOfWork.Rollback();
        
        // Assert

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("No transaction has been started to rollback.");
    }

    [Fact]
    public void Dispose_ShouldDisposeTransaction_WhenTransactionIsNotDisposed()
    {
        // Arrange
        
        UnitOfWorkUtils.SetPrivateFieldDisposedOnFalse(_unitOfWork);
        
        // Act
        
        _unitOfWork.Dispose();
        
        // Assert
        
        _mockDbConnection.Verify(c => c.Dispose(), Times.Once);
        _mockDbTransaction.Verify(t => t.Dispose(), Times.Once);
        _mockDbContext.VerifySet(d => d.Transaction = null);
        
    }
    
}