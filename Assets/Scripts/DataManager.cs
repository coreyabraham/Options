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

    public void SaveData(int data)
    {

    }

    public void SaveData(float data)
    {

    }

    public void SaveData(bool data)
    {

    }

    public void SaveData(string data)
    {

    }


}
