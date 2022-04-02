using System;
using FluentAssertions;
using Stores.Domain;
using Xunit;

namespace Stores.UnitTests.Domain
{
    public class DomainTests
    {
        [Fact]
        public void ChangeSubdomain_NewStore_SubdomainChanged()
        {
            // arrange
            var subdomain1 = "example";
            var subdomain2 = "changed";

            // act
            var store = new Store(Guid.NewGuid().ToString(), "x", subdomain1);
            store.ChangeSubdomain(subdomain2);
            
            // assert
            store.Subdomain.Should().Be(subdomain2);
        }
        
        [Fact]
        public void ChangeDomain_NewStore_DomainChanged()
        {
            // arrange
            var domain = "example.com";

            // act
            var store = new Store(Guid.NewGuid().ToString(), "x", "x");
            store.ChangeDomain(domain);
            
            // assert
            store.Domain.Should().Be(domain);
        }
    }
}