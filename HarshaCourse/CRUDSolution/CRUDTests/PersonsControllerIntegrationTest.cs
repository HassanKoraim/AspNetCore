﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {
            // Arrange

            //Act 
            HttpResponseMessage response = await _client.GetAsync("/Persons/Index");

            //Assert
            response.Should().BeSuccessful();
        }
        #endregion
    }
}
