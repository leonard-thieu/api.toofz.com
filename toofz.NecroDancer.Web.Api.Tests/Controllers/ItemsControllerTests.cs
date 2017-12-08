using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using Xunit;
using Xunit.Abstractions;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    [Collection(MockDatabaseCollection.Name)]
    public class ItemsControllerTests
    {
        public ItemsControllerTests(MockDatabaseFixture fixture)
        {
            mockDb = fixture.CreateMockNecroDancerContext();
            controller = new ItemsController(mockDb.Object);
        }

        private readonly Mock<INecroDancerContext> mockDb;
        private readonly ItemsController controller;

        public class Constructor
        {
            [DisplayFact(nameof(ItemsController))]
            public void ReturnsItemsController()
            {
                // Arrange
                var db = Mock.Of<NecroDancerContext>();

                // Act
                var controller = new ItemsController(db);

                // Assert
                Assert.IsAssignableFrom<ItemsController>(controller);
            }
        }

        public class GetItemsMethod : ItemsControllerTests
        {
            public GetItemsMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly ItemsPagination pagination = new ItemsPagination();

            [DisplayFact(nameof(HttpStatusCode.OK))]
            public async Task ReturnsOK()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ItemsEnvelope>>(result);
            }

            [DisplayFact]
            public async Task ReturnsItems()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                Assert.Equal(10, contentItems.Count());
            }

            [DisplayFact]
            public async Task ReturnsItemsOrderedByElementName()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.Equal("addchest_black", first.Name);
            }

            [DisplayFact]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.Limit = 2;

                // Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                Assert.Equal(2, contentItems.Count());
            }

            [DisplayFact]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                pagination.Offset = 2;

                // Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.Equal("addchest_white", first.Name);
            }
        }

        public class GetItemsByCategoryMethod : ItemsControllerTests
        {
            public GetItemsByCategoryMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly ItemsPagination pagination = new ItemsPagination();

            [DisplayTheory(nameof(HttpStatusCode.OK))]
            [InlineData("armor")]
            [InlineData("consumable")]
            [InlineData("feet")]
            [InlineData("food")]
            [InlineData("head")]
            [InlineData("rings")]
            [InlineData("scrolls")]
            [InlineData("spells")]
            [InlineData("torches")]
            [InlineData("weapons")]
            public async Task ReturnsOK(string category)
            {
                // Arrange -> Act
                var result = await controller.GetItemsByCategory(pagination, category);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ItemsEnvelope>>(result);
            }

            [DisplayTheory]
            [InlineData("armor", "armor_chainmail")]
            [InlineData("consumable", "misc_heart_container")]
            [InlineData("feet", "feet_ballet_shoes")]
            [InlineData("food", "food_1")]
            [InlineData("head", "head_blast_helm")]
            [InlineData("rings", "ring_becoming")]
            [InlineData("scrolls", "scroll_earthquake")]
            [InlineData("spells", "spell_bomb")]
            [InlineData("torches", "torch_1")]
            [InlineData("weapons", "weapon_axe")]
            public async Task ReturnsItemsFilteredByCategory(string category, string name)
            {
                // Arrange -> Act
                var result = await controller.GetItemsByCategory(pagination, category);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.Equal(name, first.Name);
            }
        }

        public class GetItemsBySubcategoryMethod : ItemsControllerTests
        {
            public GetItemsBySubcategoryMethod(MockDatabaseFixture fixture) : base(fixture) { }

            private readonly ItemsPagination pagination = new ItemsPagination();

            [DisplayTheory(nameof(HttpStatusCode.OK))]
            [InlineData("weapons", "bows")]
            [InlineData("weapons", "broadswords")]
            [InlineData("weapons", "cats")]
            [InlineData("weapons", "crossbows")]
            [InlineData("weapons", "daggers")]
            [InlineData("weapons", "flails")]
            [InlineData("weapons", "longswords")]
            [InlineData("weapons", "rapiers")]
            [InlineData("weapons", "spears")]
            [InlineData("weapons", "whips")]
            [InlineData("chest", "red")]
            [InlineData("chest", "purple")]
            [InlineData("chest", "black")]
            [InlineData("chest", "mimic")]
            public async Task ReturnsOK(string category, string subcategory)
            {
                // Arrange
                var filter = new ItemSubcategoryFilter
                {
                    Category = category,
                    Subcategory = subcategory
                };
                var result = await controller.GetItemsBySubcategory(pagination, filter);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ItemsEnvelope>>(result);
            }

            [DisplayTheory]
            [InlineData("weapons", "bows", "weapon_blood_bow")]
            [InlineData("weapons", "broadswords", "weapon_blood_broadsword")]
            [InlineData("weapons", "cats", "weapon_blood_cat")]
            [InlineData("weapons", "crossbows", "weapon_blood_crossbow")]
            [InlineData("weapons", "daggers", "weapon_blood_dagger")]
            [InlineData("weapons", "flails", "weapon_blood_flail")]
            [InlineData("weapons", "longswords", "weapon_blood_longsword")]
            [InlineData("weapons", "rapiers", "weapon_blood_rapier")]
            [InlineData("weapons", "spears", "weapon_blood_spear")]
            [InlineData("weapons", "whips", "weapon_blood_whip")]
            [InlineData("chest", "red", "bag_holding")]
            [InlineData("chest", "purple", "ring_becoming")]
            [InlineData("chest", "black", "armor_chainmail")]
            [InlineData("chest", "mimic", "addchest_black")]
            public async Task ReturnsItemsFilteredBySubcategory(string category, string subcategory, string name)
            {
                // Arrange -> Act
                var filter = new ItemSubcategoryFilter
                {
                    Category = category,
                    Subcategory = subcategory
                };
                var result = await controller.GetItemsBySubcategory(pagination, filter);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.Equal(name, first.Name);
            }
        }

        public class DisposeMethod : ItemsControllerTests
        {
            public DisposeMethod(MockDatabaseFixture fixture) : base(fixture) { }

            [DisplayFact]
            public void DisposesDb()
            {
                // Arrange -> Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [DisplayFact]
            public void DisposeMoreThanOnce_DisposesDbOnlyOnce()
            {
                // Arrange -> Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            public IntegrationTests(IntegrationTestsFixture fixture, ITestOutputHelper output) : base(fixture, output) { }

            [DisplayFact]
            public async Task GetItems()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/items");

                // Assert
                await RespondsWithAsync(response, Resources.GetItems);
            }

            [DisplayFact]
            public async Task GetItemsByCategory()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/items/armor");

                // Assert
                await RespondsWithAsync(response, Resources.GetItemsByCategory);
            }

            [DisplayFact]
            public async Task GetItemsBySubcategory()
            {
                // Arrange -> Act
                var response = await server.HttpClient.GetAsync("/items/weapons/bows");

                // Assert
                await RespondsWithAsync(response, Resources.GetItemsBySubcategory);
            }
        }
    }
}
