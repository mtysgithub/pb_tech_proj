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

    static dataconfig.GAMEPARAMSCONFIG_CONF_ARRAY GameParamConfig = null;

    public dataconfig.GAMEPARAMSCONFIG_CONF_ARRAY LoadConfig()
    {
        if (GameParamConfig != null)
        {
            return GameParamConfig;
        }

        dataconfig.GAMEPARAMSCONFIG_CONF_ARRAY tRet = null;
        UnityEngine.Object tConfigFile = Resources.Load("Config/dataconfig_gameparamsconfig_conf");
        if (tConfigFile != null)
        {
            byte [] tData = ((TextAsset)tConfigFile).bytes;
            using (var stream = new System.IO.MemoryStream(tData))
            {
                tRet = Serializer.Deserialize<dataconfig.GAMEPARAMSCONFIG_CONF_ARRAY>(stream);
                stream.Flush();
            }
        }
        return GameParamConfig = tRet;
    }

    public DataWarpper Load()
    {
        PbGameFile tGameToSave = new PbGameFile();

        string tFileName = Application.persistentDataPath + "/save.bin";
        using (var file = File.OpenRead(tFileName))
        {
            tGameToSave = Serializer.NonGeneric.Deserialize(tGameToSave.GetType(), file) as PbGameFile;
        }

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

        string tFileName = Application.persistentDataPath + "/save.bin";
        using (var file = File.Create(tFileName))
        {
            Serializer.NonGeneric.Serialize(file, tGameToSave);
            file.Flush();
        }
    }
}
