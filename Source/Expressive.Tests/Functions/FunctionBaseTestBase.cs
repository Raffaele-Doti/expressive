﻿using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;
using Expressive.Functions;
using Moq;
using NUnit.Framework;

namespace Expressive.Tests.Functions
{
    public abstract class FunctionBaseTestBase
    {
        protected abstract IFunction ActualFunction { get; }

        protected void AssertException(Type exceptionType, string exceptionMessage, params object[] values)
        {
            try
            {
                this.ActualFunction.Evaluate(
                    values?.Select(v => Mock.Of<IExpression>(e => e.Evaluate(It.IsAny<IDictionary<string, object>>()) == v)).ToArray(),
                    new Context(ExpressiveOptions.None));
            }
#pragma warning disable CA1031 // Do not catch general exception types - We will eventually switch to NUnit that should remove the need for this.
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Assert.That(e, Is.InstanceOf(exceptionType));
                Assert.That(exceptionMessage, Is.EqualTo(e.Message));
                return;
            }

            throw new InvalidOperationException($"An exception of type: {exceptionType} was expected but no exception was thrown");
        }

        protected object Evaluate(params object[] values)
        {
            return this.ActualFunction.Evaluate(
                values.Select(v => Mock.Of<IExpression>(e => e.Evaluate(It.IsAny<IDictionary<string, object>>()) == v)).ToArray(),
                new Context(ExpressiveOptions.None));
        }
    }
}
