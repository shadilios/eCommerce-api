﻿using System.Linq.Expressions;

namespace eCommerce.Core.Specifications
{
    public interface ISpecification<T> 
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }
    }
}
