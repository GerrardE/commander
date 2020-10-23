using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{
    public interface ICommanderRepo
    {
        IEnumerable<Command> GetAllCommands();
        Command GetCommandById(int id);
        bool SaveChanges();
        void CreateCommand(Command cmd);
        void UpdateCommand(Command cmd);
    }
}