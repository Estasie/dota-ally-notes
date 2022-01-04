using System;

namespace Dota2GSI.Nodes
{
    public interface IGameStateListener
    {
        bool Start();

        void Stop();

        event NewGameStateHandler NewGameState;
    }
}