// Copyright 2007-2016 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.PipeConfigurators
{
    using System.Collections.Generic;
    using Context;
    using GreenPipes;
    using GreenPipes.Filters;
    using Pipeline.Filters;
    using Saga;


    public class RetrySagaPipeSpecification<TSaga> :
        IPipeSpecification<SagaConsumeContext<TSaga>>
        where TSaga : class, ISaga
    {
        readonly IRetryPolicy _retryPolicy;

        public RetrySagaPipeSpecification(IRetryPolicy retryPolicy)
        {
            _retryPolicy = retryPolicy;
        }

        public void Apply(IPipeBuilder<SagaConsumeContext<TSaga>> builder)
        {
            var retryPolicy = new ConsumeContextRetryPolicy<SagaConsumeContext<TSaga>, RetrySagaConsumeContext<TSaga>>(_retryPolicy,
                x => new RetrySagaConsumeContext<TSaga>(x));

            builder.AddFilter(new RetryFilter<SagaConsumeContext<TSaga>>(retryPolicy));
        }

        public IEnumerable<ValidationResult> Validate()
        {
            if (_retryPolicy == null)
                yield return this.Failure("RetryPolicy", "must not be null");
        }
    }
}