using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using toofz.TestsShared;
using Xunit;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    public class ItemsControllerTests
    {
        private static IEnumerable<Item> Items
        {
            get => new[]
            {
                new Item("armor_glass", "")
                {
                    Slot = "body",
                    DiamondCost = 4,
                    CoinCost = 50,
                    IsArmor = true,
                    DisplayName = "Glass Armor",
                },
                new Item("misc_heart_container", "")
                {
                    Slot = null,
                    DiamondCost = null,
                    CoinCost = 50,
                    Consumable = true,
                    DisplayName = "Heart Container",
                },
                new Item("feet_ballet_shoes", "")
                {
                    Slot = "feet",
                    DiamondCost = null,
                    CoinCost = 25,
                    DisplayName = "Ballet Shoes",
                },
                new Item("food_1", "")
                {
                    Slot = "action",
                    DiamondCost = null,
                    CoinCost = 10,
                    IsFood = true,
                    DisplayName = "Apple",
                },
                new Item("head_blast_helm","")
                {
                    Slot = "head",
                    DiamondCost = 5,
                    CoinCost = 60,
                    DisplayName = "Blast Helm",
                },
                new Item("ring_becoming", "")
                {
                    Slot = "ring",
                    DiamondCost = 5,
                    CoinCost = 100,
                    DisplayName = "Ring of Becoming",
                },
                new Item("scroll_earthquake", "")
                {
                    Slot = "action",
                    DiamondCost = 3,
                    CoinCost = 20,
                    IsScroll = true,
                    DisplayName = "Earthquake Scroll",
                },
                new Item("spell_bomb", "")
                {
                    Slot = "spell",
                    DiamondCost = 6,
                    CoinCost = 150,
                    IsSpell = true,
                    DisplayName = "Bomb Spell",
                },
                new Item("spell_fireball", "")
                {
                    Slot = "spell",
                    DiamondCost = 1,
                    CoinCost = 50,
                    IsSpell = true,
                    DisplayName = "Fireball Spell",
                },
                new Item("torch_1", "")
                {
                    Slot = "torch",
                    DiamondCost = null,
                    CoinCost = 3,
                    IsTorch = true,
                    DisplayName = "Torch",
                },
                new Item("weapon_axe", "")
                {
                    Slot = "weapon",
                    DiamondCost = 8,
                    CoinCost = 60,
                    IsWeapon = true,
                    IsAxe = true,
                    DisplayName = "Axe",
                },
                new Item("weapon_blood_bow", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 250,
                    IsWeapon = true,
                    IsBow = true,
                    DisplayName = "Blood Bow",
                },
                new Item("weapon_blood_broadsword", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 40,
                    IsWeapon = true,
                    IsBroadsword = true,
                    DisplayName = "Blood Broadsword",
                },
                new Item("weapon_blood_cat", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 85,
                    IsWeapon = true,
                    IsCat = true,
                    DisplayName = "Blood Cat",
                },
                new Item("weapon_blood_crossbow", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 225,
                    IsWeapon = true,
                    IsCrossbow = true,
                    DisplayName = "Blood Crossbow",
                },
                new Item("weapon_blood_dagger", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 5,
                    IsWeapon = true,
                    IsDagger = true,
                    DisplayName = "Blood Dagger",
                },
                new Item("weapon_blood_flail", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 80,
                    IsWeapon = true,
                    IsFlail = true,
                    DisplayName = "Blood Flail",
                },
                new Item("weapon_blood_longsword", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 65,
                    IsWeapon = true,
                    IsLongsword = true,
                    DisplayName = "Blood Longsword",
                },
                new Item("weapon_blood_rapier", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 75,
                    IsWeapon = true,
                    IsRapier = true,
                    DisplayName = "Blood Rapier",
                },
                new Item("weapon_golden_spear", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 100,
                    IsWeapon = true,
                    IsSpear = true,
                    DisplayName = "Golden Spear",
                },
                new Item("weapon_obsidian_whip", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 150,
                    IsWeapon = true,
                    IsWhip = true,
                    DisplayName = "Obsidian Whip",
                },
            };
        }

        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var db = Mock.Of<NecroDancerContext>();

                // Act
                var controller = new ItemsController(db);

                // Assert
                Assert.IsAssignableFrom<ItemsController>(controller);
            }
        }

        public class GetItemsMethod
        {
            public GetItemsMethod()
            {
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(items);
                var db = mockDb.Object;
                controller = new ItemsController(db);
                pagination = new ItemsPagination();
            }

            private ItemsController controller;
            private ItemsPagination pagination;

            [Fact]
            public async Task ReturnsOk()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ItemsEnvelope>>(result);
            }

            [Fact]
            public async Task ReturnsItems()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                Assert.Equal(10, contentItems.Count());
            }

            [Fact]
            public async Task ReturnsItemsOrderedByElementName()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.Equal("armor_glass", first.Name);
            }

            [Fact]
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

            [Fact]
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
                Assert.Equal("food_1", first.Name);
            }
        }

        public class GetItemsByCategoryMethod
        {
            public GetItemsByCategoryMethod()
            {
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(items);
                var db = mockDb.Object;
                controller = new ItemsController(db);
                pagination = new ItemsPagination();
            }

            private ItemsController controller;
            private ItemsPagination pagination;

            [Theory]
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
            public async Task ReturnsOk(string category)
            {
                // Arrange -> Act
                var result = await controller.GetItemsByCategory(pagination, category);

                // Assert
                Assert.IsAssignableFrom<OkNegotiatedContentResult<ItemsEnvelope>>(result);
            }

            [Theory]
            [InlineData("armor", "armor_glass")]
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

        public class GetItemsBySubcategoryMethod
        {
            public GetItemsBySubcategoryMethod()
            {
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(items);
                var db = mockDb.Object;
                controller = new ItemsController(db);
                pagination = new ItemsPagination();
            }

            private ItemsController controller;
            private ItemsPagination pagination;

            [Theory]
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
            public async Task ReturnsOk(string category, string subcategory)
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

            [Theory]
            [InlineData("weapons", "bows", "weapon_blood_bow")]
            [InlineData("weapons", "broadswords", "weapon_blood_broadsword")]
            [InlineData("weapons", "cats", "weapon_blood_cat")]
            [InlineData("weapons", "crossbows", "weapon_blood_crossbow")]
            [InlineData("weapons", "daggers", "weapon_blood_dagger")]
            [InlineData("weapons", "flails", "weapon_blood_flail")]
            [InlineData("weapons", "longswords", "weapon_blood_longsword")]
            [InlineData("weapons", "rapiers", "weapon_blood_rapier")]
            [InlineData("weapons", "spears", "weapon_golden_spear")]
            [InlineData("weapons", "whips", "weapon_obsidian_whip")]
            [InlineData("chest", "red", "food_1")]
            [InlineData("chest", "purple", "ring_becoming")]
            [InlineData("chest", "black", "armor_glass")]
            [InlineData("chest", "mimic", "armor_glass")]
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

        public class DisposeMethod
        {
            [Fact]
            public void DisposesDb()
            {
                // Arrange
                var mockDb = new Mock<INecroDancerContext>();
                var db = mockDb.Object;
                var controller = new ItemsController(db);

                // Act
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }

            [Fact]
            public void DisposeMoreThanOnce_DisposesDbOnlyOnce()
            {
                // Arrange
                var mockDb = new Mock<INecroDancerContext>();
                var db = mockDb.Object;
                var controller = new ItemsController(db);

                // Act
                controller.Dispose();
                controller.Dispose();

                // Assert
                mockDb.Verify(d => d.Dispose(), Times.Once);
            }
        }

        public class IntegrationTests : IntegrationTestsBase
        {
            [Fact]
            public async Task GetItemsMethod()
            {
                // Arrange
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.SetupGet(d => d.Items).Returns(items);
                var db = mockDb.Object;
                Kernel.Rebind<INecroDancerContext>().ToConstant(db);

                // Act
                var response = await Server.HttpClient.GetAsync("/items");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(Resources.GetItems, content, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public async Task GetItemsByCategoryMethod()
            {
                // Arrange
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.SetupGet(d => d.Items).Returns(items);
                var db = mockDb.Object;
                Kernel.Rebind<INecroDancerContext>().ToConstant(db);

                // Act
                var response = await Server.HttpClient.GetAsync("/items/armor");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(Resources.GetItemsByCategory, content, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public async Task GetItemsBySubcategoryMethod()
            {
                // Arrange
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<INecroDancerContext>();
                mockDb.SetupGet(d => d.Items).Returns(items);
                var db = mockDb.Object;
                Kernel.Rebind<INecroDancerContext>().ToConstant(db);

                // Act
                var response = await Server.HttpClient.GetAsync("/items/weapons/bows");
                var content = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(Resources.GetItemsBySubcategory, content, ignoreLineEndingDifferences: true);
            }
        }
    }
}
