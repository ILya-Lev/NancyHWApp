using FluentAssertions;
using LoyaltyProgram.Model;
using LoyaltyProgram.Services;
using Xunit;

namespace LoyaltyProgram.Tests
{
    public class InMemoryLoyaltyProgramStoreTests
    {
        [Fact]
        public void AddUser_NewUser_Success()
        {
            var store = new InMemoryLoyaltyProgramStore();
            var id = store.AddUser(new LoyaltyProgramUser
            {
                LoyaltyPoints = 10,
                Name = "John",
                Settings = new LoyaltyProgramSettings
                {
                    Interests = new[] { "Fishing", "Hunting", "Hiking" }
                }
            });

            id.Should().Be(1, "it's the first item in the storage");

            var user = store.GetUser(id);
            user.Name.Should().Be("John");
            user.LoyaltyPoints.Should().Be(10);
            user.Settings.Interests.Should().Contain(s => s == "Fishing");
        }

        [Fact]
        public void UpdateUser_UpdateProperties_Indeed()
        {
            var store = new InMemoryLoyaltyProgramStore();
            var id = store.AddUser(new LoyaltyProgramUser
            {
                LoyaltyPoints = 7,
                Name = "Phil",
                Settings = new LoyaltyProgramSettings
                {
                    Interests = new[] { "Poker", "Bridge", "Preference" }
                }
            });

            var initialUser = store.GetUser(id);
            initialUser.Name.Should().Be("Phil");
            initialUser.LoyaltyPoints.Should().Be(7);
            initialUser.Settings.Interests.Should().Contain(s => s == "Poker");


            store.UpdateUser(id, new LoyaltyProgramUser
            {
                LoyaltyPoints = 25,
                Name = "Emma",
                Settings = new LoyaltyProgramSettings
                {
                    Interests = new[] { "Racing", "Jugging" }
                }
            });

            var updatedUser = store.GetUser(id);

            updatedUser.Name.Should().Be("Emma");
            updatedUser.LoyaltyPoints.Should().Be(25);
            updatedUser.Settings.Interests.Should().Contain(s => s == "Racing");
        }
    }
}
