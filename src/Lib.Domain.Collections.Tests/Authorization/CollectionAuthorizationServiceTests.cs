using System.Collections.Generic;
using AwesomeAssertions;
using Lib.Domain.Collections.Apis;
using Lib.Domain.Collections.Authorization;
using Lib.Shared.DataModels.Entities.Itrs.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.Domain.Collections.Tests.Authorization;

[TestClass]
public sealed class CollectionAuthorizationServiceTests
{
    [TestMethod, TestCategory("unit")]
    public void IsOwner_MatchingIds_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();

        // Act
        bool actual = subject.IsOwner("user-1", "user-1");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsOwner_DifferentIds_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();

        // Act
        bool actual = subject.IsOwner("user-1", "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAdmin_UserIsAdmin_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" }
        ];

        // Act
        bool actual = subject.IsAdmin(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAdmin_UserIsEditor_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "editor" }
        ];

        // Act
        bool actual = subject.IsAdmin(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAdmin_UserNotInList_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        List<IAuthorizedUserItrEntity> authorizedUsers = [];

        // Act
        bool actual = subject.IsAdmin(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsOwnerOrAdmin_UserIsOwner_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        List<IAuthorizedUserItrEntity> authorizedUsers = [];

        // Act
        bool actual = subject.IsOwnerOrAdmin("user-1", authorizedUsers, "user-1");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsOwnerOrAdmin_UserIsAdmin_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" }
        ];

        // Act
        bool actual = subject.IsOwnerOrAdmin("user-1", authorizedUsers, "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsOwnerOrAdmin_UserIsNeither_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "admin" }
        ];

        // Act
        bool actual = subject.IsOwnerOrAdmin("user-1", authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsEditor_UserIsEditor_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "editor" }
        ];

        // Act
        bool actual = subject.IsEditor(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsEditor_UserIsAdmin_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" }
        ];

        // Act
        bool actual = subject.IsEditor(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsEditor_UserNotInList_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        List<IAuthorizedUserItrEntity> authorizedUsers = [];

        // Act
        bool actual = subject.IsEditor(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsViewer_UserIsViewer_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "viewer" }
        ];

        // Act
        bool actual = subject.IsViewer(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsViewer_UserIsEditor_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "editor" }
        ];

        // Act
        bool actual = subject.IsViewer(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsViewer_UserNotInList_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        List<IAuthorizedUserItrEntity> authorizedUsers = [];

        // Act
        bool actual = subject.IsViewer(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAuthorizedUser_UserInList_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "viewer" }
        ];

        // Act
        bool actual = subject.IsAuthorizedUser(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAuthorizedUser_UserNotInList_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "viewer" }
        ];

        // Act
        bool actual = subject.IsAuthorizedUser(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAuthorizedUser_EmptyList_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        List<IAuthorizedUserItrEntity> authorizedUsers = [];

        // Act
        bool actual = subject.IsAuthorizedUser(authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void CanGrantAccess_UserIsOwner_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        List<IAuthorizedUserItrEntity> authorizedUsers = [];

        // Act
        bool actual = subject.CanGrantAccess("user-1", authorizedUsers, "user-1");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void CanGrantAccess_UserIsAdmin_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" }
        ];

        // Act
        bool actual = subject.CanGrantAccess("user-1", authorizedUsers, "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void CanGrantAccess_UserIsEditor_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "editor" }
        ];

        // Act
        bool actual = subject.CanGrantAccess("user-1", authorizedUsers, "user-2");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_OwnerRevokingAnyone_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-1", "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_SelfRevoke_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "editor" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_AdminRevokingEditor_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" },
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "editor" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-3");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_AdminRevokingViewer_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" },
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "viewer" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-3");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_AdminRevokingAdmin_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" },
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "admin" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-3");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_EditorRevokingViewer_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "editor" },
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "viewer" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-3");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_AdminRevokingNonExistentUser_ShouldReturnFalse()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-99");

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod, TestCategory("unit")]
    public void CanRevokeAccess_ViewerRevokingSelf_ShouldReturnTrue()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "viewer" }
        ];

        // Act
        bool actual = subject.CanRevokeAccess("user-1", authorizedUsers, "user-2", "user-2");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAdmin_MultipleUsers_ShouldFindCorrectUser()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "viewer" },
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "admin" },
            new AuthorizedUserItrEntityFake { UserId = "user-4", Role = "editor" }
        ];

        // Act
        bool actual = subject.IsAdmin(authorizedUsers, "user-3");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod, TestCategory("unit")]
    public void IsAuthorizedUser_MultipleRoles_ShouldFindAnyRole()
    {
        // Arrange
        ICollectionAuthorizationService subject = new CollectionAuthorizationService();
        IReadOnlyList<IAuthorizedUserItrEntity> authorizedUsers =
        [
            new AuthorizedUserItrEntityFake { UserId = "user-2", Role = "admin" },
            new AuthorizedUserItrEntityFake { UserId = "user-3", Role = "editor" },
            new AuthorizedUserItrEntityFake { UserId = "user-4", Role = "viewer" }
        ];

        // Act
        bool actualAdmin = subject.IsAuthorizedUser(authorizedUsers, "user-2");
        bool actualEditor = subject.IsAuthorizedUser(authorizedUsers, "user-3");
        bool actualViewer = subject.IsAuthorizedUser(authorizedUsers, "user-4");

        // Assert
        actualAdmin.Should().BeTrue();
        actualEditor.Should().BeTrue();
        actualViewer.Should().BeTrue();
    }

    private sealed class AuthorizedUserItrEntityFake : IAuthorizedUserItrEntity
    {
        public string UserId { get; init; }
        public string Role { get; init; }
        public string GrantedAt { get; init; }
        public string GrantedBy { get; init; }
    }
}
