﻿using MediatR;

namespace COO.Business.Logic.MMO.Read.GetGameServers
{
    public sealed class GetGameServersQuery : IRequest<GetGameServersResponseModel>
    {
        public GetGameServersQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}
