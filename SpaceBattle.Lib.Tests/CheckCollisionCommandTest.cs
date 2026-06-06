using Moq;
using StarWars.Lib;
using Xunit;

namespace StarWars.Test;

public class CheckCollisionCommandTest
{
    private static Mock<ICollidable> MakeCollidable(string shapeId = "ship")
    {
        var mock = new Mock<ICollidable>();
        mock.Setup(c => c.GetShapeId()).Returns(shapeId);
        mock.Setup(c => c.GetPosition()).Returns(new Vector([0, 0]));
        mock.Setup(c => c.GetVelocity()).Returns(new Vector([0, 0]));
        return mock;
    }

    [Fact]
    public void Execute_CollisionDetected_ExecutesOnCollision()
    {
        var mockSelf = MakeCollidable("ship");
        var mockOther = MakeCollidable("torpedo");
        var mockOnCollision = new Mock<Hwdtech.ICommand>();
        var mockDetector = new Mock<ICollisionDetector>();
        var mockRepo = new Mock<ICollidableRepository>();

        mockRepo.Setup(r => r.GetAll()).Returns([("other-1", mockOther.Object)]);
        mockDetector.Setup(d => d.Collides(mockSelf.Object, mockOther.Object)).Returns(true);

        new CheckCollisionCommand("self-id", mockSelf.Object, mockRepo.Object, mockDetector.Object, mockOnCollision.Object)
            .Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void Execute_NoCollision_DoesNotExecuteOnCollision()
    {
        var mockSelf = MakeCollidable();
        var mockOther = MakeCollidable("torpedo");
        var mockOnCollision = new Mock<Hwdtech.ICommand>();
        var mockDetector = new Mock<ICollisionDetector>();
        var mockRepo = new Mock<ICollidableRepository>();

        mockRepo.Setup(r => r.GetAll()).Returns([("other-1", mockOther.Object)]);
        mockDetector.Setup(d => d.Collides(It.IsAny<ICollidable>(), It.IsAny<ICollidable>())).Returns(false);

        new CheckCollisionCommand("self-id", mockSelf.Object, mockRepo.Object, mockDetector.Object, mockOnCollision.Object)
            .Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void Execute_SkipsSelfById()
    {
        var mockSelf = MakeCollidable();
        var mockOnCollision = new Mock<Hwdtech.ICommand>();
        var mockDetector = new Mock<ICollisionDetector>();
        var mockRepo = new Mock<ICollidableRepository>();

        // Repository only contains self — should be skipped
        mockRepo.Setup(r => r.GetAll()).Returns([("self-id", mockSelf.Object)]);

        new CheckCollisionCommand("self-id", mockSelf.Object, mockRepo.Object, mockDetector.Object, mockOnCollision.Object)
            .Execute();

        mockDetector.Verify(d => d.Collides(It.IsAny<ICollidable>(), It.IsAny<ICollidable>()), Times.Never);
        mockOnCollision.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void Execute_StopsAfterFirstCollision()
    {
        var mockSelf = MakeCollidable();
        var mockOther1 = MakeCollidable("torpedo");
        var mockOther2 = MakeCollidable("asteroid");
        var mockOnCollision = new Mock<Hwdtech.ICommand>();
        var mockDetector = new Mock<ICollisionDetector>();
        var mockRepo = new Mock<ICollidableRepository>();

        mockRepo.Setup(r => r.GetAll()).Returns(
        [
            ("obj-1", mockOther1.Object),
            ("obj-2", mockOther2.Object)
        ]);
        mockDetector.Setup(d => d.Collides(mockSelf.Object, mockOther1.Object)).Returns(true);
        mockDetector.Setup(d => d.Collides(mockSelf.Object, mockOther2.Object)).Returns(true);

        new CheckCollisionCommand("self-id", mockSelf.Object, mockRepo.Object, mockDetector.Object, mockOnCollision.Object)
            .Execute();

        mockOnCollision.Verify(c => c.Execute(), Times.Once);
    }
}
