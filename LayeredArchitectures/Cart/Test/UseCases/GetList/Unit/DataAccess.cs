using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartServiceTest.UseCases.GetList.Unit;

public class DataAccess
{
    [Test]
    public async Task
        GivenValidCartId_WhenQueryingForCartItems_ReturnListOfItems()
    {
        //Arrange 
        var fakeConfiguration = Substitute.For<IConfiguration>();
        fakeConfiguration.GetConnectionString(default!).ReturnsForAnyArgs(@"Filename=.\DummyDb");


        //Act


        //Assert
    }
}
