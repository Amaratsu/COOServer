using System.Collections.Generic;
using MediatR;

namespace COO.Business.Logic.MMO.Write.GetGameServers
{
    public class GetGameServersResponseModel : IRequest<Unit>
    {
        public List<GameServer> GameServers { get; set; }
    }
}
