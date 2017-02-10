using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;

using ProtoBuf;
using dataconfig;

public sealed class SaveLoadMgr
{
    public class DataWarpper
    {
        public PbPlayer PlayerInf;
        public List<PbFood> FoodsInf = new List<PbFood>();
    }

    private static SaveLoadMgr _instance = null;
    public static SaveLoadMgr Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new SaveLoadMgr();
            }
            return _instance;
        }
    }

    public DataWarpper Load()
    {
        PbGameFile tGameToSave = new PbGameFile();

#if !UNITY_EDITOR
        using (var file = File.Create("save.bin"))
        {
            tGameToSave = Serializer.NonGeneric.Deserialize(tGameToSave.GetType(), file) as PbGameFile;
        }
#else
        string tFileName = Application.persistentDataPath + "/save.bin";
        using (var file = File.OpenRead(tFileName))
        {
            tGameToSave = Serializer.NonGeneric.Deserialize(tGameToSave.GetType(), file) as PbGameFile;
        }
#endif

        DataWarpper tRet = new DataWarpper()
        {
            PlayerInf = tGameToSave.player,
        };

        tGameToSave.foods.ForEach((PbFood item) => 
        {
            tRet.FoodsInf.Add(item);
        });

        return tRet;
    }

    public void Save(SceneCtr scene)
    {
        PbGameFile tGameToSave = new PbGameFile();

        Player tPlayer = scene.Players[0];
        PbPlayer tPlayerData = new PbPlayer()
        {
            id = tPlayer.id,
            score = tPlayer.score
        };
        tPlayer.Body.ForEach((GameObject item) => 
        {
            tPlayerData.bodys.Add(new PbVector3()
            {
                x = item.transform.localPosition.x,
                y = item.transform.localPosition.y,
                z = item.transform.localPosition.z,
            });
        });

        tGameToSave.player = tPlayerData;

        List<GameObject> tFoodsGo = scene.Foods;
        tFoodsGo.ForEach((GameObject item) => 
        {
            FoodCellCtr tFoodCtr = item.GetComponent<FoodCellCtr>();
            tGameToSave.foods.Add(new PbFood()
            {
                score = tFoodCtr.Score,
                position = new PbVector3()
                {
                    x = item.transform.localPosition.x,
                    y = item.transform.localPosition.y,
                    z = item.transform.localPosition.z
                }
            });
        });

#if !UNITY_EDITOR
        using (var file = File.Create("save.bin"))
        {
            Serializer.NonGeneric.Serialize(file, tGameToSave);
        }
#else
        string tFileName = Application.persistentDataPath + "/save.bin";
        using (var file = File.Create(tFileName))
        {
            Serializer.NonGeneric.Serialize(file, tGameToSave);
            file.Flush();
        }
#endif
    }
}
