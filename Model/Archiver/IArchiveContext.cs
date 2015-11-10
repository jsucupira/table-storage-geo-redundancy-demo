using System.Collections.Generic;

namespace Model.Archiver
{
    public interface IArchiveContext
    {
        List<Archive> FindAll();
        Archive Save(Archive archive);
    }
}