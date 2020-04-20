using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BizCard.API.Controllers;
using BizCard.API.ViewModel;
using BizCard.Core.Data;
using BizCard.Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BizCard.API.UnitTests
{
    public class CardControllerTests
    {
        public CardControllerTests()
        {
        }


        [Fact]
        public async Task ShouldReturnExpectedCardInfo()
        {
            // Arrange
            var cardRepository = new Mock<IRepository<Core.Models.Card>>();
            var cards = new List<Card>() { new Card { Name = "unittest" } };
            cardRepository.Setup(x => x.All()).Returns(cards.AsQueryable<Card>());
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<List<Card>, List<CardViewModel>>(It.IsAny<List<Card>>()))
                .Returns(new List<CardViewModel> {
                    new CardViewModel { Name = "123" }
                });

            var controller = new CardController(cardRepository.Object, mapperMock.Object);


            // Act
            var result = await controller.CardInfo();

            // Assert
            result.Should().BeOfType<IActionResult>()
                .Which.As<OkObjectResult>().Value
                .As<CardViewModel>().Name.Should().Equals("123");
        }
    }
}
