using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.NecroDancer.Web.Api.Tests.Properties;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class ItemsControllerTests
    {
        static IEnumerable<Item> Items
        {
            get => new[]
            {
                new Item("armor_glass", "")
                {
                    Slot = "body",
                    DiamondCost = 4,
                    CoinCost = 50,
                    IsArmor = true,
                },
                new Item("misc_heart_container", "")
                {
                    Slot = null,
                    DiamondCost = null,
                    CoinCost = 50,
                    Consumable = true,
                },
                new Item("feet_ballet_shoes", "")
                {
                    Slot = "feet",
                    DiamondCost = null,
                    CoinCost = 25,
                },
                new Item("food_1", "")
                {
                    Slot = "action",
                    DiamondCost = null,
                    CoinCost = 10,
                    IsFood = true,
                },
                new Item("head_blast_helm","")
                {
                    Slot = "head",
                    DiamondCost = 5,
                    CoinCost = 60,
                },
                new Item("ring_becoming", "")
                {
                    Slot = "ring",
                    DiamondCost = 5,
                    CoinCost = 100,
                },
                new Item("scroll_earthquake", "")
                {
                    Slot = "action",
                    DiamondCost = 3,
                    CoinCost = 20,
                    IsScroll = true,
                },
                new Item("spell_bomb", "")
                {
                    Slot = "spell",
                    DiamondCost = 6,
                    CoinCost = 150,
                    IsSpell = true,
                },
                new Item("spell_fireball", "")
                {
                    Slot = "spell",
                    DiamondCost = 1,
                    CoinCost = 50,
                    IsSpell = true,
                },
                new Item("torch_1", "")
                {
                    Slot = "torch",
                    DiamondCost = null,
                    CoinCost = 3,
                    IsTorch = true,
                },
                new Item("weapon_axe", "")
                {
                    Slot = "weapon",
                    DiamondCost = 8,
                    CoinCost = 60,
                    IsWeapon = true,
                    IsAxe = true,
                },
                new Item("weapon_blood_bow", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 250,
                    IsWeapon = true,
                    IsBow = true,
                },
                new Item("weapon_blood_broadsword", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 40,
                    IsWeapon = true,
                    IsBroadsword = true,
                },
                new Item("weapon_blood_cat", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 85,
                    IsWeapon = true,
                    IsCat = true,
                },
                new Item("weapon_blood_crossbow", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 225,
                    IsWeapon = true,
                    IsCrossbow = true,
                },
                new Item("weapon_blood_dagger", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 5,
                    IsWeapon = true,
                    IsDagger = true,
                },
                new Item("weapon_blood_flail", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 80,
                    IsWeapon = true,
                    IsFlail = true,
                },
                new Item("weapon_blood_longsword", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 65,
                    IsWeapon = true,
                    IsLongsword = true,
                },
                new Item("weapon_blood_rapier", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 75,
                    IsWeapon = true,
                    IsRapier = true,
                },
                new Item("weapon_golden_spear", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 100,
                    IsWeapon = true,
                    IsSpear = true,
                },
                new Item("weapon_obsidian_whip", "")
                {
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 150,
                    IsWeapon = true,
                    IsWhip = true,
                },
            };
        }

        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var db = Mock.Of<NecroDancerContext>();

                // Act
                var controller = new ItemsController(db);

                // Assert
                Assert.IsInstanceOfType(controller, typeof(ItemsController));
            }
        }

        [TestClass]
        public class GetItemsMethod
        {
            public GetItemsMethod()
            {
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(items);
                var db = mockDb.Object;
                controller = new ItemsController(db);
                pagination = new ItemsPagination();
            }

            ItemsController controller;
            ItemsPagination pagination;

            [TestMethod]
            public async Task ReturnsOk()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ItemsEnvelope>));
            }

            [TestMethod]
            public async Task ReturnsItems()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                Assert.AreEqual(10, contentItems.Count());
            }

            [TestMethod]
            public async Task ReturnsItemsOrderedByElementName()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.AreEqual("armor_glass", first.Name);
            }

            [TestMethod]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.Limit = 2;

                // Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                Assert.AreEqual(2, contentItems.Count());
            }

            [TestMethod]
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
                Assert.AreEqual("food_1", first.Name);
            }
        }

        [TestClass]
        public class GetItemsByCategoryMethod
        {
            public GetItemsByCategoryMethod()
            {
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(items);
                var db = mockDb.Object;
                controller = new ItemsController(db);
                pagination = new ItemsPagination();
            }

            ItemsController controller;
            ItemsPagination pagination;

            [DataTestMethod]
            [DataRow("armor")]
            [DataRow("consumable")]
            [DataRow("feet")]
            [DataRow("food")]
            [DataRow("head")]
            [DataRow("rings")]
            [DataRow("scrolls")]
            [DataRow("spells")]
            [DataRow("torches")]
            [DataRow("weapons")]
            public async Task ReturnsOk(string category)
            {
                // Arrange -> Act
                var result = await controller.GetItemsByCategory(pagination, category);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ItemsEnvelope>));
            }

            [DataTestMethod]
            [DataRow("armor", "armor_glass")]
            [DataRow("consumable", "misc_heart_container")]
            [DataRow("feet", "feet_ballet_shoes")]
            [DataRow("food", "food_1")]
            [DataRow("head", "head_blast_helm")]
            [DataRow("rings", "ring_becoming")]
            [DataRow("scrolls", "scroll_earthquake")]
            [DataRow("spells", "spell_bomb")]
            [DataRow("torches", "torch_1")]
            [DataRow("weapons", "weapon_axe")]
            public async Task ReturnsItemsFilteredByCategory(string category, string name)
            {
                // Arrange -> Act
                var result = await controller.GetItemsByCategory(pagination, category);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsEnvelope>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.AreEqual(name, first.Name);
            }
        }

        [TestClass]
        public class GetItemsBySubcategoryMethod
        {
            public GetItemsBySubcategoryMethod()
            {
                var mockItems = new MockDbSet<Item>(Items);
                var items = mockItems.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(items);
                var db = mockDb.Object;
                controller = new ItemsController(db);
                pagination = new ItemsPagination();
            }

            ItemsController controller;
            ItemsPagination pagination;

            [DataTestMethod]
            [DataRow("weapons", "bows")]
            [DataRow("weapons", "broadswords")]
            [DataRow("weapons", "cats")]
            [DataRow("weapons", "crossbows")]
            [DataRow("weapons", "daggers")]
            [DataRow("weapons", "flails")]
            [DataRow("weapons", "longswords")]
            [DataRow("weapons", "rapiers")]
            [DataRow("weapons", "spears")]
            [DataRow("weapons", "whips")]
            [DataRow("chest", "red")]
            [DataRow("chest", "purple")]
            [DataRow("chest", "black")]
            [DataRow("chest", "mimic")]
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
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ItemsEnvelope>));
            }

            [DataTestMethod]
            [DataRow("weapons", "bows", "weapon_blood_bow")]
            [DataRow("weapons", "broadswords", "weapon_blood_broadsword")]
            [DataRow("weapons", "cats", "weapon_blood_cat")]
            [DataRow("weapons", "crossbows", "weapon_blood_crossbow")]
            [DataRow("weapons", "daggers", "weapon_blood_dagger")]
            [DataRow("weapons", "flails", "weapon_blood_flail")]
            [DataRow("weapons", "longswords", "weapon_blood_longsword")]
            [DataRow("weapons", "rapiers", "weapon_blood_rapier")]
            [DataRow("weapons", "spears", "weapon_golden_spear")]
            [DataRow("weapons", "whips", "weapon_obsidian_whip")]
            [DataRow("chest", "red", "food_1")]
            [DataRow("chest", "purple", "ring_becoming")]
            [DataRow("chest", "black", "armor_glass")]
            [DataRow("chest", "mimic", "armor_glass")]
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
                Assert.AreEqual(name, first.Name);
            }
        }

        [TestClass]
        public class DisposeMethod
        {
            [TestMethod]
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

            [TestMethod]
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

        [TestClass]
        public class IntegrationTests : IntegrationTestsBase
        {
            [TestMethod]
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
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.That.NormalizedAreEqual(Resources.GetItems, content);
            }

            [TestMethod]
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
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.That.NormalizedAreEqual(Resources.GetItemsByCategory, content);
            }

            [TestMethod]
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
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.That.NormalizedAreEqual(Resources.GetItemsBySubcategory, content);
            }
        }
    }
}
