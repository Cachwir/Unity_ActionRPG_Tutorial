using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntitySaveLoadManager
{
    void Save();

    void Load();

    bool SaveFileExists();
}
