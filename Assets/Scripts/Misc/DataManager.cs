using UnityEngine;

public class DataManager
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }

            return instance;
        }
    }

    public void SaveData(string key, int data)
    {

    }

    public void SaveData(string key, float data)
    {

    }

    public void SaveData(string key, bool data)
    {

    }

    public void SaveData(string key, string data)
    {

    }

    public void GetData(string key)
    {

    }

    public void ResetData()
    {

    }
}
