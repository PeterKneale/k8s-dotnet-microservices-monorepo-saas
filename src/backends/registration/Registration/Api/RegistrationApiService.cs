using System;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Registration.Application;
using Registration.Messages;

namespace Registration.Api
{
    public class RegistrationApiService : RegistrationApi.RegistrationApiBase
    {
        private readonly IRegistrationQueue _queue;

        public RegistrationApiService(IRegistrationQueue queue)
        {
            _queue = queue;
        }

        public override async Task<Empty> SubmitRegistration(SubmitRegistrationRequest request, ServerCallContext context)
        {
            var name = request.Name;
            var email = request.Email;
            var reference = request.Reference;

            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NullOrWhiteSpace(reference, nameof(reference));

            await _queue.Enqueue(new RegistrationRequest
            {
                StoreId = Guid.NewGuid().ToString(),
                AccountId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Name = name,
                Email = email,
                Reference = reference
            }, context.CancellationToken);
            
            return new Empty();
        }

        public override Task<GetRegistrationStatusReply> GetRegistrationStatus(GetRegistrationStatusRequest request, ServerCallContext context)
        {
            var reference = request.Reference;

            Guard.Against.NullOrWhiteSpace(reference, nameof(reference));

            return Task.FromResult(new GetRegistrationStatusReply {Complete = true});
        }
    }
}