using Models;

namespace Utilities
{
    public interface ILevelLoader
    {
        Level Load(string levelName);
    }
}