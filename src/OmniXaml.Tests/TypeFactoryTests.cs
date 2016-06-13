using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmniXaml.Tests
{
    using Testing.Classes;
    using Xunit;

    public class TypeFactoryTests
    {
        [Fact]
        public void NoDefaultCtor_WitNoArgs()
        {
            var sut = new TypeFactory();

            Assert.False(sut.CanCreate(typeof(ImmutableDummy)));
        }

        [Fact]
        public void NoDefaultCtor_WithCorrectArgs()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreate(typeof(ImmutableDummy), "some string"));
        }

        [Fact]
        public void NoDefaultCtor_WithIncorrectArgs()
        {
            var sut = new TypeFactory();

            Assert.True(sut.CanCreate(typeof(ImmutableDummy), "some string", "other string"));
        }
    }
}
