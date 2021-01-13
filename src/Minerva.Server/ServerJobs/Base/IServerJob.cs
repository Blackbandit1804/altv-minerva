﻿using System.Threading.Tasks;

namespace Minerva.Server.ServerJobs.Base
{
    public interface IServerJob
    {
        Task OnStartup();

        Task OnSave();

        Task OnShutdown();
    }
}
