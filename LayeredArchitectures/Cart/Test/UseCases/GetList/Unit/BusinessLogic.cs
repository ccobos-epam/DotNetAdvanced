using NSubstitute;
using CartService.UseCases.GetList;
using CartService.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace CartServiceTest.UseCases.GetList.Unit;


public class BusinessLogic
{
    [Test]
    public async Task 
        GivenValidIdOfCart_WhenQueryingForItems_ReturnListOfItems()
    {
        //Arrange 
        var fakeDataAccess = Substitute.For<IDataAccess>();
        List<ItemEntity> fakeResult = [new ItemEntity {
            Id = 1, Name = "Test", Price = 45m, Quantity = 2
        }];
        fakeDataAccess.GetListOfItemsinCart(default).ReturnsForAnyArgs(fakeResult);
        var sut = new CartService.UseCases.GetList.BusinessLogic() { DataAccess = fakeDataAccess };

        //Act
        var result = await sut.GetListOfItemsinCart(Guid.Empty);

        //Assert
        await Assert.That(result.Results.Count).IsEqualTo(1);
        await fakeDataAccess.ReceivedWithAnyArgs().GetListOfItemsinCart(default);
    }
}
