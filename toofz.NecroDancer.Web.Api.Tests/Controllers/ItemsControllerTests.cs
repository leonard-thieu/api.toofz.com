using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using toofz.NecroDancer.Data;
using toofz.NecroDancer.Web.Api.Controllers;
using toofz.NecroDancer.Web.Api.Models;
using toofz.TestsShared;

namespace toofz.NecroDancer.Web.Api.Tests.Controllers
{
    class ItemsControllerTests
    {
        static IEnumerable<Item> GetItems()
        {
            return new List<Item>
            {
                new Item
                {
                    ElementName = "armor_glass",
                    Name = "Glass Armor",
                    Slot = "body",
                    DiamondCost = 4,
                    CoinCost = 50,
                    IsArmor = true,
                },
                new Item
                {
                    ElementName = "misc_heart_container",
                    Name = "Heart Container",
                    Slot = null,
                    DiamondCost = null,
                    CoinCost = 50,
                    Consumable = true,
                },
                new Item
                {
                    ElementName = "feet_ballet_shoes",
                    Name = "Ballet Shoes",
                    Slot = "feet",
                    DiamondCost = null,
                    CoinCost = 25,
                },
                new Item
                {
                    ElementName = "food_1",
                    Name = "Apple",
                    Slot = "action",
                    DiamondCost = null,
                    CoinCost = 10,
                    IsFood = true,
                },
                new Item
                {
                    ElementName = "head_blast_helm",
                    Name = "Blast Helm",
                    Slot = "head",
                    DiamondCost = 5,
                    CoinCost = 60,
                },
                new Item
                {
                    ElementName = "ring_becoming",
                    Name = "Ring Of Becoming",
                    Slot = "ring",
                    DiamondCost = 5,
                    CoinCost = 100,
                },
                new Item
                {
                    ElementName = "scroll_earthquake",
                    Name = "Earthquake Scroll",
                    Slot = "action",
                    DiamondCost = 3,
                    CoinCost = 20,
                    IsScroll = true,
                },
                new Item
                {
                    ElementName = "spell_bomb",
                    Name = "Bomb Spell",
                    Slot = "spell",
                    DiamondCost = 6,
                    CoinCost = 150,
                    IsSpell = true,
                },
                new Item
                {
                    ElementName = "spell_fireball",
                    Name = "Fireball Spell",
                    Slot = "spell",
                    DiamondCost = 1,
                    CoinCost = 50,
                    IsSpell = true,
                },
                new Item
                {
                    ElementName = "torch_1",
                    Name = "Torch",
                    Slot = "torch",
                    DiamondCost = null,
                    CoinCost = 3,
                    IsTorch = true,
                },
                new Item
                {
                    ElementName = "weapon_axe",
                    Name = "Axe",
                    Slot = "weapon",
                    DiamondCost = 8,
                    CoinCost = 60,
                    IsWeapon = true,
                    IsAxe = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_bow",
                    Name = "Blood Bow",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 250,
                    IsWeapon = true,
                    IsBow = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_broadsword",
                    Name = "Blood Broadsword",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 40,
                    IsWeapon = true,
                    IsBroadsword = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_cat",
                    Name = "Blood Cat",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 85,
                    IsWeapon = true,
                    IsCat = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_crossbow",
                    Name = "Blood Crossbow",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 225,
                    IsWeapon = true,
                    IsCrossbow = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_dagger",
                    Name = "Blood Dagger",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 5,
                    IsWeapon = true,
                    IsDagger = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_flail",
                    Name = "Blood Flail",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 80,
                    IsWeapon = true,
                    IsFlail = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_longsword",
                    Name = "Blood Longsword",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 65,
                    IsWeapon = true,
                    IsLongsword = true,
                },
                new Item
                {
                    ElementName = "weapon_blood_rapier",
                    Name = "Blood Rapier",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 75,
                    IsWeapon = true,
                    IsRapier = true,
                },
                new Item
                {
                    ElementName = "weapon_golden_spear",
                    Name = "Golden Spear",
                    Slot = "weapon",
                    DiamondCost = null,
                    CoinCost = 100,
                    IsWeapon = true,
                    IsSpear = true,
                },
                new Item
                {
                    ElementName = "weapon_obsidian_whip",
                    Name = "Obsidian Whip",
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
        public class GetItemsMethod_ItemsPagination_CancellationToken
        {
            public GetItemsMethod_ItemsPagination_CancellationToken()
            {
                var items = GetItems();
                var mockDbItems = new MockDbSet<Item>(items);
                var dbItems = mockDbItems.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(dbItems);
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
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ItemsDTO>));
            }

            [TestMethod]
            public async Task ReturnsItems()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsDTO>)result;
                var contentItems = contentResult.Content.Items;
                Assert.AreEqual(10, contentItems.Count());
            }

            [TestMethod]
            public async Task ReturnsItemsOrderedByElementName()
            {
                // Arrange -> Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsDTO>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.AreEqual("armor_glass", first.Name);
            }

            [TestMethod]
            public async Task LimitIsLessThanResultsCount_ReturnsLimitResults()
            {
                // Arrange
                pagination.limit = 2;

                // Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsDTO>)result;
                var contentItems = contentResult.Content.Items;
                Assert.AreEqual(2, contentItems.Count());
            }

            [TestMethod]
            public async Task OffsetIsSpecified_ReturnsOffsetResults()
            {
                // Arrange
                pagination.offset = 2;

                // Act
                var result = await controller.GetItems(pagination);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsDTO>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.AreEqual("food_1", first.Name);
            }
        }

        [TestClass]
        public class GetItemsMethod_ItemsPagination_String_CancellationToken
        {
            public GetItemsMethod_ItemsPagination_String_CancellationToken()
            {
                var items = GetItems();
                var mockDbItems = new MockDbSet<Item>(items);
                var dbItems = mockDbItems.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(dbItems);
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
                var result = await controller.GetItems(pagination, category);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ItemsDTO>));
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
                var result = await controller.GetItems(pagination, category);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsDTO>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.AreEqual(name, first.Name);
            }
        }

        [TestClass]
        public class GetItemsMethod_ItemsPagination_String_String_CancellationToken
        {
            public GetItemsMethod_ItemsPagination_String_String_CancellationToken()
            {
                var items = GetItems();
                var mockDbItems = new MockDbSet<Item>(items);
                var dbItems = mockDbItems.Object;
                var mockDb = new Mock<NecroDancerContext>();
                mockDb.Setup(x => x.Items).Returns(dbItems);
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
                    category = category,
                    subcategory = subcategory
                };
                var result = await controller.GetItems(pagination, filter);

                // Assert
                Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ItemsDTO>));
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
                    category = category,
                    subcategory = subcategory
                };
                var result = await controller.GetItems(pagination, filter);

                // Assert
                var contentResult = (OkNegotiatedContentResult<ItemsDTO>)result;
                var contentItems = contentResult.Content.Items;
                var first = contentItems.First();
                Assert.AreEqual(name, first.Name);
            }
        }
    }
}
