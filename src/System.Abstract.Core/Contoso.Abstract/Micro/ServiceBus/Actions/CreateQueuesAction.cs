#region License
/*
The MIT License

Copyright (c) 2008 Sky Morey

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion
using Contoso.Abstract.Micro.ServiceBus.Impl;
namespace Contoso.Abstract.Micro.ServiceBus.Actions
{
    /// <summary>
    /// CreateQueuesAction
    /// </summary>
    public class CreateQueuesAction : AbstractCreateQueuesAction
    {
        private readonly IMicroServiceBus _serviceBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQueuesAction"/> class.
        /// </summary>
        /// <param name="queueStrategy">The queue strategy.</param>
        /// <param name="serviceBus">The service bus.</param>
        public CreateQueuesAction(IMicroQueueStrategy queueStrategy, IMicroServiceBus serviceBus)
            : base(queueStrategy)
        {
            _serviceBus = serviceBus;
        }

        /// <summary>
        /// Executes the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public override void Execute(string user)
        {
            CreateQueues(1, _serviceBus.Endpoint, user);
        }
    }
}