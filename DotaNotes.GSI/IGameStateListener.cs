using System;
using Microsoft.AspNetCore.Mvc;

namespace Dota2GSI.Nodes
{
    public interface IGameStateListener
    {
        bool Start();

        void Stop();

        event NewGameStateHandler NewGameState;
    }
}