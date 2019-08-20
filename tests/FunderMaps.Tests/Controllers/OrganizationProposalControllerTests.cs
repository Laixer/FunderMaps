using FunderMaps.Controllers.Api;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Tests.Helpers;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FunderMaps.Tests.Controllers
{
    public class OrganizationProposalControllerTests
    {
        [Fact]
        public async Task Get_ReturnsOrganizationProposal()
        {
            // Arrange
            var options = DatabaseWrapper.GenerateDatabase<FunderMapsDbContext>(nameof(Get_ReturnsOrganizationProposal));

            var token = Guid.NewGuid();
            var proposal = new OrganizationProposal("testcomp", "info@example.org")
            {
                Token = token,
            };

            using (var context = new FunderMapsDbContext(options))
            {
                await context.OrganizationProposals.AddAsync(proposal);
                await context.SaveChangesAsync();
            }

            using (var context = new FunderMapsDbContext(options))
            {
                var controller = new OrganizationProposalController(context, new LookupNormalizer());

                // Act
                var result = await controller.GetAsync(token);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<OrganizationProposal>(okResult.Value);
                Assert.Equal(proposal.Token, returnValue.Token);
                Assert.Equal(proposal.Name, returnValue.Name);
                Assert.Equal(proposal.Email, returnValue.Email);
            }
        }

        [Fact]
        public async Task Get_ReturnsNotFound_GivenWrongId()
        {
            // Arrange
            var options = DatabaseWrapper.GenerateDatabase<FunderMapsDbContext>(nameof(Get_ReturnsNotFound_GivenWrongId));

            var token = Guid.NewGuid();
            var proposal = new OrganizationProposal("testcomp", "info@example.org")
            {
                Token = token,
            };

            using (var context = new FunderMapsDbContext(options))
            {
                await context.OrganizationProposals.AddAsync(proposal);
                await context.SaveChangesAsync();
            }

            using (var context = new FunderMapsDbContext(options))
            {
                var controller = new OrganizationProposalController(context, new LookupNormalizer());

                // Act
                var result = await controller.GetAsync(Guid.NewGuid());

                // Assert
                var okResult = Assert.IsType<ObjectResult>(result);
                var returnValue = Assert.IsType<ErrorOutputModel>(okResult.Value);
                Assert.Equal(1, returnValue.Errors.Count);
                Assert.Equal("Resource not found", returnValue.Errors[0].Message);
            }
        }

        [Fact]
        public async Task Post_ReturnsNewOrganizationProposal()
        {
            // Arrange
            var options = DatabaseWrapper.GenerateDatabase<FunderMapsDbContext>(nameof(Post_ReturnsNewOrganizationProposal));

            var token = Guid.NewGuid();
            var proposal = new OrganizationProposal("testcomp", "info@example.org")
            {
                Token = token,
            };

            using (var context = new FunderMapsDbContext(options))
            {
                var controller = new OrganizationProposalController(context, new LookupNormalizer());

                // Act
                var result = await controller.PostAsync(proposal);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnValue = Assert.IsType<OrganizationProposal>(okResult.Value);
                Assert.Equal(proposal.Name, returnValue.Name);
                Assert.Equal(proposal.Email, returnValue.Email);
            }
        }

        [Fact]
        public async Task Post_ReturnsConflict_GivenExistingName()
        {
            // Arrange
            var normalizer = new LookupNormalizer();
            var options = DatabaseWrapper.GenerateDatabase<FunderMapsDbContext>(nameof(Post_ReturnsConflict_GivenExistingName));

            var token = Guid.NewGuid();
            var proposal = new OrganizationProposal("testcomp", "info@example.org")
            {
                Token = token,
                NormalizedName = normalizer.Normalize("testcomp"),
            };

            using (var context = new FunderMapsDbContext(options))
            {
                await context.OrganizationProposals.AddAsync(proposal);
                await context.SaveChangesAsync();
            }

            using (var context = new FunderMapsDbContext(options))
            {
                var controller = new OrganizationProposalController(context, new LookupNormalizer());

                // Act
                var result = await controller.PostAsync(proposal);

                // Assert
                var okResult = Assert.IsType<ConflictObjectResult>(result);
                Assert.Equal(proposal.Name, okResult.Value);
            }
        }

        [Fact]
        public async Task Delete_ReturnsNothing()
        {
            // Arrange
            var options = DatabaseWrapper.GenerateDatabase<FunderMapsDbContext>(nameof(Delete_ReturnsNothing));

            var token = Guid.NewGuid();
            var proposal = new OrganizationProposal("testcomp", "info@example.org")
            {
                Token = token,
            };

            using (var context = new FunderMapsDbContext(options))
            {
                await context.OrganizationProposals.AddAsync(proposal);
                await context.SaveChangesAsync();
            }

            using (var context = new FunderMapsDbContext(options))
            {
                var controller = new OrganizationProposalController(context, new LookupNormalizer());

                // Act
                var result = await controller.DeleteAsync(token);

                // Assert
                var okResult = Assert.IsType<NoContentResult>(result);
            }
        }
    }
}
